using System;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using EosWsSharp.Requests;
using EosWsSharp.Requests.Options;
using EosWsSharp.Responses.Converters;
using EosWsSharp.Responses.Types;
using Newtonsoft.Json;
using EosWsSharp.Responses;

namespace EosWsSharp
{
    public class DfuseClient
    {
        /// <summary>
        /// The websocket-uri the clientWebSocket connects to.
        /// </summary>
        private readonly Uri wsUri;

        /// <summary>
        /// The bearer-token passed for authentication.
        /// </summary>
        private readonly string bearer;

        /// <summary>
        /// The origin passed to the header.
        /// </summary>
        private readonly string origin;

        /// <summary>
        /// The clientWebSocket.
        /// </summary>
        private ClientWebSocket clientWebSocket;

        /// <summary>
        /// Initializes a new instance of the <see cref="DfuseClient"/> class.
        /// </summary>
        /// <param name="eosNetwork">
        /// The eos network.
        /// </param>
        /// <param name="bearer">
        /// The bearer-token passed as authorization.
        /// </param>
        /// <param name="origin">
        /// The origin passed to the header.
        /// </param>
        public DfuseClient(Network eosNetwork, string bearer, string origin)
        {
            this.bearer = bearer;
            this.origin = origin;
            this.wsUri = new Uri($"wss://{eosNetwork.ToString().ToLowerInvariant()}.eos.dfuse.io/v1/stream");

            clientWebSocket = new ClientWebSocket();
            clientWebSocket.Options.SetRequestHeader(headerName: "Origin", headerValue: origin);
            clientWebSocket.Options.SetRequestHeader(headerName: "Authorization", headerValue: "Bearer " + bearer);
        }

        /// <summary>
        /// Connects to the Dfuse-Service.
        /// </summary>
        /// <param name="ctl">
        /// The CancellationToken ctl.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ConnectAsync(CancellationToken ctl)
        {
                await clientWebSocket.ConnectAsync(wsUri, ctl);
                Receive(ctl);
        }

        /// <summary>
        /// Waits and receives messages via Websocket.
        /// </summary>
        /// <param name="ctl">
        /// The CancellationToken ctl.
        /// </param>
        async void Receive(CancellationToken ctl)
        {
            while (clientWebSocket.State == WebSocketState.Open)
            {
                ArraySegment<byte> receivedBytes = new ArraySegment<byte>(new byte[32768]);

                WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(receivedBytes, ctl);
                while (!result.EndOfMessage)
                {
                    result = await clientWebSocket.ReceiveAsync(receivedBytes, ctl);
                }
                HandleResponse(Encoding.UTF8.GetString(receivedBytes.Array, 0, result.Count));
            }
        }

        /// <summary>
        /// Handles the dfuse-responses.
        /// </summary>
        /// <param name="jsonResponse">
        /// The json-response.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        private void HandleResponse(string jsonResponse)
        {
            dynamic dfuseResponseDynamic = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            if(dfuseResponseDynamic == null)
                return;
            
            switch (dfuseResponseDynamic.type.ToString())
            {
                case "action_trace":
                    DfuseWebSocketResponse<ActionTrace> actionTraceResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<ActionTrace>>(jsonResponse, new DfuseResponseConverter());
                    OnActionTracesReceived(new DfuseMessageReceivedEventArgs<ActionTrace>(){response = actionTraceResponse});
                    break;
                case "table_snapshot":
                    DfuseWebSocketResponse<TableRows> tableRowsResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<TableRows>>(jsonResponse, new DfuseResponseConverter());
                    OnTableRowsReceived(new DfuseMessageReceivedEventArgs<TableRows>(){response = tableRowsResponse});
                    break;
                case "table_delta":
                    DfuseWebSocketResponse<TableDelta> tableDeltaResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<TableDelta>>(jsonResponse, new DfuseResponseConverter());
                    OnTableDeltaReceived(new DfuseMessageReceivedEventArgs<TableDelta>(){response = tableDeltaResponse});
                    break;
                case "transaction_lifecycle":
                    DfuseWebSocketResponse<TransactionLifecycle> transactionLifecycleResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<TransactionLifecycle>>(jsonResponse, new DfuseResponseConverter());
                    OnTransactionLifecycleReceived(new DfuseMessageReceivedEventArgs<TransactionLifecycle>(){response = transactionLifecycleResponse});
                    break;
                case "head_info":
                    DfuseWebSocketResponse<HeadInfo> headInfoResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<HeadInfo>>(jsonResponse, new DfuseResponseConverter());
                    OnHeadInfoReceived(new DfuseMessageReceivedEventArgs<HeadInfo>() { response = headInfoResponse });
                    break;
                case "progress":
                    DfuseWebSocketResponse<Progress> progressResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<Progress>>(jsonResponse, new DfuseResponseConverter());
                    OnProgressReceived(new DfuseMessageReceivedEventArgs<Progress>() { response = progressResponse });
                    break;
                case "error":
                    DfuseWebSocketResponse<Error> errorResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<Error>>(jsonResponse, new DfuseResponseConverter());
                    OnErrorReceived(new DfuseMessageReceivedEventArgs<Error>() { response = errorResponse });
                    break;
                case "ping":
                    DfuseWebSocketResponse<Ping> pingResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<Ping>>(jsonResponse, new DfuseResponseConverter());
                    OnPingReceived(new DfuseMessageReceivedEventArgs<Ping>() { response = pingResponse });
                    break;
                case "listening":
                    DfuseWebSocketResponse<Listening> listeningResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<Listening>>(jsonResponse, new DfuseResponseConverter());
                    OnListening(new DfuseMessageReceivedEventArgs<Listening>() { response = listeningResponse });
                    break;
                case "unlistened":
                    DfuseWebSocketResponse<Unlistened> unlistenedResponse = JsonConvert.DeserializeObject<DfuseWebSocketResponse<Unlistened>>(jsonResponse, new DfuseResponseConverter());
                    OnUnlistened(new DfuseMessageReceivedEventArgs<Unlistened>() { response = unlistenedResponse });
                    break;
                default:
                    throw new Exception($"unknown type {dfuseResponseDynamic.type.ToString()}");
            }
        }

        /// <summary>
        /// Closes websocket-connection async.
        /// </summary>
        /// <param name="statusDescription">
        /// A status description for closing the connection.
        /// </param>
        /// <param name="ctl">
        /// The CancellationToken ctl.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task CloseAsync(string statusDescription, CancellationToken ctl)
        {
            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", ctl);
        }

        /// <summary>
        /// method for sending a get_action_trace-request.
        /// </summary>
        /// <param name="getActionTraceOpts">
        /// Options for sending a get_action_trace-request.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task GetActionTraces(DfuseWebSocketRequest<GetActionTracesOptions> getActionTraceOpts)
        {
            string getActionTraceOptsJson = JsonConvert.SerializeObject(getActionTraceOpts);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(getActionTraceOptsJson,0,getActionTraceOptsJson.Length)),WebSocketMessageType.Text,true,CancellationToken.None);
        }


        /// <summary>
        /// method for sending a get_table_row-request.
        /// </summary>
        /// <param name="getTableRowOpts">
        /// Options for sending a get_table_row-request.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task GetTableRows(DfuseWebSocketRequest<GetTableRowOptions> getTableRowOpts)
        {
            string getTableRowOptsJson = JsonConvert.SerializeObject(getTableRowOpts);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(getTableRowOptsJson, 0, getTableRowOptsJson.Length)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// method for sending a get_transaction_lifecycle-request.
        /// </summary>
        /// <param name="getTransactionLifecycleOpts">
        /// Options for sending a get_transaction_lifecycle-request.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task GetTransactionLifecycle(DfuseWebSocketRequest<GetTransactionLifecycleOptions> getTransactionLifecycleOpts)
        {
            string getTransactionLifecycleOptsJson = JsonConvert.SerializeObject(getTransactionLifecycleOpts);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(getTransactionLifecycleOptsJson, 0, getTransactionLifecycleOptsJson.Length)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// method for sending a get_head_info-request.
        /// </summary>
        /// <param name="getHeadInfoOpts">
        /// Options for sending a get_head_info-request.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task GetHeadInfo(DfuseWebSocketRequest<GetHeadInfoOptions> getHeadInfoOpts)
        {
            string getHeadInfoOptsJson = JsonConvert.SerializeObject(getHeadInfoOpts);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(getHeadInfoOptsJson, 0, getHeadInfoOptsJson.Length)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// method for sending a unlisten-request.
        /// </summary>
        /// <param name="unlistenOpts">
        /// Options for sending a unlisten-request.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task Unlisten(DfuseWebSocketRequest<UnlistenOptions> unlistenOpts)
        {
            string getTransactionLifecycleOptsJson = JsonConvert.SerializeObject(unlistenOpts);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(getTransactionLifecycleOptsJson, 0, getTransactionLifecycleOptsJson.Length)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// Invokes MessageReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<IDfuseResponseData>.
        /// </param>
        protected virtual void OnMessageReceived(DfuseMessageReceivedEventArgs<IDfuseResponseData> e)
        {
            MessageReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes ActionTracesReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<ActionTrace>.
        /// </param>
        protected virtual void OnActionTracesReceived(DfuseMessageReceivedEventArgs<ActionTrace> e)
        {
            ActionTracesReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes TableRowsReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<TableRows>.
        /// </param>
        protected virtual void OnTableRowsReceived(DfuseMessageReceivedEventArgs<TableRows> e)
        {
            TableRowsReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes TableDeltaReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<TableDelta>.
        /// </param>
        protected virtual void OnTableDeltaReceived(DfuseMessageReceivedEventArgs<TableDelta> e)
        {
            TableDeltaReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes TransactionLifecycleReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<TransactionLifecycle>.
        /// </param>
        protected virtual void OnTransactionLifecycleReceived(DfuseMessageReceivedEventArgs<TransactionLifecycle> e)
        {
            TransactionLifecycleReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes HeadInfoReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<HeadInfo>.
        /// </param>
        protected virtual void OnHeadInfoReceived(DfuseMessageReceivedEventArgs<HeadInfo> e)
        {
            HeadInfoReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes ProgressReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<Progress>.
        /// </param>
        protected virtual void OnProgressReceived(DfuseMessageReceivedEventArgs<Progress> e)
        {
            ProgressReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes PingReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<Ping>.
        /// </param>
        protected virtual void OnPingReceived(DfuseMessageReceivedEventArgs<Ping> e)
        {
            PingReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes ErrorReceived-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<Error>.
        /// </param>
        protected virtual void OnErrorReceived(DfuseMessageReceivedEventArgs<Error> e)
        {
            ErrorReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes Listening-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<Listening>.
        /// </param>
        protected virtual void OnListening(DfuseMessageReceivedEventArgs<Listening> e)
        {
            Listening?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes Unlistened-Event.
        /// </summary>
        /// <param name="e">
        /// DfuseMessageReceivedEventArgs<Unlistened>.
        /// </param>
        protected virtual void OnUnlistened(DfuseMessageReceivedEventArgs<Unlistened> e)
        {
            Unlistened?.Invoke(this, e);
        }

        /// <summary>
        /// MessageReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<IDfuseResponseData>> MessageReceived;

        /// <summary>
        /// ActionTracesReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<ActionTrace>> ActionTracesReceived;

        /// <summary>
        /// TableRowsReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<TableRows>> TableRowsReceived;

        /// <summary>
        /// TableDeltaReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<TableDelta>> TableDeltaReceived;

        /// <summary>
        /// TransactionLifecycleReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<TransactionLifecycle>> TransactionLifecycleReceived;

        /// <summary>
        /// HeadInfoReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<HeadInfo>> HeadInfoReceived;

        /// <summary>
        /// ProgressReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Progress>> ProgressReceived;

        /// <summary>
        /// ErrorReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Error>> ErrorReceived;

        /// <summary>
        /// PingReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Ping>> PingReceived;

        /// <summary>
        /// Listening-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Listening>> Listening;

        /// <summary>
        /// Unlistened-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Unlistened>> Unlistened;

    }

    /// <summary>
    /// dfuse messageReceivedEventArgs of generic type IDfuseResponseData.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class DfuseMessageReceivedEventArgs<T> where T : IDfuseResponseData
    {
        public DfuseWebSocketResponse<T> response;
    }

    /// <summary>
    /// Eos-network-enumerable.
    /// </summary>
    public enum Network
    {
        Mainnet,
        Kylin,
        Jungle
    }
}
