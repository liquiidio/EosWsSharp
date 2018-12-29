using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    /// See https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-actiontrace
    /// </summary>
    public class ActionTrace : IDfuseResponseData
    {
        [JsonProperty("block_num")]
        public long BlockNum { get; internal set; }

        [JsonProperty("block_id")]
        public string BlockId { get; internal set; }

        [JsonProperty("block_time")]
        public DateTimeOffset BlockTime { get; internal set; }

        [JsonProperty("trx_id")]
        public string TrxId { get; internal set; }

        [JsonProperty("idx")]
        public long Idx { get; internal set; }

        [JsonProperty("depth")]
        public long Depth { get; internal set; }

        [JsonProperty("trace")]
        public Trace Trace { get; internal set; }

        [JsonProperty("ramops")]
        public RamOps[] RamOps { get; internal set; }

        [JsonProperty("dtrxops")]
        public DtrxOps[] DtrxOps { get; internal set; }
    }

    public class DtrxOps
    {
        [JsonProperty("op")]
        public string Op { get; internal set; }

        [JsonProperty("action_idx")]
        public long ActionIdx { get; internal set; }

        [JsonProperty("sender")]
        public string Sender { get; internal set; }

        [JsonProperty("sender_id")]
        public string SenderId { get; internal set; }

        [JsonProperty("payer")]
        public string Payer { get; internal set; }

        [JsonProperty("published_at")]
        public DateTimeOffset PublishedAt { get; internal set; }

        [JsonProperty("delay_until")]
        public DateTimeOffset DelayUntil { get; internal set; }

        [JsonProperty("expiration_at")]
        public DateTimeOffset ExpirationAt { get; set ; }

        [JsonProperty("trx_id")]
        public string TrxId { get; internal set; }

        [JsonProperty("trx")]
        public JObject Trx { get; internal set; }

        public dynamic DynamicTrxObject => Trx != null ? JsonConvert.DeserializeObject<dynamic>(Trx.ToString()) : null;
    }

    public class RamOps
    {
        [JsonProperty("op")]
        public string Op { get; internal set; }

        [JsonProperty("action_idx")]
        public long ActionIdx { get; internal set; }

        [JsonProperty("payer")]
        public string Payer { get; internal set; }

        [JsonProperty("delta")]
        public long Delta { get; internal set; }

        [JsonProperty("usage")]
        public long Usage { get; internal set; }
    }

    public class Trace
    {
        [JsonProperty("receipt")]
        public Receipt Receipt { get; internal set; }

        [JsonProperty("act")]
        public Act Act { get; internal set; }

        [JsonProperty("context_free")]
        public bool ContextFree { get; internal set; }

        [JsonProperty("elapsed")]
        public long Elapsed { get; internal set; }

        [JsonProperty("console")]
        public string Console { get; internal set; }

        [JsonProperty("trx_id")]
        public string TrxId { get; internal set; }

        [JsonProperty("block_num")]
        public long BlockNum { get; internal set; }

        [JsonProperty("block_time")]
        public DateTimeOffset BlockTime { get; internal set; }

        [JsonProperty("producer_block_id")]
        public string ProducerBlockId { get; internal set; }

        [JsonProperty("account_ram_deltas")]
        public AccountRamDelta[] AccountRamDeltas { get; internal set; }

        [JsonProperty("except")]
        public JObject Except { get; internal set; }

        public dynamic DynamicExceptObj => Except != null ? JsonConvert.DeserializeObject<dynamic>(Except.ToString()) : null;   // TODO no docs

        [JsonProperty("inline_traces")]
        public JObject[] InlineTraces { get; internal set; }

        public dynamic DynamicInlineTracesObj => InlineTraces != null ? JsonConvert.DeserializeObject<dynamic>(InlineTraces.ToString()) : null;   // TODO no docs

    }

    public class AccountRamDelta
    {
        [JsonProperty("account")]
        public string Account { get; internal set; }

        [JsonProperty("delta")]
        public long Delta { get; internal set; }
    }

    public class Act
    {
        [JsonProperty("account")]
        public string Account { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("authorization")]
        public Authorization[] Authorization { get; internal set; }

        [JsonProperty("data")]
        public ActData Data { get; internal set; }

        [JsonProperty("hex_data")]
        public string HexData { get; internal set; }
    }

    public class Authorization
    {
        [JsonProperty("actor")]
        public string Actor { get; internal set; }

        [JsonProperty("permission")]
        public string Permission { get; internal set; }
    }

    public class ActData
    {
        [JsonProperty("proposer")]
        public string Proposer { get; internal set; }

        [JsonProperty("proposal_name")]
        public string ProposalName { get; internal set; }

        [JsonProperty("executer")]
        public string Executer { get; internal set; }
    }

    public class Receipt
    {
        [JsonProperty("receiver")]
        public string Receiver { get; internal set; }
    }
}
