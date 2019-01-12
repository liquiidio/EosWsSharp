using System;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    ///     See https://docs.dfuse.io/#websocket-based-api-ping
    /// </summary>
    public class Ping : IDfuseResponseData
    {
        public DateTimeOffset PingData;

        public Ping(DateTimeOffset pingData)
        {
            PingData = pingData;
        }

        public static implicit operator Ping(string pingData)
        {
            return new Ping(DateTimeOffset.Parse(pingData));
        }

        public static implicit operator Ping(DateTime pingDateTime)
        {
            return new Ping(pingDateTime);
        }
    }
}