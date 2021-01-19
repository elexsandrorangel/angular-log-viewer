using System;

namespace LogViewer.Models.SearchModels
{
    public abstract class BaseSearchModel
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Itens per page
        /// </summary>
        public int Quantity { get; set; } = int.MaxValue;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Define if record is active
        /// </summary>

        public bool? Active { get; set; }

        /// <summary>
        /// Created Date from
        /// </summary>
        public DateTime? CreatedDateStart { get; set; }

        /// <summary>
        /// Created Date to
        /// </summary>
        public DateTime? CreatedDateEnd { get; set; }

        /// <summary>
        /// Last change date from
        /// </summary>
        public DateTime? ModifiedDateStart { get; set; }

        /// <summary>
        /// Last change date to
        /// </summary>
        public DateTime? ModifiedDateEnd { get; set; }
    }
}
