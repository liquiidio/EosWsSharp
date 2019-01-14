using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EosWsSharp.Requests;
using EosWsSharp.Requests.Options;
using EosWsSharp.Responses;
using EosWsSharp.Responses.Converters;
using EosWsSharp.Responses.Types;
using Newtonsoft.Json;

namespace EosWsSharp
{
    public class DfuseClient
    {
        /// <summary>
        ///     The bearer-token passed for authentication.
        /// </summary>
        private readonly string _bearer;

        /// <summary>
        ///     The origin passed to the header.
        /// </summary>
        private readonly string _origin;

        /// <summary>
        ///     The websocket-uri the clientWebSocket connects to.
        /// </summary>
        private readonly Uri _wsUri;

        /// <summary>
        ///     The clientWebSocket.
        /// </summary>
        private readonly ClientWebSocket _clientWebSocket;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DfuseClient" /> class.
        /// </summary>
        /// <param name="eosNetwork">
        ///     The eos network.
        /// </param>
        /// <param name="bearer">
        ///     The bearer-token passed as authorization.
        /// </param>
        /// <param name="origin">
        ///     The origin passed to the header.
        /// </param>
        public DfuseClient(Network eosNetwork, string bearer, string origin)
        {
            _bearer = bearer;
            _origin = origin;
            _wsUri = new Uri($"wss://{eosNetwork.ToString().ToLowerInvariant()}.eos.dfuse.io/v1/stream");

            _clientWebSocket = new ClientWebSocket();
            _clientWebSocket.Options.SetRequestHeader("Origin", origin);
            _clientWebSocket.Options.SetRequestHeader("Authorization", "Bearer " + bearer);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DfuseClient" /> class.
        /// </summary>
        /// <param name="eosNetwork">
        ///     The eos network.
        /// </param>
        /// <param name="bearer">
        ///     The bearer-token passed as authorization.
        /// </param>
        /// <param name="origin">
        ///     The origin passed to the header.
        /// </param>
        /// <param name="uri">
        ///     dfuse-Endpoint Uri
        /// </param>
        public DfuseClient(Network eosNetwork, string bearer, string origin, string uri)
        {
            _bearer = bearer;
            _origin = origin;
            _wsUri = new Uri(uri);

            _clientWebSocket = new ClientWebSocket();
            _clientWebSocket.Options.SetRequestHeader("Origin", origin);
            _clientWebSocket.Options.SetRequestHeader("Authorization", "Bearer " + bearer);
        }

        /// <summary>
        ///     The WebSocketState of the underlying ClientWebSocket.
        /// </summary>
        public WebSocketState ConnectionState => _clientWebSocket.State;

        /// <summary>
        ///     Connects to the Dfuse-Service.
        /// </summary>
        /// <param name="ctl">
        ///     The CancellationToken ctl.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task ConnectAsync(CancellationToken ctl)
        {
            await _clientWebSocket.ConnectAsync(_wsUri, ctl);
            Receive(ctl);
        }

        /// <summary>
        ///     Waits and receives messages via Websocket.
        /// </summary>
        /// <param name="ctl">
        ///     The CancellationToken ctl.
        /// </param>
        private async void Receive(CancellationToken ctl)
        {
            while (_clientWebSocket.State == WebSocketState.Open)
                try
                {
                    var receivedBytes = new ArraySegment<byte>(new byte[1024]);

                    var result = await _clientWebSocket.ReceiveAsync(receivedBytes, ctl);
                    var jsonResponse = Encoding.UTF8.GetString(receivedBytes.Array, 0, result.Count);
                    while (!result.EndOfMessage)
                    {
                        result = await _clientWebSocket.ReceiveAsync(receivedBytes, ctl);
                        jsonResponse += Encoding.UTF8.GetString(receivedBytes.Array, 0, result.Count);
                    }
                    HandleResponse(jsonResponse);
                }
                catch (Exception e)
                {
                    OnException(new DfuseExceptionEventArgs() { Exception = e });
                }
            OnConnectionLost(new DfuseWebSocketClosedEventArgs {State = _clientWebSocket.State});
        }

        /// <summary>
        ///     Handles the dfuse-responses.
        /// </summary>
        /// <param name="jsonResponse">
        ///     The json-response.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        private void HandleResponse(string jsonResponse)
        {
            try
            {
                var dfuseResponseDynamic = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (dfuseResponseDynamic == null)
                    return;

                switch (dfuseResponseDynamic.type.ToString())
                {
                    case "action_trace":
                        var actionTraceResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<ActionTrace>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnActionTracesReceived(new DfuseMessageReceivedEventArgs<ActionTrace>
                            {Response = actionTraceResponse});
                        break;
                    case "table_snapshot":
                        var tableRowsResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<TableRows>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnTableRowsReceived(new DfuseMessageReceivedEventArgs<TableRows>
                            {Response = tableRowsResponse});
                        break;
                    case "table_delta":
                        var tableDeltaResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<TableDelta>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnTableDeltaReceived(new DfuseMessageReceivedEventArgs<TableDelta>
                            {Response = tableDeltaResponse});
                        break;
                    case "transaction_lifecycle":
                        var transactionLifecycleResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<TransactionLifecycle>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnTransactionLifecycleReceived(new DfuseMessageReceivedEventArgs<TransactionLifecycle>
                            {Response = transactionLifecycleResponse});
                        break;
                    case "head_info":
                        var headInfoResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<HeadInfo>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnHeadInfoReceived(new DfuseMessageReceivedEventArgs<HeadInfo> {Response = headInfoResponse});
                        break;
                    case "progress":
                        var progressResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<Progress>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnProgressReceived(new DfuseMessageReceivedEventArgs<Progress> {Response = progressResponse});
                        break;
                    case "error":
                        var errorResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<Error>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnErrorReceived(new DfuseMessageReceivedEventArgs<Error> {Response = errorResponse});
                        break;
                    case "ping":
                        var pingResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<Ping>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnPingReceived(new DfuseMessageReceivedEventArgs<Ping> {Response = pingResponse});
                        break;
                    case "listening":
                        var listeningResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<Listening>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnListening(new DfuseMessageReceivedEventArgs<Listening> {Response = listeningResponse});
                        break;
                    case "unlistened":
                        var unlistenedResponse =
                            JsonConvert.DeserializeObject<DfuseWebSocketResponse<Unlistened>>(jsonResponse,
                                new DfuseResponseConverter());
                        OnUnlistened(new DfuseMessageReceivedEventArgs<Unlistened> {Response = unlistenedResponse});
                        break;
                    default:
                        throw new Exception($"unknown type {dfuseResponseDynamic.type.ToString()}");
                }
            }
            catch (Exception e)
            {
                OnException(new DfuseExceptionEventArgs(){Exception = e});
            }
        }

        /// <summary>
        ///     Closes websocket-connection async.
        /// </summary>
        /// <param name="statusDescription">
        ///     A status description for closing the connection.
        /// </param>
        /// <param name="ctl">
        ///     The CancellationToken ctl.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task CloseAsync(string statusDescription, CancellationToken ctl)
        {
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", ctl);
        }

        /// <summary>
        ///     method for sending a get_action_trace-request.
        /// </summary>
        /// <param name="getActionTraceOpts">
        ///     Options for sending a get_action_trace-request.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task GetActionTraces(DfuseWebSocketRequest<GetActionTracesOptions> getActionTraceOpts)
        {
            var getActionTraceOptsJson = JsonConvert.SerializeObject(getActionTraceOpts);
            await _clientWebSocket.SendAsync(
                new ArraySegment<byte>(
                    Encoding.ASCII.GetBytes(getActionTraceOptsJson, 0, getActionTraceOptsJson.Length)),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }


        /// <summary>
        ///     method for sending a get_table_row-request.
        /// </summary>
        /// <param name="getTableRowOpts">
        ///     Options for sending a get_table_row-request.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task GetTableRows(DfuseWebSocketRequest<GetTableRowOptions> getTableRowOpts)
        {
            var getTableRowOptsJson = JsonConvert.SerializeObject(getTableRowOpts);
            await _clientWebSocket.SendAsync(
                new ArraySegment<byte>(Encoding.ASCII.GetBytes(getTableRowOptsJson, 0, getTableRowOptsJson.Length)),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        ///     method for sending a get_transaction_lifecycle-request.
        /// </summary>
        /// <param name="getTransactionLifecycleOpts">
        ///     Options for sending a get_transaction_lifecycle-request.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task GetTransactionLifecycle(
            DfuseWebSocketRequest<GetTransactionLifecycleOptions> getTransactionLifecycleOpts)
        {
            var getTransactionLifecycleOptsJson = JsonConvert.SerializeObject(getTransactionLifecycleOpts);
            await _clientWebSocket.SendAsync(
                new ArraySegment<byte>(Encoding.ASCII.GetBytes(getTransactionLifecycleOptsJson, 0,
                    getTransactionLifecycleOptsJson.Length)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        ///     method for sending a get_head_info-request.
        /// </summary>
        /// <param name="getHeadInfoOpts">
        ///     Options for sending a get_head_info-request.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task GetHeadInfo(DfuseWebSocketRequest<GetHeadInfoOptions> getHeadInfoOpts)
        {
            var getHeadInfoOptsJson = JsonConvert.SerializeObject(getHeadInfoOpts);
            await _clientWebSocket.SendAsync(
                new ArraySegment<byte>(Encoding.ASCII.GetBytes(getHeadInfoOptsJson, 0, getHeadInfoOptsJson.Length)),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        ///     method for sending a unlisten-request.
        /// </summary>
        /// <param name="unlistenOpts">
        ///     Options for sending a unlisten-request.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task Unlisten(DfuseWebSocketRequest<UnlistenOptions> unlistenOpts)
        {
            var getTransactionLifecycleOptsJson = JsonConvert.SerializeObject(unlistenOpts);
            await _clientWebSocket.SendAsync(
                new ArraySegment<byte>(Encoding.ASCII.GetBytes(getTransactionLifecycleOptsJson, 0,
                    getTransactionLifecycleOptsJson.Length)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        ///     Invokes MessageReceived-Event.
        /// </summary>
        /// <param name="e" IDfuseResponseData=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnMessageReceived(DfuseMessageReceivedEventArgs<IDfuseResponseData> e)
        {
            MessageReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes ActionTracesReceived-Event.
        /// </summary>
        /// <param name="e" ActionTrace=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnActionTracesReceived(DfuseMessageReceivedEventArgs<ActionTrace> e)
        {
            ActionTracesReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes TableRowsReceived-Event.
        /// </summary>
        /// <param name="e" TableRows=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnTableRowsReceived(DfuseMessageReceivedEventArgs<TableRows> e)
        {
            TableRowsReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes TableDeltaReceived-Event.
        /// </summary>
        /// <param name="e" TableDelta=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnTableDeltaReceived(DfuseMessageReceivedEventArgs<TableDelta> e)
        {
            TableDeltaReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes TransactionLifecycleReceived-Event.
        /// </summary>
        /// <param name="e" TransactionLifecycle=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnTransactionLifecycleReceived(DfuseMessageReceivedEventArgs<TransactionLifecycle> e)
        {
            TransactionLifecycleReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes HeadInfoReceived-Event.
        /// </summary>
        /// <param name="e" HeadInfo=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnHeadInfoReceived(DfuseMessageReceivedEventArgs<HeadInfo> e)
        {
            HeadInfoReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes ProgressReceived-Event.
        /// </summary>
        /// <param name="e" Progress=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnProgressReceived(DfuseMessageReceivedEventArgs<Progress> e)
        {
            ProgressReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes PingReceived-Event.
        /// </summary>
        /// <param name="e" Ping=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnPingReceived(DfuseMessageReceivedEventArgs<Ping> e)
        {
            PingReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes ErrorReceived-Event.
        /// </summary>
        /// <param name="e" Error=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnErrorReceived(DfuseMessageReceivedEventArgs<Error> e)
        {
            ErrorReceived?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes Listening-Event.
        /// </summary>
        /// <param name="e" Listening=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnListening(DfuseMessageReceivedEventArgs<Listening> e)
        {
            Listening?.Invoke(this, e);
        }

        /// <summary>
        ///     Invokes Unlistened-Event.
        /// </summary>
        /// <param name="e" Unlistened=".">
        ///     DfuseMessageReceivedEventArgs
        /// </param>
        protected virtual void OnUnlistened(DfuseMessageReceivedEventArgs<Unlistened> e)
        {
            Unlistened?.Invoke(this, e);
        }

        /// <summary>
        ///     MessageReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<IDfuseResponseData>> MessageReceived;

        /// <summary>
        ///     ActionTracesReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<ActionTrace>> ActionTracesReceived;

        /// <summary>
        ///     TableRowsReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<TableRows>> TableRowsReceived;

        /// <summary>
        ///     TableDeltaReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<TableDelta>> TableDeltaReceived;

        /// <summary>
        ///     TransactionLifecycleReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<TransactionLifecycle>> TransactionLifecycleReceived;

        /// <summary>
        ///     HeadInfoReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<HeadInfo>> HeadInfoReceived;

        /// <summary>
        ///     ProgressReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Progress>> ProgressReceived;

        /// <summary>
        ///     ErrorReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Error>> ErrorReceived;

        /// <summary>
        ///     PingReceived-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Ping>> PingReceived;

        /// <summary>
        ///     Listening-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Listening>> Listening;

        /// <summary>
        ///     Unlistened-Event
        /// </summary>
        public event EventHandler<DfuseMessageReceivedEventArgs<Unlistened>> Unlistened;

        /// <summary>
        ///     Invokes ConnectionLost-Event.
        /// </summary>
        /// <param name="e">
        ///     DfuseWebSocketClosedEventArgs
        /// </param>
        protected virtual void OnConnectionLost(DfuseWebSocketClosedEventArgs e)
        {
            ConnectionLost?.Invoke(this, e);
        }


        /// <summary>
        ///     ConnectionLost-Event, when connection lost
        /// </summary>
        public event EventHandler<DfuseWebSocketClosedEventArgs> ConnectionLost;


        /// <summary>
        ///     Invokes Exeption-Event.
        /// </summary>
        /// <param name="e">
        ///     DfuseWebSocketClosedEventArgs
        /// </param>
        protected virtual void OnException(DfuseExceptionEventArgs e)
        {
            Exception?.Invoke(this, e);
        }

        /// <summary>
        ///     Exception-Event, when exception is thrown
        /// </summary>
        public event EventHandler<DfuseExceptionEventArgs> Exception;
    }

    /// <summary>
    ///     dfuse messageReceivedEventArgs of generic type IDfuseResponseData.
    /// </summary>
    public class DfuseWebSocketClosedEventArgs
    {
        public WebSocketState State;
    }

    /// <summary>
    ///     dfuse exceptionEventArgs of generic type IDfuseResponseData.
    /// </summary>
    public class DfuseExceptionEventArgs
    {
        public Exception Exception;
    }

    /// <summary>
    ///     dfuse messageReceivedEventArgs of generic type IDfuseResponseData.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class DfuseMessageReceivedEventArgs<T> where T : IDfuseResponseData
    {
        public DfuseWebSocketResponse<T> Response;
    }

    /// <summary>
    ///     Eos-network-enumerable.
    /// </summary>
    public enum Network
    {
        Mainnet,
        Kylin,
        Jungle
    }
}