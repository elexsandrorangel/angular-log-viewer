using System;

namespace LogViewer.Models.SearchModels
{
    public class AccessLogSearchModel : BaseSearchModel
    {
        public string RemoteAddress { get; set; }
        public string RemoteUser { get; set; }
        public string RequestUrl { get; set; }
        public int Status { get; set; }
        public int BytesSent { get; set; }
        public string HttpReferer { get; set; }
        public string UserAgent { get; set; }
        public float? GzipRatio { get; set; }
    }
}
