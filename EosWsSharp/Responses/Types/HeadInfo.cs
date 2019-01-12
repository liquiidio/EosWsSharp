using System;
using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     See https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-headinfo
    /// </summary>
    public class HeadInfo : IDfuseResponseData
    {
        [JsonProperty("last_irreversible_block_num")]
        public long LastIrreversibleBlockNum { get; internal set; }

        [JsonProperty("last_irreversible_block_id")]
        public string LastIrreversibleBlockId { get; internal set; }

        [JsonProperty("head_block_num")] public long HeadBlockNum { get; internal set; }

        [JsonProperty("head_block_id")] public string HeadBlockId { get; internal set; }

        [JsonProperty("head_block_time")] public DateTimeOffset HeadBlockTime { get; internal set; }

        [JsonProperty("head_block_producer")] public string HeadBlockProducer { get; internal set; }
    }
}