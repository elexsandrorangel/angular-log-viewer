using AutoMapper;
using LogViewer.Business.Interfaces;
using LogViewer.Infrastructure.Exceptions;
using LogViewer.Models.SearchModels;
using LogViewer.Models.ViewModels;
using LogViewer.Repository.Entities;
using LogViewer.Repository.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogViewer.Business.Implementations
{
    public class AccessLogBusiness
        : BaseBusiness<AccessLog, AccessLogViewModel, AccessLogSearchModel, IAccessLogRepository>, IAccessLogBusiness
    {
        public AccessLogBusiness(IAccessLogRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<ResultViewModel> ParseLogEntryAsync(string fileName, string logContent)
        {
            // Apache/Nginx log format: $remote_addr - $remote_user [$time_local] "$request" $status $body_bytes_sent "$http_referer" "$http_user_agent"
            string pattern = @"(?<remote_addr>.*?)\s-\s(?<remote_user>.*?)\s\[(?<time_local>.*?)\]\s""(?<request>.*?)""\s(?<status>\d{3})\s(?<body_bytes_sent>.*?)\s""(?<http_referer>.*?)""\s""(?<http_user_agent>.*?)""";

            var matches = Regex.Matches(logContent, pattern, RegexOptions.IgnoreCase);

            if (!matches.Any())
            {
                return new ResultViewModel
                {
                    Message = $"The log file '{fileName}' hasn't any matches",
                    Success = false
                };
            }

            // Fill the properties with named group of regular expression
            var data =  matches.Select(m => new AccessLogViewModel
            {
                RemoteAddress = m.Groups["remote_addr"].Value,
                RemoteUser = m.Groups["remote_user"].Value,
                Time = DateTime.ParseExact(m.Groups["time_local"].Value, "dd/MMM/yyyy:HH:mm:ss K", CultureInfo.InvariantCulture),
                RequestUrl = m.Groups["request"].Value,
                Status = Convert.ToInt32(m.Groups["status"].Value),
                BytesSent = m.Groups["body_bytes_sent"].Value == "-" ? 0 : Convert.ToInt32(m.Groups["body_bytes_sent"].Value),
                HttpReferer = m.Groups["http_referer"].Value,
                UserAgent = m.Groups["http_user_agent"].Value
            }).ToList();

            if (data.Any())
            {
                await AddAsync(data);
            }
            return new ResultViewModel
            {
                Message = $"The log file '{fileName}' was imported successfully",
                Success = true
            };
        }

        protected override void ValidateInsert(AccessLogViewModel model)
        {

            if (string.IsNullOrEmpty(model.RemoteAddress))
            {
                throw new LogViewerException("The remote IP address is required");
            }

            if (string.IsNullOrEmpty(model.RequestUrl))
            {
                throw new LogViewerException("The Request URL is required");
            }
            if (model.Status < 100 || model.Status > 599)
            {
                throw new LogViewerException("The status code is invalid");
            }
        }
    }
}
