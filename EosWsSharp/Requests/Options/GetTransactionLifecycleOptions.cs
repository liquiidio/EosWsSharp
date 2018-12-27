using Newtonsoft.Json;

namespace EosWsSharp.Requests.Options
{
    /// <summary>
    /// See https://docs.dfuse.io/#websocket-based-api-get_transaction_lifecycle
    /// </summary>
    public class GetTransactionLifecycleOptions : IDfuseRequestData
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
    }
}
