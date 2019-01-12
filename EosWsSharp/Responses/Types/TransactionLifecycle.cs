using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     See https://docs.dfuse.io/#types-strong-get-strong-code-v0-state-tables-scopes-code-transactionlifecycle
    /// </summary>
    public class TransactionLifecycle : IDfuseResponseData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("transaction_status")]
        public TransactionStatus TransactionStatus { get; internal set; }

        [JsonProperty("id")] public string Id { get; internal set; }

        [JsonProperty("transaction")] public dynamic Transaction { get; internal set; } // TODO no docs

        public dynamic DynamicTransactionObj =>
            Transaction != null ? JsonConvert.DeserializeObject<dynamic>(Transaction.ToString()) : null; // TODO no docs

        [JsonProperty("execution_trace")] public ActionTrace ExecutionTrace { get; internal set; }

        [JsonProperty("execution_block_header")]
        public string ExecutionBlockHeader { get; internal set; }

        [JsonProperty("dtrxops")] public DtrxOp[] DtrxOps { get; internal set; }

        [JsonProperty("ramops")] public RamOps[] RamOps { get; internal set; }

        [JsonProperty("pub_keys")] public string[] PubKeys { get; internal set; }

        [JsonProperty("created_by")] public ExtDTrxOp CreatedBy { get; internal set; }

        [JsonProperty("canceled_by")] public ExtDTrxOp CanceledBy { get; internal set; }

        [JsonProperty("execution_irreversible")]
        public bool ExecutionIrreversible { get; internal set; }

        [JsonProperty("creation_irreversible")]
        public bool CreationIrreversible { get; internal set; }

        [JsonProperty("cancelation_irreversible")]
        public bool CancelationIrreversible { get; internal set; }
    }

    public class DtrxOp
    {
        [JsonProperty("op")] public string Op { get; internal set; }

        [JsonProperty("trx_id")] public string TrxId { get; internal set; }

        [JsonProperty("trx")] public string Trx { get; internal set; }
    }

    public class ExtDTrxOp // TODO ExtDTrxop https://github.com/dfuse-io/eosws-go/blob/master/mdl/v1/dtrxop.go#L24
    {
        [JsonProperty("src_trx_id")] public string SourceTransactionId { get; internal set; }

        [JsonProperty("block_num")] public int BlockNum { get; internal set; }

        [JsonProperty("block_id")] public string BlockID { get; internal set; }

        [JsonProperty("block_time")] public DateTimeOffset BlockTime { get; internal set; }

        [JsonProperty("dtrxop")] public DtrxOp DtrxOp { get; internal set; }

        [JsonProperty("op")] public string Op { get; internal set; }

        [JsonProperty("action_idx")] public long ActionIdx { get; internal set; }

        [JsonProperty("sender")] public string Sender { get; internal set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum TransactionStatus
    {
        pending,
        delayed,
        canceled,
        expired,
        executed,
        soft_fail,
        hard_fail
    }
}