using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     See https://docs.dfuse.io/#websocket-based-api-error
    /// </summary>
    public class Error : IDfuseResponseData
    {
        [JsonProperty("code")] public string Code { get; internal set; }

        [JsonProperty("message")] public string Message { get; internal set; }

        [JsonProperty("details")] public Details Details { get; internal set; }
    }

    public class Details
    {
        [JsonProperty("tx_id")] public string TxId { get; internal set; }
    }
}