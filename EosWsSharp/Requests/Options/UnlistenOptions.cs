using Newtonsoft.Json;

namespace EosWsSharp.Requests.Options
{
    /// <summary>
    /// See https://docs.dfuse.io/#websocket-based-api-unlisten
    /// </summary>
    public class UnlistenOptions : IDfuseRequestData
    {
        [JsonProperty("req_id", Required = Required.Always)]
        public string ReqId { get; set; }
    }
}
