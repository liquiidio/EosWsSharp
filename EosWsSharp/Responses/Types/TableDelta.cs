using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     See https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-tabledelta
    /// </summary>
    public class TableDelta : IDfuseResponseData
    {
        [JsonProperty("block_num")] public long BlockNum { get; internal set; }

        [JsonProperty("step")] public string Step { get; internal set; }

        [JsonProperty("dbop")] public DbOp DbOp { get; internal set; }
    }
}