using UnityEngine;
using OscJack;

public class Tester : MonoBehaviour
{
    OscServer server;

    void Start()
    {
        server = new OscServer();
        server.Start();
    }

    void Update()
    {
        while (server.MessageCount > 0)
            Debug.Log(server.PopMessage());
    }
}
