using UnityEngine;
using OscJack;

public class ReceiverTest : MonoBehaviour
{
    [SerializeField] int _port = 9000;
    [SerializeField] string _oscAddress = "/test";

    OscServer _server;

    Vector2 _position;
    float _scale;

    void Start()
    {
        _server = new OscServer(_port);
        _server.MessageDispatcher.AddCallback(_oscAddress, MessageCallback);
    }

    void OnDestroy()
    {
        _server.Dispose();
    }

    void Update()
    {
        transform.localPosition = new Vector3(_position.x, _position.y, 0);
        transform.localScale = Vector3.one * _scale;
    }

    void MessageCallback(string address, OscDataHandle data)
    {
        _scale = data.GetValueAsFloat(0);

        _position = new Vector2(
            data.GetValueAsFloat(1),
            data.GetValueAsFloat(2)
        );
    }
}
