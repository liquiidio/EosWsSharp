using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     See https://docs.dfuse.io/#websocket-based-api-listening
    /// </summary>
    public class Listening : IDfuseResponseData
    {
        [JsonProperty("next_block")] public long NextBlock { get; internal set; }
    }
}