using System;
using Newtonsoft.Json;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    /// See https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-transactionlifecycle
    /// </summary>
    public class TransactionLifecycle : IDfuseResponseData
    {
        [JsonProperty("transaction_status")]
        public string TransactionStatus { get; internal set; }

        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("transaction")]
        public string Transaction { get; internal set; }

        [JsonProperty("execution_trace")]
        public string ExecutionTrace { get; internal set; }

        [JsonProperty("execution_block_header")]
        public string ExecutionBlockHeader { get; internal set; }

        [JsonProperty("dtrxops")]
        public DtrxOp[] DtrxOps { get; internal set; }

        [JsonProperty("ramops")]
        public RamOps RamOps { get; internal set; }

        [JsonProperty("pub_keys")]
        public string[] PubKeys { get; internal set; }

        [JsonProperty("created_by")]
        public CreatedBy CreatedBy { get; internal set; }

        [JsonProperty("canceled_by")]
        public ExtDTrxOp CanceledBy { get; internal set; }

        [JsonProperty("execution_irreversible")]
        public bool ExecutionIrreversible { get; internal set; }

        [JsonProperty("creation_irreversible")]
        public bool CreationIrreversible { get; internal set; }

        [JsonProperty("cancelation_irreversible")]
        public bool CancelationIrreversible { get; internal set; }
    }

    public class CreatedBy
    {
        [JsonProperty("src_trx_id")]
        public string SrcTrxId { get; internal set; }

        [JsonProperty("block_num")]
        public long BlockNum { get; internal set; }

        [JsonProperty("block_id")]
        public string BlockId { get; internal set; }

        [JsonProperty("op")]
        public string Op { get; internal set; }

        [JsonProperty("action_idx")]
        public long ActionIdx { get; internal set; }

        [JsonProperty("sender")]
        public string Sender { get; internal set; }
    }

    public class DtrxOp
    {
        [JsonProperty("op")]
        public string Op { get; internal set; }

        [JsonProperty("trx_id")]
        public string TrxId { get; internal set; }

        [JsonProperty("trx")]
        public string Trx { get; internal set; }
    }

    public class ExtDTrxOp
    {
        [JsonProperty("src_trx_id")]
        public string SourceTransactionId { get; internal set; }

        [JsonProperty("block_num")]
        public int BlockNum { get; internal set; }

        [JsonProperty("block_id")]
        public string BlockID { get; internal set; }

        [JsonProperty("block_time")]
        public DateTimeOffset BlockTime { get; internal set; }

        [JsonProperty("dtrxop")]
        public DtrxOp DtrxOp { get; internal set; }
    }
}
