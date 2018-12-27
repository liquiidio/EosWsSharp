using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    /// See https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-tablerows
    /// </summary>
    public class TableRows : IDfuseResponseData
    {
        [JsonProperty("account")]
        public string Account { get; internal set; }

        [JsonProperty("scope")]
        public string Scope { get; internal set; }

        [JsonProperty("rows")]
        public DbRow[] Rows { get; internal set; }
    }
}