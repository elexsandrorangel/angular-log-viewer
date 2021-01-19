using AutoMapper;
using LogViewer.Business.Interfaces;
using LogViewer.Infrastructure.Exceptions;
using LogViewer.Models.SearchModels;
using LogViewer.Models.ViewModels;
using LogViewer.Repository.Entities;
using LogViewer.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace LogViewer.Business.Implementations
{
    public abstract class BaseBusiness<TEntity, TModel, TSearch, TRepo> : IBaseBusiness<TEntity, TModel, TSearch>
        where TEntity : BaseEntity
        where TModel : BaseViewModel
        where TSearch : BaseSearchModel
        where TRepo : IBaseRepository<TEntity>
    {
        #region Fields

        protected readonly TRepo Repository;

        protected readonly IMapper _mapper;

        #endregion Fields

        #region Ctor

        protected BaseBusiness(TRepo repository, IMapper mapper)
        {
            Repository = repository;
            _mapper = mapper;
        }

        #endregion Ctor

        #region Mapper

        /// <summary>
        /// Converts a entity object to value object
        /// </summary>
        /// <param name="t"><paramref name="t"/> Entity class</param>
        /// <returns>Converted model</returns>
        internal TModel ModelFromEntity(TEntity t)
        {
            return t == null ? null : _mapper.Map<TModel>(t);
        }

        /// <summary>
        /// Converts a list of entities object to value object enumerable
        /// </summary>
        /// <param name="entities"><paramref name="entities"/> Entity class</param>
        /// <returns></returns>
        internal IEnumerable<TModel> ModelFromEntity(IEnumerable<TEntity> entities)
        {
            return entities == null ? null : _mapper.Map<IEnumerable<TModel>>(entities);
        }

        internal TEntity EntityFromModel(TModel model)
        {
            return _mapper.Map<TEntity>(model);
        }

        internal IEnumerable<TEntity> EntityFromModel(IEnumerable<TModel> models)
        {
            return _mapper.Map<IEnumerable<TEntity>>(models);
        }

        #endregion Mapper

        #region Add

        public virtual async Task<TModel> AddAsync(TModel t)
        {
            ValidateInsert(t);

            t.CreatedAt = DateTime.UtcNow;
            t.ModifiedAt = DateTime.UtcNow;
            //if (string.IsNullOrEmpty(t.CreatedBy))
            //{
            //    t.CreatedBy = t.ModifiedBy;
            //}
            //t.ModifiedAt = DateTime.UtcNow;
            t.Active = true;
            return ModelFromEntity(await Repository.AddAsync(EntityFromModel(t)));
        }

        public virtual async Task<IEnumerable<TModel>> AddAsync(IEnumerable<TModel> tList)
        {
            var list = tList;

            ValidateInsert(tList);

            foreach (var item in list)
            {
                item.CreatedAt = DateTime.UtcNow;
                item.ModifiedAt = DateTime.UtcNow;
                item.Active = true;
            }

            return ModelFromEntity(await Repository.AddAsync(EntityFromModel(list)));
        }

        #endregion Add

        #region Count

        public virtual async Task<long> CountAsync()
        {
            return await Repository.CountAsync();
        }

        #endregion Count

        #region Delete

        public virtual async Task DeleteAsync(int id)
        {
            //var record = await GetAsync(id);

            //if (record == null)
            //{
            //    throw new LogViewerNotFoundException();
            //}

            //await DeleteAsync(record);
            //await ValidateDeleteAsync(await GetAsync(id));
            //await Repository.DeleteAsync(id);

            await InactivateAsync(id, null);
        }

        public virtual async Task DeleteAsync(TModel t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            //await ValidateDeleteAsync(t);
            //await Repository.DeleteAsync(EntityFromModel(t));

            await ValidateDeleteAsync(t);

            await InactivateAsync(t.Id, null);
        }

        #endregion Delete

        #region Get

        public virtual async Task<IEnumerable<TModel>> GetAsync(int page = 1, int qty = int.MaxValue)
        {
            try
            {
                if (page > 0)
                {
                    page -= 1;
                }
                else
                {
                    page = 0;
                }
                return ModelFromEntity(await Repository.GetAsync(page, qty, false));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<TModel> GetAsync(int id, bool track = false)
        {
            return ModelFromEntity(await Repository.GetAsync(id, track));
        }

        public virtual async Task<IEnumerable<TModel>> SearchAsync(TSearch criterias)
        {
            if (criterias == null)
            {
                throw new ArgumentNullException(nameof(criterias));
            }

            #region Expression Tree

            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "s");

            List<Expression> expressions = new List<Expression>();
            if (criterias.Active.HasValue)
            {
                MemberExpression me = Expression.Property(pe, "Active");
                ConstantExpression ce = Expression.Constant(criterias.Active.Value);
                BinaryExpression be = Expression.Equal(me, ce);
                expressions.Add(be);
            }

            if (!string.IsNullOrEmpty(criterias.Name))
            {
                Expression propertyExp = Expression.Property(pe, "Name");

                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var someValue = Expression.Constant(criterias.Name, typeof(string));
                var containsMethodExp = Expression.Call(propertyExp, method, someValue);
                expressions.Add(containsMethodExp);
            }

            if (criterias.CreatedDateStart.HasValue)
            {
                MemberExpression me = Expression.Property(pe, "CreatedAt");
                var criterionConstantMin = Expression.Constant(criterias.CreatedDateStart.Value, typeof(DateTime?));

                var greaterThanCall = Expression.GreaterThanOrEqual(me, criterionConstantMin);
                expressions.Add(greaterThanCall);
            }

            if (criterias.CreatedDateEnd.HasValue)
            {
                MemberExpression me = Expression.Property(pe, "CreatedAt");
                var criterionConstantMax = Expression.Constant(criterias.CreatedDateEnd.Value, typeof(DateTime?));

                var lessThanCall = Expression.LessThanOrEqual(me, criterionConstantMax);
                expressions.Add(lessThanCall);
            }

            if (criterias.ModifiedDateStart.HasValue)
            {
                MemberExpression me = Expression.Property(pe, "ModifiedAt");
                var criterionConstantMin = Expression.Constant(criterias.ModifiedDateStart.Value, typeof(DateTime?));

                var greaterThanCall = Expression.GreaterThanOrEqual(me, criterionConstantMin);
                expressions.Add(greaterThanCall);
            }

            if (criterias.ModifiedDateEnd.HasValue)
            {
                MemberExpression me = Expression.Property(pe, "ModifiedDate");
                var criterionConstantMax = Expression.Constant(criterias.ModifiedDateEnd.Value, typeof(DateTime?));

                var lessThanCall = Expression.LessThanOrEqual(me, criterionConstantMax);
                expressions.Add(lessThanCall);
            }

            // Prevents aggregate exceptions with empty list
            if (!expressions.Any())
            {
                return null;
            }

            Expression<Func<TEntity, bool>> finalExpression =
                Expression.Lambda<Func<TEntity, bool>>(expressions.Aggregate(Expression.AndAlso), pe);

            #endregion Expression Tree

            var result = await Repository.GetAsync(finalExpression, criterias.Page, criterias.Quantity);

            return ModelFromEntity(result);
        }

        #endregion Get

        #region Save

        public virtual async Task<TModel> SaveOrUpdateAsync(TModel t)
        {
            return t?.Id == 0 ? await AddAsync(t) : await UpdateAsync(t);
        }

        #endregion Save

        #region Update

        public virtual async Task<bool> InactivateAsync(int id, int? inactivatedBy)
        {
            TModel obj = await GetAsync(id, false);
            if (obj == null)
            {
                throw new LogViewerNotFoundException();
            }

            await ValidateDeleteAsync(obj);

            obj.Active = false;
            obj.ModifiedAt = DateTime.UtcNow;
            //await UpdateAsync(obj);
            await Repository.UpdateAsync(EntityFromModel(obj));
            return true;
        }

        public virtual async Task<TModel> UpdateAsync(TModel updated)
        {
            await ValidateUpdateAsync(updated);

            TModel record = await GetAsync(updated.Id, false);

            if (record == null)
            {
                throw new LogViewerNotFoundException();
            }

            updated.CreatedAt = record.CreatedAt;
            updated.ModifiedAt = DateTime.UtcNow;

            var data = await Repository.UpdateAsync(EntityFromModel(updated));
            return ModelFromEntity(data);
        }

        public virtual async Task<TModel> UpdateAsync(TModel updated, int key)
        {
            await ValidateUpdateAsync(updated);

            TModel record = await GetAsync(key, true);

            if (record == null)
            {
                throw new LogViewerNotFoundException();
            }

            updated.Id = key;

            updated.CreatedAt = record.CreatedAt;

            updated.ModifiedAt = DateTime.UtcNow;
            var data = await Repository.UpdateAsync(EntityFromModel(updated), key);
            return ModelFromEntity(data);
        }

        #endregion Update

        #region Validations

        /// <summary>
        /// Data validation before insert model into database
        /// </summary>
        /// <param name="model">Model to insert</param>
        /// <exception cref="LogViewerException">Business exception</exception>
        protected abstract void ValidateInsert(TModel model);

        /// <summary>
        /// Data validation before insert model into database
        /// </summary>
        /// <param name="models">Models to insert</param>
        /// <exception cref="LogViewerException">Business exception</exception>
        protected virtual void ValidateInsert(IEnumerable<TModel> models)
        {
            foreach (var item in models)
            {
                ValidateInsert(item);
            }
        }

        /// <summary>
        /// Data validation before update model into database
        /// </summary>
        /// <param name="model">Model to update</param>
        /// <remarks>Asynchronous</remarks>
        /// <exception cref="LogViewerException">Business exception</exception>
        protected virtual async Task ValidateUpdateAsync(TModel model)
        {
            if (model == null || model?.Id == 0)
            {
                throw new LogViewerException();
            }
            var record = await GetAsync(model.Id, false);
            if (record == null)
            {
                throw new LogViewerNotFoundException();
            }

            ValidateInsert(model);
        }

        /// <summary>
        /// Data validation before remove model from database
        /// </summary>
        /// <param name="model">Model to remove</param>
        /// <remarks>Asynchronous</remarks>
        /// <exception cref="LogViewerException">Business exception</exception>
        protected virtual async Task ValidateDeleteAsync(TModel model)
        {
            if (model == null || model?.Id == 0)
            {
                throw new LogViewerException();
            }
            var record = await GetAsync(model.Id, false);
            if (record == null)
            {
                throw new LogViewerNotFoundException();
            }
        }

        protected async Task<TEntity> GetEntityOrThrowAsync(TModel model)
        {
            var data = await Repository.GetAsync(model.Id, false);

            if (data == null)
            {
                throw new LogViewerNotFoundException();
            }

            return data;
        }

        #endregion Validations
    }
}