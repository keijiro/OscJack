// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using OscJack;

class ServerTest : MonoBehaviour
{
    OscServer _server;

    void Start()
    {
        _server = new OscServer(9000); // Port number

        _server.MessageDispatcher.AddCallback(
            "/test", // OSC address
            (string address, OscDataHandle data) => {
                Debug.Log(string.Format("({0}, {1})",
                    data.GetElementAsFloat(0),
                    data.GetElementAsFloat(1)));
            }
        );
    }

    void OnDestroy()
    {
        _server?.Dispose();
        _server = null;
    }
}
