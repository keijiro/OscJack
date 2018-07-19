using UnityEngine;
using OscJack2;

public class Test : MonoBehaviour
{
    [SerializeField] string _address = "/test";
    [SerializeField] int _port = 9000;

    OscServer _server;

    void Start()
    {
        _server = new OscServer(_port);
        _server.messageDispatcher.AddCallback(_address, MessageCallback);
        _server.Start();
    }

    void OnDestroy()
    {
        _server.Dispose();
    }

    void MessageCallback(OscDataHandle data)
    {
        Debug.Log(data.GetValueAsString(0));
    }
}
