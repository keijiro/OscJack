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
        Queue<OscMessage> messageQueue_;
        Byte[] readBuffer_;
        int readPoint_;

        #region Public members

        public int MessageCount {
            get { return messageQueue_.Count; }
        }

        public OscParser()
        {
            messageQueue_ = new Queue<OscMessage>();
        }

        public OscMessage PopMessage()
        {
            return messageQueue_.Dequeue();
        }

        public void FeedData(Byte[] data)
        {
            readBuffer_ = data;
            readPoint_ = 0;

            ReadMessage();

            readBuffer_ = null;
        }

        #endregion

        #region Private methods

        void ReadMessage()
        {
            var path = ReadString();

            if (path == "#bundle")
            {
                ReadInt64();

                while (true)
                {
                    if (readPoint_ >= readBuffer_.Length) return;

                    var peek = readBuffer_[readPoint_];
                    if (peek == '/' || peek == '#') {
                        ReadMessage();
                        return;
                    }

                    var bundleEnd = readPoint_ + ReadInt32();
                    while (readPoint_ < bundleEnd)
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

            messageQueue_.Enqueue(temp);
        }

        float ReadFloat32()
        {
            Byte[] temp = {
                readBuffer_[readPoint_ + 3],
                readBuffer_[readPoint_ + 2],
                readBuffer_[readPoint_ + 1],
                readBuffer_[readPoint_]
            };
            readPoint_ += 4;
            return BitConverter.ToSingle(temp, 0);
        }

        int ReadInt32 ()
        {
            int temp =
                (readBuffer_[readPoint_ + 0] << 24) +
                (readBuffer_[readPoint_ + 1] << 16) +
                (readBuffer_[readPoint_ + 2] << 8) +
                (readBuffer_[readPoint_ + 3]);
            readPoint_ += 4;
            return temp;
        }

        long ReadInt64 ()
        {
            long temp =
                ((long)readBuffer_[readPoint_ + 0] << 56) +
                ((long)readBuffer_[readPoint_ + 1] << 48) +
                ((long)readBuffer_[readPoint_ + 2] << 40) +
                ((long)readBuffer_[readPoint_ + 3] << 32) +
                ((long)readBuffer_[readPoint_ + 4] << 24) +
                ((long)readBuffer_[readPoint_ + 5] << 16) +
                ((long)readBuffer_[readPoint_ + 6] << 8) +
                ((long)readBuffer_[readPoint_ + 7]);
            readPoint_ += 8;
            return temp;
        }

        string ReadString()
        {
            var offset = 0;
            while (readBuffer_[readPoint_ + offset] != 0) offset++;
            var s = System.Text.Encoding.UTF8.GetString(readBuffer_, readPoint_, offset);
            readPoint_ += (offset + 4) & ~3;
            return s;
        }

        Byte[] ReadBlob()
        {
            var length = ReadInt32();
            var temp = new Byte[length];
            Array.Copy(readBuffer_, readPoint_, temp, 0, length);
            readPoint_ += (length + 3) & ~3;
            return temp;
        }

        #endregion
    }
}
