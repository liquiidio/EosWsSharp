using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     See https://docs.dfuse.io/#websocket-based-api-progress
    /// </summary>
    public class Progress : IDfuseResponseData
    {
        [JsonProperty("block_num")] public long BlockNum { get; internal set; }

        [JsonProperty("block_id")] public string BlockId { get; internal set; }
    }
}