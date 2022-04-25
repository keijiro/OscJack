OSC Jack
========

![gif](https://i.imgur.com/mjp2o3t.gif)

**OSC Jack** is a lightweight implementation of [OSC] (Open Sound Control)
server/client written in C#, mainly aiming to provide OSC support to [Unity].

[OSC]: http://opensoundcontrol.org/
[Unity]: https://unity3d.com/

System Requirements
-------------------

- Unity 2021.3 or later

OSC Jack requires `System.Net.Sockets` that supports most platforms but a few
network-restrictive platforms like WebGL.

How To Install
--------------

This package uses the [scoped registry] feature to resolve package
dependencies. Open the Package Manager page in the Project Settings window and
add the following entry to the Scoped Registries list:

- Name: `Keijiro`
- URL: `https://registry.npmjs.com`
- Scope: `jp.keijiro`

![Scoped Registry](https://user-images.githubusercontent.com/343936/162576797-ae39ee00-cb40-4312-aacd-3247077e7fa1.png)

Now you can install the package from My Registries page in the Package Manager
window.

![My Registries](https://user-images.githubusercontent.com/343936/162576825-4a9a443d-62f9-48d3-8a82-a3e80b486f04.png)

[scoped registry]: https://docs.unity3d.com/Manual/upm-scoped.html

OSC Connection
--------------

![OSC Connection](https://user-images.githubusercontent.com/343936/165038054-33bebb1c-27b6-4fa3-9dd7-6f4091c7eb65.png)

You have to create an **OSC Connection** file to specify a connection type,
a host address and a port number. To create a new OSC Connection file, navigate
to Assets > Create > ScriptableObjects > OSC Jack > Connection.

At the moment, OSC Jack only supports the UDP connection type.

You have to specify a target host address to send OSC messages (you can leave
it empty for receive-only connections).

OSC Components
--------------

### OSC Event Receiver

![Event Receiver](https://user-images.githubusercontent.com/343936/165036750-63baad08-5b3c-4145-b9b9-e956d199d3dd.png)

**OSC Event Receiver** receives OSC messages and invokes a [UnityEvent] with
received data.

[UnityEvent]: https://docs.unity3d.com/Manual/UnityEvents.html

### OSC Property Sender

![Property Sender](https://user-images.githubusercontent.com/343936/165036537-2b80d2ed-a69a-4101-8678-86d244440369.png)

**OSC Property Sender** observes a component property and sends OSC messages
on changes to it.

OSC Monitor
-----------

![OSC Monitor](https://i.imgur.com/ZExVcuz.png)

**OSC Monitor** is a small utility inspecting incoming OSC messages. To open
the monitor, navigate to **Window > OSC Monitor**.

Low-Level API
-------------

### OscClient (implements IDisposable)

`OscClient` is a class for sending OSC messages. It supports `int`, `float` and
`string` types. It also supports sending up to four elements within a single
message.

```csharp
// IP address, port number
using (var client = new OscClient("127.0.0.1", 9000))
{
  // Send two-component float values ten times.
  for (var i = 0; i < 10; i++)
  {
    yield return new WaitForSeconds(0.5f);
    client.Send("/test",       // OSC address
                i * 10.0f,     // First element
                Random.value); // Second element
  }
}
```

### OscServer (implements IDisposable)

`OscServer` is a class for receiving OSC messages. You can add a delegate to
`MessageDispatcher` to receive messages sent to a specific OSC address (or give
an empty string to receive all messages).

Please note that the server invokes the delegates in the server thread. You
have to queue the events for processing them in the main thread.

It supports `int`, `float` and `string` types. It also supports receiving up to
four elements within a single message.

```csharp
using (var server = new OscServer(9000)) // Port number
{
  server.MessageDispatcher.AddCallback(
    "/test", // OSC address
    (string address, OscDataHandle data) => {
        Debug.Log(string.Format("({0}, {1})",
            data.GetElementAsFloat(0),
            data.GetElementAsFloat(1)));
    }
  );
  yield return new WaitForSeconds(10);
}
```
