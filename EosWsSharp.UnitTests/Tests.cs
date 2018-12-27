using System;
using System.Threading;
using System.Threading.Tasks;
using EosWsSharp.Requests;
using EosWsSharp.Requests.Options;
using EosWsSharp.Responses;
using EosWsSharp.Responses.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace EosWsSharp.UnitTests
{
    [TestClass]
    public class Tests
    {
        private string bearer = "";
        private string origin = "";


        [TestMethod]
        public async Task GetErrorTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotError = false;

            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetActionTraces(new DfuseWebSocketRequest<GetActionTracesOptions>()
            {
                Fetch = false,
                Listen = false,
                ReqId = "error_test",
                Data = new GetActionTracesOptions()
                {
                    Accounts = "eosio.tokennnnnnnnn",
                    ActionName = "transferrrrrrrrrrrr",
                    WithRamOps = true,
                    WithDTrxOps = true,
                    WithInlineTraces = true,
                }
            });

            await Task.Delay(10000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotError);
        }

        [TestMethod]
        public async Task GetActionTracesTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotListening = false;
            bool gotActionTrace = false;
            bool gotError = false;

            client.Listening += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Listening>));
                gotListening = true;
            };
            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.ActionTracesReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<ActionTrace>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                gotActionTrace = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetActionTraces(new DfuseWebSocketRequest<GetActionTracesOptions>()
            {
                Fetch = false,
                Listen = true,
                ReqId = "action_traces_test",
                Data = new GetActionTracesOptions()
                {
                    Accounts = "eosio.token",
                    ActionName = "transfer",
                    WithRamOps = true,
                    WithDTrxOps = true,
                    WithInlineTraces = true,
                }
            });

            await Task.Delay(30000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            Assert.IsTrue(gotListening);
            Assert.IsTrue(gotActionTrace);
            Assert.IsFalse(gotError);
        }

        [TestMethod]
        public async Task GetPingTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotError = false;

            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetHeadInfo(new DfuseWebSocketRequest<GetHeadInfoOptions>()
            {
                Fetch = false,
                Listen = true,
                ReqId = "head_info_test",
                Data = new GetHeadInfoOptions()
                {
                    // empty
                }
            });

            await Task.Delay(30000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            Assert.IsFalse(gotError);
        }

        [TestMethod]
        public async Task GetHeadInfoTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotListening = false;
            bool gotHeadInfo = false;
            bool gotError = false;

            client.Listening += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Listening>));
                gotListening = true;
            };
            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.HeadInfoReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<HeadInfo>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                gotHeadInfo = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetHeadInfo(new DfuseWebSocketRequest<GetHeadInfoOptions>()
            {
                Fetch = false,
                Listen = true,
                ReqId = "head_info_test",
                Data = new GetHeadInfoOptions()
                {
                    // empty
                }
            });

            await Task.Delay(30000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            //Assert.IsTrue(gotListening);  // TODO no listening response?
            Assert.IsTrue(gotHeadInfo);
            Assert.IsFalse(gotError);
        }

        [TestMethod]
        public async Task GetTableDeltaAsHexTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotListening = false;
            bool gotTableDelta = false;
            bool gotError = false;

            client.Listening += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Listening>));
                gotListening = true;
            };
            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.TableDeltaReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<TableDelta>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                Assert.IsNotNull(args.response.Data.DbOp.New.Hex);
                Assert.IsNotNull(args.response.Data.DbOp.Old.Hex);
                gotTableDelta = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetTableRows(new DfuseWebSocketRequest<GetTableRowOptions>()
            {
                Fetch = false,
                Listen = true,
                ReqId = "table_rows_test",
                StartBlock = 34155920,                
                Data = new GetTableRowOptions()
                {
                    Code = "eosio.token",
                    Json = false,
                    Scope = "finalfrontor",
                    Table = "accounts"
                }
            });

            await Task.Delay(10000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            Assert.IsTrue(gotListening);
            Assert.IsTrue(gotTableDelta);
            Assert.IsFalse(gotError);
        }

        [TestMethod]
        public async Task GetTableDeltaAsJsonTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotListening = false;
            bool gotTableDelta = false;
            bool gotError = false;

            client.Listening += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Listening>));
                gotListening = true;
            };
            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.TableDeltaReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<TableDelta>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                Assert.IsNotNull(args.response.Data.DbOp.New.Json.First);
                Assert.IsNotNull(args.response.Data.DbOp.Old.Json.First);
                gotTableDelta = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetTableRows(new DfuseWebSocketRequest<GetTableRowOptions>()
            {
                Fetch = false,
                Listen = true,
                ReqId = "table_rows_test",
                StartBlock = 34155920,
                Data = new GetTableRowOptions()
                {
                    Code = "eosio.token",
                    Json = true,
                    Scope = "finalfrontor",
                    Table = "accounts"
                }
            });

            await Task.Delay(10000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            Assert.IsTrue(gotListening);
            Assert.IsTrue(gotTableDelta);
            Assert.IsFalse(gotError);
        }

        [TestMethod]
        public async Task GetTableSnapshotAsJsonTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotTableRows = false;
            bool gotError = false;

            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.TableRowsReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<TableRows>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                gotTableRows = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetTableRows(new DfuseWebSocketRequest<GetTableRowOptions>()
            {
                Fetch = true,
                Listen = false,
                ReqId = "table_snapshot_test",
                Data = new GetTableRowOptions()
                {
                    Code = "eosio.token",
                    Json = true,
                    Scope = "eosio.token",
                    Table = "accounts",
                    // TODO Offset, Limit ?
                }
            });

            await Task.Delay(30000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            Assert.IsTrue(gotTableRows);
            Assert.IsFalse(gotError);
        }

        [TestMethod]
        public async Task GetTableSnapshotAsHexTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotTableRows = false;
            bool gotError = false;

            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.TableRowsReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<TableRows>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                gotTableRows = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetTableRows(new DfuseWebSocketRequest<GetTableRowOptions>()
            {
                Fetch = true,
                Listen = false,
                ReqId = "table_snapshot_test",
                Data = new GetTableRowOptions()
                {
                    Code = "eosio.token",
                    Json = false,
                    Scope = "eosio.token",
                    Table = "accounts",
                    // TODO Offset, Limit ?
                }
            });

            await Task.Delay(30000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            Assert.IsTrue(gotTableRows);
            Assert.IsFalse(gotError);
        }

        [TestMethod]
        public async Task GetTransactionLifecycleTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotListening = false;
            bool gotTransactionLifecycle = false;
            bool gotError = false;

            client.Listening += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Listening>));
                gotListening = true;
            };
            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.TransactionLifecycleReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<TransactionLifecycle>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                gotTransactionLifecycle = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };
            await client.ConnectAsync(CancellationToken.None);

            await client.GetTransactionLifecycle(new DfuseWebSocketRequest<GetTransactionLifecycleOptions>()
            {
                Fetch = false,
                Listen = true,
                ReqId = "transaction_test",
                Data = new GetTransactionLifecycleOptions()
                {
                    Id = "8a1fc59c932467b2ab38bcd0c651ec6222993482292cc2ffef89a9025e7b841b"
                }
            });

            await Task.Delay(30000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            Assert.IsTrue(gotListening);
            Assert.IsTrue(gotTransactionLifecycle);
            Assert.IsFalse(gotError);
        }

        [TestMethod]
        public async Task UnlistenTest()
        {
            DfuseClient client = new DfuseClient(Network.Mainnet, bearer, origin);

            bool gotPing = false;
            bool gotListening = false;
            bool gotHeadInfo = false;
            bool gotUnlistened = false;
            bool gotError = false;

            client.Listening += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Listening>));
                gotListening = true;
            };
            client.PingReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Ping>));
                gotPing = true;
            };
            client.HeadInfoReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<HeadInfo>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                gotHeadInfo = true;
            };
            client.Unlistened += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Unlistened>));
                Assert.IsNotNull(args.response.Type);
                Assert.IsNotNull(args.response.Data);
                gotUnlistened = true;
            };
            client.ErrorReceived += (sender, args) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(args.response));
                Assert.IsInstanceOfType(args.response, typeof(DfuseWebSocketResponse<Error>));
                gotError = true;
            };

            await client.ConnectAsync(CancellationToken.None);

            await client.GetHeadInfo(new DfuseWebSocketRequest<GetHeadInfoOptions>()
            {
                Fetch = false,
                Listen = true,
                ReqId = "unlisten_test",
                Data = new GetHeadInfoOptions()
                {
                    // empty
                }
            });

            await Task.Delay(60000);

            await client.Unlisten(new DfuseWebSocketRequest<UnlistenOptions>()
            {
                Fetch = false,
                Listen = true,
                ReqId = "transaction_test",
                Data = new UnlistenOptions()
                {
                    ReqId = "unlisten_test"
                }
            });

            await Task.Delay(30000);
            await client.CloseAsync("test", CancellationToken.None);
            Assert.IsTrue(gotPing);
            //Assert.IsTrue(gotListening);  // TODO no listening ?
            Assert.IsTrue(gotHeadInfo);
            Assert.IsTrue(gotUnlistened);
            gotHeadInfo = false;
            await Task.Delay(5000);
            Assert.IsFalse(gotHeadInfo);
            Assert.IsFalse(gotError);
        }
    }
}
