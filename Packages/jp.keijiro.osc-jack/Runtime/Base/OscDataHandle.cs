// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using System;
using System.Collections.Generic;

namespace OscJack
{
    //
    // Data handle class that provides offset values to each data element
    // within a shared data buffer
    //
    public sealed class OscDataHandle
    {
        #region Public methods

        public int GetElementCount()
        {
            return _typeTags.Count;
        }

        public int GetElementAsInt(int index)
        {
            if (index >= _typeTags.Count) return 0;
            var tag = _typeTags[index];
            var offs = _offsets[index];
            if (tag == 'i') return OscDataTypes.ReadInt(_sharedBuffer, offs);
            if (tag == 'f') return (int)OscDataTypes.ReadFloat(_sharedBuffer, offs);
            return 0;
        }

        public float GetElementAsFloat(int index)
        {
            if (index >= _typeTags.Count) return 0;
            var tag = _typeTags[index];
            var offs = _offsets[index];
            if (tag == 'f') return OscDataTypes.ReadFloat(_sharedBuffer, offs);
            if (tag == 'i') return OscDataTypes.ReadInt(_sharedBuffer, offs);
            return 0;
        }

        public string GetElementAsString(int index)
        {
            if (index >= _typeTags.Count) return "";
            var tag = _typeTags[index];
            var offs = _offsets[index];
            if (tag == 's') return OscDataTypes.ReadString(_sharedBuffer, offs);
            if (tag == 'i') return OscDataTypes.ReadInt(_sharedBuffer, offs).ToString();
            if (tag == 'f') return OscDataTypes.ReadFloat(_sharedBuffer, offs).ToString();
            return "";
        }

        #endregion

        #region Internal method

        internal void Scan(Byte[] buffer, int offset)
        {
            // Reset the internal state.
            _sharedBuffer = buffer;
            _typeTags.Clear();
            _offsets.Clear();

            // Read type tags.
            offset++; // ","

            while (true)
            {
                var tag = (char)buffer[offset];
                if (!OscDataTypes.IsSupportedTag(tag)) break;
                _typeTags.Add(tag);
                offset++;
            }

            offset += OscDataTypes.GetStringSize(buffer, offset);

            // Determine the offsets of the each element.
            foreach (var tag in _typeTags)
            {
                _offsets.Add(offset);

                if (tag == 'i' || tag == 'f')
                {
                    offset += 4;
                }
                else if (tag == 's')
                {
                    offset += OscDataTypes.GetStringSize(buffer, offset);
                }
                else // tag == 'b'
                {
                    offset += 4 + OscDataTypes.ReadInt(buffer, offset);
                }
            }
        }

        #endregion

        #region Private members

        Byte[] _sharedBuffer;

        List<char> _typeTags = new List<char>(8);
        List<int> _offsets = new List<int>(8);

        #endregion
    }
}
