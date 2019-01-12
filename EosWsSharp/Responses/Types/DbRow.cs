using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-dbrow
    /// </summary>
    public class DbRow
    {
        [JsonProperty("payer")] public string Payer { get; internal set; }

        [JsonProperty("hex")] public string Hex { get; internal set; }

        [JsonProperty("key")] public string Key { get; internal set; }

        [JsonProperty("json")] public dynamic Json { get; internal set; }

        [JsonProperty("error")] public string Error { get; internal set; }
    }
}