using Newtonsoft.Json;
using System;

namespace LogViewer.Models.ViewModels
{
    public class AccessLogViewModel : BaseViewModel
    {
        [JsonProperty("remoteAddr")]
        public string RemoteAddress { get; set; }

        [JsonProperty("remoteUser")]
        public string RemoteUser { get; set; }

        [JsonProperty("timeLocal")]
        public DateTime Time { get; set; }
        
        [JsonProperty("requestUrl")]
        public string RequestUrl { get; set; }
        
        [JsonProperty("statusCode")]
        public int Status { get; set; }
        
        [JsonProperty("bytesSent")]
        public int BytesSent { get; set; }
        
        [JsonProperty("httpRefer")]
        public string HttpReferer { get; set; }
        
        [JsonProperty("httpUserAgent")]
        public string UserAgent { get; set; }
        
        [JsonProperty("gzipRatio")]
        public float? GzipRatio { get; set; }
    }
}
