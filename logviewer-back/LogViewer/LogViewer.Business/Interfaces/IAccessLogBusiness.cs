using LogViewer.Models.SearchModels;
using LogViewer.Models.ViewModels;
using LogViewer.Repository.Entities;
using System.Threading.Tasks;

namespace LogViewer.Business.Interfaces
{
    public interface IAccessLogBusiness : IBaseBusiness<AccessLog, AccessLogViewModel, AccessLogSearchModel>
    {
        /// <summary>
        /// Convert Apache/Nginx access log entries from format:
        /// $remote_addr - $remote_user [$time_local] "$request" $status $body_bytes_sent "$http_referer" "$http_user_agent"
        /// to <see cref="AccessLogViewModel"/> object
        /// </summary>
        /// <param name="fileName">Log file name</param>
        /// <param name="logContent">Apache/Nginx access log</param>
        /// <returns>Instance of <see cref="ResultViewModel"/> object</returns>
        Task<ResultViewModel> ParseLogEntryAsync(string fileName, string logContent);
    }
}
