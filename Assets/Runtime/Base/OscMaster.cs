using System.Collections.Generic;
using System.Text;

namespace OscJack2
{
    public static class OscMaster
    {
        static Dictionary<int, OscServer> _servers = new Dictionary<int, OscServer>();
        static Dictionary<string, OscClient> _clients = new Dictionary<string, OscClient>();
        static StringBuilder _stringBuilder = new StringBuilder();

        static string GetKey(string ipAddress, int port)
        {
            _stringBuilder.Length = 0;
            _stringBuilder.Append(ipAddress).Append(port);
            return _stringBuilder.ToString();
        }

        public static OscServer GetServer(int port)
        {
            OscServer server;
            if (!_servers.TryGetValue(port, out server))
            {
                server = new OscServer(port);
                _servers[port] = server;
            }
            return server;
        }

        public static OscClient GetClient(string ipAddress, int port)
        {
            var key = GetKey(ipAddress, port);
            OscClient client;
            if (!_clients.TryGetValue(key, out client))
            {
                client = new OscClient(ipAddress, port);
                _clients[key] = client;
            }
            return client;
        }
    }
}
