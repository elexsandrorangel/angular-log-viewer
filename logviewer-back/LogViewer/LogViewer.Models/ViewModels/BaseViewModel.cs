using System;

namespace LogViewer.Models.ViewModels
{
    public abstract class BaseViewModel
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

    }
}
