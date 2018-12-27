using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using EosWsSharp.Requests.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EosWsSharp.Requests
{
    /// <summary>
    /// Generic DfuseWebsocketRequest see https://docs.dfuse.io/#introduction for more
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class DfuseWebSocketRequest<T> where T : IDfuseRequestData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type", Required = Required.Always)]
        public RequestType Type
        {
            get
            {
                if (Data is GetActionTracesOptions)
                    return RequestType.get_action_traces;
                if (Data is GetHeadInfoOptions)
                    return RequestType.get_head_info;
                if (Data is GetTableRowOptions)
                    return RequestType.get_table_rows;
                if (Data is GetTransactionLifecycleOptions)
                    return RequestType.get_transaction;
                if (Data is UnlistenOptions)
                    return RequestType.unlisten;
                return RequestType.unknown;
            }
        }

        [JsonProperty("req_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ReqId { get; set; }

        [DefaultValue(false)]
        [JsonProperty("fetch", DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Fetch { get; set; }

        [DefaultValue(false)]
        [JsonProperty("listen", DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Listen { get; set; }

        [JsonProperty("start_block", NullValueHandling = NullValueHandling.Ignore)]
        public long? StartBlock { get; set; }

        [JsonProperty("with_progress", NullValueHandling = NullValueHandling.Ignore)]
        public long? WithProgress { get; set; }

        [JsonProperty("data", Required = Required.Always)]
        public T Data { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum RequestType
    {
        get_action_traces,
        get_head_info,
        get_transaction,
        get_table_rows,
        unlisten,
        unknown
    }

    public interface IDfuseRequestData
    {    }

}