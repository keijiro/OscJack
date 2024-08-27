// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using System;
using System.Net;
using System.Net.Sockets;

namespace OscJack
{
    public sealed class OscClient : IDisposable
    {
        #region Object life cycle

        public OscClient(string destination, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            if (destination == "255.255.255.255")
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            var dest = new IPEndPoint(IPAddress.Parse(destination), port);
            _socket.Connect(dest);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Packet sender methods

        public void Send(string address)
        {
            _encoder.Clear();
            _encoder.Append(address);
            _encoder.Append(",");
            _socket.Send(_encoder.Buffer, _encoder.Length, SocketFlags.None);
        }

        public void Send(string address, params int[] data)
        {
            _encoder.Clear();
            _encoder.Append(address);
            _encoder.Append(",iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii".Substring(0, data.Length + 1));
            foreach(int d in data)
                _encoder.Append(d);
            _socket.Send(_encoder.Buffer, _encoder.Length, SocketFlags.None);
        }

        public void Send(string address, params float[] data)
        {
            _encoder.Clear();
            _encoder.Append(address);
            _encoder.Append(",ffffffffffffffffffffffffffffffff".Substring(0, data.Length + 1));
            foreach(float d in data)
                _encoder.Append(d);
            _socket.Send(_encoder.Buffer, _encoder.Length, SocketFlags.None);
        }


        public void Send(string address, string data)
        {
            _encoder.Clear();
            _encoder.Append(address);
            _encoder.Append(",s");
            _encoder.Append(data);
            _socket.Send(_encoder.Buffer, _encoder.Length, SocketFlags.None);
        }

        #endregion

        #region IDispose implementation

        bool _disposed;

        void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }

                _encoder = null;
            }
        }

        ~OscClient()
        {
            Dispose(false);
        }

        #endregion

        #region Private variables

        OscPacketEncoder _encoder = new OscPacketEncoder();
        Socket _socket;

        #endregion
    }
}
