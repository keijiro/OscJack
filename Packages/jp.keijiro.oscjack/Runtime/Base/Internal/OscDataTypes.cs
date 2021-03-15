// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using System;
using System.Text;

namespace OscJack
{
    internal static class OscDataTypes
    {
        static Byte[] _temp4 = new Byte[4]; // only used to reverse byte order

        public static bool IsSupportedTag(char tag)
        {
            return tag == 'i' || tag == 'f' || tag == 's' || tag == 'b';
        }

        public static int Align4(int length)
        {
            return (length + 3) & ~3;
        }

        public static int ReadInt(Byte[] buffer, int offset)
        {
            return (buffer[offset + 0] << 24) +
                   (buffer[offset + 1] << 16) +
                   (buffer[offset + 2] <<  8) +
                   (buffer[offset + 3]);
        }

        public static float ReadFloat(Byte[] buffer, int offset)
        {
            _temp4[0] = buffer[offset + 3];
            _temp4[1] = buffer[offset + 2];
            _temp4[2] = buffer[offset + 1];
            _temp4[3] = buffer[offset    ];
            return BitConverter.ToSingle(_temp4, 0);
        }

        public static int GetStringSize(Byte[] buffer, int offset)
        {
            var length = 0;
            while (buffer[offset + length] != 0) length++;
            return Align4(offset + length + 1) - offset;
        }

        public static string ReadString(Byte[] buffer, int offset)
        {
            var length = 0;
            while (buffer[offset + length] != 0) length++;
            return Encoding.UTF8.GetString(buffer, offset, length);
        }
    }
}
