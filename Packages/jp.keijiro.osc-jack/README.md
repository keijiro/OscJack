OSC Jack
========

![gif](https://i.imgur.com/mjp2o3t.gif)

**OSC Jack** is a lightweight implementation of [OSC] (Open Sound Control)
server and client that is written in C#. It mainly aims to provide basic OSC
support to [Unity].

[OSC]: http://opensoundcontrol.org/
[Unity]: https://unity3d.com/

System Requirements
-------------------

- Unity 2017.3 or later

OSC Jack uses and requires a `System.Net.Sockets` implementation. It means that
it runs on most platforms but doesn't support few special platforms like WebGL
or network-restrictive consoles.

Installation
------------

This package uses the [scoped registry] feature to resolve package
dependencies. Please add the following sections to the manifest file
(Packages/manifest.json).

[scoped registry]: https://docs.unity3d.com/Manual/upm-scoped.html

To the `scopedRegistries` section:

```
{
  "name": "Keijiro",
  "url": "https://registry.npmjs.com",
  "scopes": [ "jp.keijiro" ]
}
```

To the `dependencies` section:

```
"jp.keijiro.osc-jack": "1.0.1"
```

After changes, the manifest file should look like below:

```
{
  "scopedRegistries": [
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [ "jp.keijiro" ]
    }
  ],
  "dependencies": {
    "jp.keijiro.osc-jack": "1.0.1",
    ...
```

OSC Components
--------------

### OSC Event Receiver

![OSC Event Receiver](https://i.imgur.com/tWUe42Y.png)

**OSC Event Receiver** receives OSC messages and invokes a [UnityEvent] with
received data. This can be a handy way to modify property values or invoke
methods based on OSC messages.

[UnityEvent]: https://docs.unity3d.com/Manual/UnityEvents.html

### OSC Property Sender

![OSC Property Sender](https://i.imgur.com/dkx26EE.png)

**OSC Property Sender** provides a handy way to send OSC messages based on a
component property value. It observes a given component property, and sends OSC
messages when changes in the property are detected. 

OSC Monitor
-----------

![OSC Monitor](https://i.imgur.com/ZExVcuz.png)

**OSC Monitor** is a small utility to show incoming messages to existing OSC
servers. It's useful to check if messages are correctly received at the
servers. To open the monitor, navigate to **Window > OSC Monitor**.

Scripting Interface
-------------------

OSC Jack also provides non-Unity dependent classes that can be used from any
C# script. These classes are useful when sending/receiving OSC messages that
are not directly related to component properties or events.

### OSC Client class

`OscClient` provides basic functionalities to send OSC messages to a specific
UDP port of a host. It supports `int`, `float` and `string` types, and it's
capable of sending up to four elements within a single message. It implements
`IDisposable`, so it can be manually terminated by calling the `Dispose` method
(or left it until automatically being finalized).

```csharp
// IP address, port number
var client = new OscClient("127.0.0.1", 9000);

// Send two-component float values ten times.
for (var i = 0; i < 10; i++) {
    yield return new WaitForSeconds(0.5f);
    client.Send("/test",       // OSC address
                i * 10.0f,     // First element
                Random.value); // Second element
}

// Terminate the client.
client.Dispose();
```

### OSC Server class

`OscServer` provides basic functionalities to receive OSC messages that are
sent to a specific UDP port of the host. It starts receiving messages when a
server instance is created, and terminates when disposed (via the `IDisposable`
interface).

You can add delegates to `MessageDispatcher` to receive messages sent to a
specific OSC address, or you can give an empty string as an address to receive
all messages arrived at the port.

Note that the delegates are to be called in the server thread; You have to
queue the events for processing them in the main thread (this will be required
in most cases of Unity).

Just like the client class, it supports `int`, `float` and `string` types, and
capable of receiving up to four elements within a single message.

```csharp
var server = new OscServer(9000); // Port number

server.MessageDispatcher.AddCallback(
    "/test", // OSC address
    (string address, OscDataHandle data) => {
        Debug.Log(string.Format("({0}, {1})",
            data.GetElementAsFloat(0),
            data.GetElementAsFloat(1)));
    }
);

yield return new WaitForSeconds(10);
server.Dispose();
```
