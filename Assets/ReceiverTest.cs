using UnityEngine;
using OscJack2;

public class ReceiverTest : MonoBehaviour
{
    [SerializeField] int _port = 9000;
    [SerializeField] string _oscAddress = "/test";

    OscServer _server;
    Vector3 _data;

    void Start()
    {
        _server = new OscServer(_port);
        _server.MessageDispatcher.AddCallback(_oscAddress, MessageCallback);
        _server.Start();
    }

    void OnDestroy()
    {
        _server.Dispose();
    }

    void Update()
    {
        transform.localPosition = new Vector3(_data.y, _data.z, 0);
        transform.localScale = Vector3.one * _data.x;
    }

    void MessageCallback(string address, OscDataHandle data)
    {
        _data = data.GetValuesAsVector3();
    }
}
