# EosWsSharp
C# dfuse.io Websocket-Client

*See https://docs.dfuse.io/#introduction*
  

### Usage

#### Configuration 

In order to connect to the dfsue-Services you need to create a new instance of the **DfuseClient** class passing the neccessary parameters to establish the connection

Example: 

```csharp
DfuseClient client = new DfuseClient(eosNetwork: Network.Mainnet, bearer: "your-bearer-token", origin: "your-origin");
```
* eosNetwork - enum-member of available EOSIO networks
* bearer - Bearer token containing credentials for authorization with the server
* origin - string passed to the Origin HTTP request header

#### Event-Subscriptions

In order to receive API-Responses you have to connect to the dfuse-servers and to consume the different events raised by the client when receiving responses.

##### Connect
```csharp
await client.ConnectAsync(CancellationToken.None);
```


##### Consume Events

* ###### HeadInfoReceived
	```csharp
	client.HeadInfoReceived += (sender, args) =>
	{
		HeadInfo headInfo = args.response.Data;
		Console.WriteLine(headInfo.HeadBlockNum);
	};
	```

* ###### ActionTracesReceived
	```csharp
	client.ActionTracesReceived += (sender, args) =>
	{
        ActionTrace actionTrace = args.response.Data;
        Console.WriteLine(actionTrace.Trace.InlineTraces.ToString());
	};
	```

* ###### TableDeltaReceived
	```csharp
	client.TableDeltaReceived += (sender, args) =>
	{
		TableDelta tableDelta = args.response.Data;
		Console.WriteLine(tableDelta.DbOp.AccountName);
	};
	```

* ###### TableRowsReceived
	```csharp
	client.TableDeltaReceived += (sender, args) =>
	{
		TableRows tableRows = args.response.Data;
		Console.WriteLine(tableRows.Account);
	};
	```

* ###### TransactionLifecycleReceived
	```csharp
	client.TransactionLifecycleReceived += (sender, args) =>
	{
		TransactionLifecycle transactionLifecycle = args.response.Data;
		Console.WriteLine(transactionLifecycle.TransactionStatus);
	};
	```

* ###### PingReceived
	```csharp
	client.PingReceived += (sender, args) =>
	{
		Ping ping = args.response.Data;
		Console.WriteLine(ping);
	};
	```

* ###### Listening
	```csharp
	client.Listening += (sender, args) =>
	{
        Listening listening = args.response.Data;
        Console.WriteLine(listening.NextBlock);
	};
	```

* ###### ErrorReceived
	```csharp
	client.ErrorReceived += (sender, args) =>
	{
		Error error = args.response.Data;
		Console.WriteLine(error.Message);
	};
	```


#### Api-Requests
In order to receive API-Responses you have send API-Requests via the given methods.

  * ###### GetHeadInfo
	```csharp
	await client.GetHeadInfo(new DfuseWebSocketRequest<GetHeadInfoOptions>()
	{
		Fetch = false,
		Listen = true,
		ReqId = "head_info_test",
		Data = new GetHeadInfoOptions(){	
	}
	});
	```

  * ###### GetActionTraces
	```csharp
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
	```

  * ###### GetTableRows
	```csharp
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
	```

  * ###### GetTransactionLifecycle
	```csharp
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
	```

  * ###### Unlisten
	```csharp
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
	```
