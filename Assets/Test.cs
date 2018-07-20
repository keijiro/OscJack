using UnityEngine;
using OscJack2;

public class Test : MonoBehaviour
{
    [SerializeField] string _address = "/test";
    [SerializeField] int _port = 9000;

    OscServer _server;
    float _value = 1;

    void Start()
    {
        _server = new OscServer(_port);
        _server.MessageDispatcher.AddCallback(_address, MessageCallback);
        _server.Start();
    }

    void OnDestroy()
    {
        _server.Dispose();
    }

    void Update()
    {
        transform.localScale = Vector3.one * _value;
    }

    void MessageCallback(string address, OscDataHandle data)
    {
        _value = data.GetValueAsFloat(0);
    }
}
