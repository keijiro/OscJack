using System;
using System.Collections.Generic;

namespace OscJack
{
    public struct OscMessage
    {
        public string path;
        public object[] data;

        public OscMessage(string path, object[] data)
        {
            this.path = path;
            this.data = data;
        }

        public override string ToString ()
        {
            var temp = path + ":";
            foreach (var o in data)
                temp += o + ":";
            return temp;
        }
    }

    public class OscParser
    {
        #region Public Methods And Properties

        public int MessageCount {
            get { return _messageQueue.Count; }
        }

        public OscParser()
        {
            _messageQueue = new Queue<OscMessage>();
        }

        public OscMessage PopMessage()
        {
            return _messageQueue.Dequeue();
        }

        public void FeedData(Byte[] data)
        {
            _readBuffer = data;
            _readPoint = 0;

            ReadMessage();

            _readBuffer = null;
        }

        #endregion

        #region Private Implementation

        Queue<OscMessage> _messageQueue;
        Byte[] _readBuffer;
        int _readPoint;

        void ReadMessage()
        {
            var path = ReadString();

            if (path == "#bundle")
            {
                ReadInt64();

                while (true)
                {
                    if (_readPoint >= _readBuffer.Length) return;

                    var peek = _readBuffer[_readPoint];
                    if (peek == '/' || peek == '#') {
                        ReadMessage();
                        return;
                    }

                    var bundleEnd = _readPoint + ReadInt32();
                    while (_readPoint < bundleEnd)
                        ReadMessage();
                }
            }

            var types = ReadString();
            var temp = new OscMessage(path, new object[types.Length - 1]);

            for (var i = 0; i < types.Length - 1; i++)
            {
                switch (types[i + 1])
                {
                case 'f':
                    temp.data[i] = ReadFloat32();
                    break;
                case 'i':
                    temp.data[i] = ReadInt32();
                    break;
                case 's':
                    temp.data[i] = ReadString();
                    break;
                case 'b':
                    temp.data[i] = ReadBlob();
                    break;
                }
            }

            _messageQueue.Enqueue(temp);
        }

        float ReadFloat32()
        {
            Byte[] temp = {
                _readBuffer[_readPoint + 3],
                _readBuffer[_readPoint + 2],
                _readBuffer[_readPoint + 1],
                _readBuffer[_readPoint]
            };
            _readPoint += 4;
            return BitConverter.ToSingle(temp, 0);
        }

        int ReadInt32 ()
        {
            int temp =
                (_readBuffer[_readPoint + 0] << 24) +
                (_readBuffer[_readPoint + 1] << 16) +
                (_readBuffer[_readPoint + 2] << 8) +
                (_readBuffer[_readPoint + 3]);
            _readPoint += 4;
            return temp;
        }

        long ReadInt64 ()
        {
            long temp =
                ((long)_readBuffer[_readPoint + 0] << 56) +
                ((long)_readBuffer[_readPoint + 1] << 48) +
                ((long)_readBuffer[_readPoint + 2] << 40) +
                ((long)_readBuffer[_readPoint + 3] << 32) +
                ((long)_readBuffer[_readPoint + 4] << 24) +
                ((long)_readBuffer[_readPoint + 5] << 16) +
                ((long)_readBuffer[_readPoint + 6] << 8) +
                ((long)_readBuffer[_readPoint + 7]);
            _readPoint += 8;
            return temp;
        }

        string ReadString()
        {
            var offset = 0;
            while (_readBuffer[_readPoint + offset] != 0) offset++;
            var s = System.Text.Encoding.UTF8.GetString(_readBuffer, _readPoint, offset);
            _readPoint += (offset + 4) & ~3;
            return s;
        }

        Byte[] ReadBlob()
        {
            var length = ReadInt32();
            var temp = new Byte[length];
            Array.Copy(_readBuffer, _readPoint, temp, 0, length);
            _readPoint += (length + 3) & ~3;
            return temp;
        }

        #endregion
    }
}
