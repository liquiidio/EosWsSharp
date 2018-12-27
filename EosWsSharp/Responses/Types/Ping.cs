using System;

namespace EosWsSharp.Responses.Types
{
    /// <summary>
    /// See https://docs.dfuse.io/#websocket-based-api-ping
    /// </summary>
    public class Ping : IDfuseResponseData
    {
        private DateTimeOffset _pingData;

        public Ping(DateTimeOffset pingData)
        {
            _pingData = pingData;
        }

        public static implicit operator Ping(string pingData)
        {
            return new Ping(DateTimeOffset.Parse(pingData));
        }
    }
}