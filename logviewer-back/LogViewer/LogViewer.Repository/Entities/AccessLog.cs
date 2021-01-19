
using System;

namespace LogViewer.Repository.Entities
{
    public class AccessLog : BaseEntity
    {
        public string RemoteAddress { get; set; }
        public string RemoteUser { get; set; }
        public DateTime Time { get; set; }
        public string RequestUrl { get; set; }
        public int Status { get; set; }
        public int BytesSent { get; set; }
        public string HttpReferer { get; set; }
        public string UserAgent { get; set; }
        public float? GzipRatio { get; set; }
    }
}
