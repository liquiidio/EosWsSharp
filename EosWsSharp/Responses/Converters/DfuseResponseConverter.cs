using System;
using EosWsSharp.Responses.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace EosWsSharp.Responses.Converters
{
    /// <summary>
    /// Custom DfuseResponseConverter for converting response-json to Generic DfuseWebSocketResponse
    /// </summary>
    class DfuseResponseConverter : CustomCreationConverter<DfuseWebSocketResponse<IDfuseResponseData>>
    {
        public DfuseWebSocketResponse<IDfuseResponseData> Create(Type objectType, JObject jObject)
        {
            var type = (string)jObject.Property("type");

            switch (type)
            {
                case "action_trace":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new ActionTrace()
                    };
                case "table_rows":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new TableRows()
                    };
                case "table_delta":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new TableDelta()
                    };
                case "transaction_lifecycle":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new TransactionLifecycle()
                    };
                case "get_head_info":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new HeadInfo()
                    };
                case "progress":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new Progress()
                    };
                case "error":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new Error()
                    };
                case "listening":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new Listening()
                    };
                case "unlisten":
                    return new DfuseWebSocketResponse<IDfuseResponseData>()
                    {
                        Data = new Unlistened()
                    };
            }

            throw new ApplicationException($"The type {type} is not supported!");
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override DfuseWebSocketResponse<IDfuseResponseData> Create(System.Type objectType)
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}
