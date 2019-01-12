using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-dbop
    /// </summary>
    public class DbOp
    {
        [JsonProperty("op")] public string Op { get; internal set; }

        [JsonProperty("action_idx")] public short ActionIdx { get; internal set; }

        [JsonProperty("account")] public string AccountName { get; internal set; }

        [JsonProperty("scope")] public string Scope { get; internal set; }

        [JsonProperty("key")] public string Key { get; internal set; }

        [JsonProperty("old")] public DbRow Old { get; internal set; }

        [JsonProperty("new")] public DbRow New { get; internal set; }
    }
}