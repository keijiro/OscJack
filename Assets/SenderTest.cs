using UnityEngine;
using OscJack2;

class SenderTest : MonoBehaviour
{
    [SerializeField] string _ipAddress = "127.0.0.1";
    [SerializeField] int _port = 9000;
    [SerializeField] string _oscAddress = "/test";

    OscClient _client;

    void Start()
    {
        _client = new OscClient(_ipAddress, _port);
    }

    void Update()
    {
        var t = Time.time;
        var x = Mathf.Sin(t * 1.1f);
        var y = Mathf.Sin(t * 1.9f);
        var s = Mathf.Sin(t * 2.3f) * 0.4f + 1;
        _client.Send(_oscAddress, new Vector3(s, x, y));
    }
}
