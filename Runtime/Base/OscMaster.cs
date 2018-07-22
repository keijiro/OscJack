// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using System.Collections.Generic;
using System.Text;

namespace OscJack
{
    public static class OscMaster
    {
        #region Public methods

        public static OscServer GetSharedServer(int port)
        {
            OscServer server;
            if (!_servers.TryGetValue(port, out server))
            {
                server = new OscServer(port);
                _servers[port] = server;
            }
            return server;
        }

        public static OscClient GetSharedClient(string ipAddress, int port)
        {
            var key = GetClientKey(ipAddress, port);
            OscClient client;
            if (!_clients.TryGetValue(key, out client))
            {
                client = new OscClient(ipAddress, port);
                _clients[key] = client;
            }
            return client;
        }

        #endregion

        #region Mapping objects

        // OSC server map (key = port number)
        static Dictionary<int, OscServer> _servers = new Dictionary<int, OscServer>();

        // OSC client map (key = IP address + port number)
        static Dictionary<string, OscClient> _clients = new Dictionary<string, OscClient>();

        #endregion

        #region Client key generator

        static StringBuilder _stringBuilder = new StringBuilder();

        static string GetClientKey(string ipAddress, int port)
        {
            _stringBuilder.Length = 0;
            _stringBuilder.Append(ipAddress).Append(':').Append(port);
            return _stringBuilder.ToString();
        }

        #endregion
    }
}
