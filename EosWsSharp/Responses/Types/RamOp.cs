using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    /// See https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-ramop
    /// </summary>
    public class RamOp
    {
        [JsonProperty("op")]
        public string Op { get; internal set; }

        [JsonProperty("action_idx")]
        public short ActionIdx { get; internal set; }

        [JsonProperty("payer")]
        public string Payer { get; internal set; }

        [JsonProperty("delta")]
        public long Delta { get; internal set; }

        [JsonProperty("usage")]
        public long Usage { get; internal set; }

    }
}
