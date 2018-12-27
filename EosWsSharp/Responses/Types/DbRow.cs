using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    /// https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-dbrow
    /// </summary>
    public class DbRow
    {
        [JsonProperty("payer")]
        public string Payer { get; internal set; }

        [JsonProperty("hex")]
        public string Hex { get; internal set; }

        [JsonProperty("key")]
        public string Key { get; internal set; }

        [JsonProperty("json")]
        public JObject Json { get; internal set; }

        public dynamic DynamicRowObj => Json != null ? JsonConvert.DeserializeObject<dynamic>(Json.ToString()) : null;

        [JsonProperty("error")]
        public string Error { get; internal set; }

    }
}
