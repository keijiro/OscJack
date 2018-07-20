using UnityEngine;

namespace OscJack2
{
    public static class OscClientExtensions
    {
        public static void Send(this OscClient client, string address, Vector2 data)
        {
            client.Send(address, data.x, data.y);
        }

        public static void Send(this OscClient client, string address, Vector3 data)
        {
            client.Send(address, data.x, data.y, data.z);
        }

        public static void Send(this OscClient client, string address, Vector4 data)
        {
            client.Send(address, data.x, data.y, data.z, data.w);
        }

        public static void Send(this OscClient client, string address, Vector2Int data)
        {
            client.Send(address, data.x, data.y);
        }

        public static void Send(this OscClient client, string address, Vector3Int data)
        {
            client.Send(address, data.x, data.y, data.z);
        }
    }
}
