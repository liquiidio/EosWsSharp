using Newtonsoft.Json;

namespace EosWsSharp.Requests.Options
{
    /// <summary>
    /// See https://docs.dfuse.io/#websocket-based-api-get_action_traces
    /// </summary>
    public class GetActionTracesOptions : IDfuseRequestData
    {
        [JsonProperty("accounts", Required = Required.Always)]
        public string Accounts { get; set; }

        [JsonProperty("receivers", NullValueHandling = NullValueHandling.Ignore)]
        public string Receivers { get; set; }

        [JsonProperty("action_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ActionName { get; set; }

        [JsonProperty("with_ramops", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WithRamOps { get; set; }

        [JsonProperty("with_inline_traces", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WithInlineTraces { get; set; }

        [JsonProperty("with_dtrxops", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WithDTrxOps { get; set; }
    }
}
