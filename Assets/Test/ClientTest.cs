// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using System.Collections;
using OscJack;

class ClientTest : MonoBehaviour
{
    OscClient _client;

    IEnumerator Start()
    {
        // IP address, port number
        _client = new OscClient("127.0.0.1", 9000);

        // Send two-component float values ten times.
        for (var i = 0; i < 10; i++) {
            yield return new WaitForSeconds(0.5f);
            _client.Send("/test",       // OSC address
                         i * 10.0f,     // First element
                         Random.value); // Second element
        }
    }

    void OnDestroy()
    {
        _client?.Dispose();
        _client = null;
    }
}
