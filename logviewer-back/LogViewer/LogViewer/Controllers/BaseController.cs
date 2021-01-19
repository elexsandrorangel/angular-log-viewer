using LogViewer.Business.Interfaces;
using LogViewer.Models.SearchModels;
using LogViewer.Models.ViewModels;
using LogViewer.Repository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogViewer.Controllers
{
    public abstract class BaseController<TBusiness, TEntity, TModel, TSearch>
       : ControllerBase
       where TEntity : BaseEntity
       where TModel : BaseViewModel
       where TSearch : BaseSearchModel
       where TBusiness : IBaseBusiness<TEntity, TModel, TSearch>
    {
        /// <summary>
        /// Application business implementation
        /// </summary>
        protected readonly TBusiness Business;

        protected BaseController(TBusiness business)
        {
            Business = business;
        }

        [NonAction]
        protected int? GetCurrentUserId()
        {
            var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return null;
            }

            return Convert.ToInt32(userIdClaim);
        }

        /// <summary>
        /// Retrieves a list of records
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="page">Page number</param>
        /// <param name="qty">Itens per page</param>
        /// <response code="200">Request successfull</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int?}")]
        [Route("page/{page:int?}/items/{qty:int}")]
        [Route("{id:int?}/page/{page:int}/items/{qty:int}")]
        [ProducesResponseType(typeof(IEnumerable<BaseViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Get(int? id = null, int page = 0, int qty = int.MaxValue)
        {
            if (id.HasValue)
            {
                TModel model = await Business.GetAsync(id.Value, false);

                if (model == null)
                {
                    return NotFound();
                }

                return Ok(model);
            }

            IEnumerable<TModel> list = await Business.GetAsync(page, qty);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        /// <summary>
        /// Search records
        /// </summary>
        /// <param name="search">Criteria</param>
        /// <response code="200">Request successfull</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(typeof(IEnumerable<BaseViewModel>), 200)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Search([FromBody] TSearch search)
        {
            return Ok(await Business.SearchAsync(search));
        }

        /// <summary>
        /// Create a new record
        /// </summary>
        /// <param name="model">Model to persist</param>
        /// <response code="201">Record created successfully</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(BaseViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> CreateAsync([FromBody] TModel model)
        {
            //AuthenticationVO authenticatedUser = GetCurrentUser();

            //if (authenticatedUser == null)
            //{
            //    return Unauthorized();
            //}

            //UsersVO user = await _usersBusiness.GetByEmailAsync(authenticatedUser.userPrincipalName);

            //model.CreatedById = user.Id;
            //model.ModifiedById = user.Id;

            TModel data = await Business.AddAsync(model);

            string url = $"{Request.Scheme}://{Request.Host}{Request.Path}/{data.Id}";
            return Created(url, data);
        }

        /// <summary>
        /// Update the specified record
        /// </summary>
        /// <param name="id">Record identifier</param>
        /// <param name="model">Record data</param>
        /// <response code="200">Request successfull</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="404">Record not found</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(BaseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Edit(int id, [FromBody] TModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            //AuthenticationVO authenticatedUser = GetCurrentUser();

            //if (authenticatedUser == null)
            //{
            //    return Unauthorized();
            //}

            //UsersVO user = await _usersBusiness.GetByEmailAsync(authenticatedUser.userPrincipalName);

            //model.ModifiedById = user.Id;

            TModel data = await Business.UpdateAsync(model);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        /// <summary>
        /// Update/Patch the specified record
        /// </summary>
        /// <param name="id">Record identifier</param>
        /// <param name="model">Record data</param>
        /// <response code="200">Request successfull</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="404">Record not found</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpPatch]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(BaseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Patch(int id, [FromBody] TModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            //AuthenticationVO authenticatedUser = GetCurrentUser();

            //if (authenticatedUser == null)
            //{
            //    return Unauthorized();
            //}

            //UsersVO user = await _usersBusiness.GetByEmailAsync(authenticatedUser.userPrincipalName);

            //if (model.Id == int.Empty)
            //{
            //    model.Id = id;
            //}

            //model.ModifiedById = user.Id;

            TModel data = await Business.UpdateAsync(model, id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        /// <summary>
        /// Remove the record from database
        /// </summary>
        /// <param name="id">Record identifier</param>
        /// <response code="204">Request successfull</response>
        /// <response code="400">Request has missing/invalid values</response>
        /// <response code="404">Record not found</response>
        /// <response code="500">Oops! An error occurred</response>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await Business.DeleteAsync(id);
            return NoContent();
        }
    }
}