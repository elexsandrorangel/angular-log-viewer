using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace LogViewer.Models.ViewModels
{
    public class AccessLogViewModel : BaseViewModel
    {
        [JsonProperty("remoteAddr")]
        [JsonPropertyName("remoteAddr")] // Component used for Swagger docs
        public string RemoteAddress { get; set; }

        [JsonProperty("remoteUser")]
        [JsonPropertyName("remoteUser")] // Component used for Swagger docs
        public string RemoteUser { get; set; }

        [JsonProperty("timeLocal")]
        [JsonPropertyName("timeLocal")]
        public DateTime Time { get; set; }
        
        [JsonProperty("requestUrl")]
        [JsonPropertyName("requestUrl")]
        public string RequestUrl { get; set; }
        
        [JsonProperty("statusCode")]
        [JsonPropertyName("statusCode")]
        public int Status { get; set; }
        
        [JsonProperty("bytesSent")]
        [JsonPropertyName("bytesSent")]
        public int BytesSent { get; set; }
        
        [JsonProperty("httpRefer")]
        [JsonPropertyName("httpRefer")]
        public string HttpReferer { get; set; }
        
        [JsonProperty("httpUserAgent")]
        [JsonPropertyName("httpUserAgent")]
        public string UserAgent { get; set; }
        
        [JsonProperty("gzipRatio")]
        [JsonPropertyName("gzipRatio")]
        public float? GzipRatio { get; set; }
    }
}
