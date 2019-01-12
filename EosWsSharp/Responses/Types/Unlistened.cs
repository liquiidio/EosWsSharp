using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     See https://docs.dfuse.io/#websocket-based-api-unlisten
    /// </summary>
    public class Unlistened : IDfuseResponseData
    {
        [JsonProperty("success")] public bool Success { get; internal set; }
    }
}