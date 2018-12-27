using Newtonsoft.Json;

namespace EosWsSharp.Requests.Options
{
    /// <summary>
    /// See https://docs.dfuse.io/#websocket-based-api-get_table_rows
    /// </summary>
    public class GetTableRowOptions : IDfuseRequestData
    {
        [JsonProperty("code", Required = Required.Always)]
        public string Code { get; set; }

        [JsonProperty("scope", Required = Required.Always)]
        public string Scope { get; set; }

        [JsonProperty("table", Required = Required.Always)]
        public string Table { get; set; }

        [JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Json { get; set; }
    }
}
