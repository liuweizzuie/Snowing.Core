using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.Json
{
    [JsonObject]
    public class JsonResult
    {
        [JsonProperty(PropertyName = "status", Order = 0)]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "message", Order = 1)]
        public string Message { get; set; }

        public bool IsSuccess()
        {
            return this.Status == 0;
        }

        public JsonResult()
        {
            this.Status = 0;
            this.Message = "OK";
        }
    }

    [JsonObject]
    public class JsonResult<T> : JsonResult
    {
        [JsonProperty(PropertyName = "data", Order = 2)]
        public T Data { get; set; }

        public JsonResult()
        {
            this.Data = default(T);
            this.Message = "OK";
        }
    }
}
