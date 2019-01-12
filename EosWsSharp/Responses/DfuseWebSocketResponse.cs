using System.Diagnostics.CodeAnalysis;
using EosWsSharp.Responses.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EosWsSharp.Responses
{
    /// <summary>
    ///     Generic DfuseWebsocketResponse see https://docs.dfuse.io/#introduction for more
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class DfuseWebSocketResponse<T> where T : IDfuseResponseData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public ResponseType Type
        {
            get
            {
                if (Data is ActionTrace)
                    return ResponseType.action_trace;
                if (Data is HeadInfo)
                    return ResponseType.head_info;
                if (Data is TableRows)
                    return ResponseType.table_snapshot;
                if (Data is TableDelta)
                    return ResponseType.table_delta;
                if (Data is Listening)
                    return ResponseType.listening;
                if (Data is Ping)
                    return ResponseType.ping;
                if (Data is Unlistened)
                    return ResponseType.unlistened;
                if (Data is TransactionLifecycle)
                    return ResponseType.transaction_lifecycle;
                return ResponseType.unknown;
            }
        }

        [JsonProperty("req_id")] public string ReqId { get; internal set; }

        [JsonProperty("data")] public T Data { get; internal set; }
    }

    /// <summary>
    ///     ResponseType-enumeration.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ResponseType
    {
        action_trace,
        head_info,
        table_snapshot,
        table_delta,
        transaction_lifecycle,
        listening,
        unlistened,
        ping,
        unknown
    }

    public interface IDfuseResponseData
    {
    }
}