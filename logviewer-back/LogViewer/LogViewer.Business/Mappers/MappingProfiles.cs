using LogViewer.Models.ViewModels;
using LogViewer.Repository.Entities;

namespace LogViewer.Business.Mappers
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<AccessLog, AccessLogViewModel>().ReverseMap();
        }
    }
}
