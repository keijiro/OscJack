// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using System;

namespace OscJack
{
    internal sealed class OscPacketEncoder
    {
        public Byte[] Buffer { get { return _buffer; } }
        public int Length { get { return _length; } }

        public void Clear()
        {
            _length = 0;
        }

        public void Append(string data)
        {
            var len = data.Length;
            for (var i = 0; i < len; i++)
                _buffer[_length++] = (Byte)data[i];

            var len4 = OscDataTypes.Align4(len + 1);
            for (var i = len; i < len4; i++)
                _buffer[_length++] = 0;
        }

        public void Append(int data)
        {
            _buffer[_length++] = (Byte)(data >> 24);
            _buffer[_length++] = (Byte)(data >> 16);
            _buffer[_length++] = (Byte)(data >>  8);
            _buffer[_length++] = (Byte)(data);
        }

        public void Append(float data)
        {
            _tempFloat[0] = data;
            System.Buffer.BlockCopy(_tempFloat, 0, _tempByte, 0, 4);
            _buffer[_length++] = _tempByte[3];
            _buffer[_length++] = _tempByte[2];
            _buffer[_length++] = _tempByte[1];
            _buffer[_length++] = _tempByte[0];
        }

        Byte[] _buffer = new Byte[4096];
        int _length;

        // Used to change byte order
        static float[] _tempFloat = new float[1];
        static Byte[] _tempByte = new Byte[4];
    }
}
