// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using System;
using System.Collections.Generic;

namespace OscJack
{
    internal sealed class OscPacketParser
    {
        #region Public Members

        public OscPacketParser(OscMessageDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Parse(Byte[] buffer, int length)
        {
            ScanMessage(buffer, 0, length);
        }

        #endregion

        #region Private Methods

        OscMessageDispatcher _dispatcher;
        OscDataHandle _dataHandle = new OscDataHandle();

        void ScanMessage(Byte[] buffer, int offset, int length)
        {
            // Where the next element begins if any
            var next = offset + length;

            // OSC address
            var address = OscDataTypes.ReadString(buffer, offset);
            offset += OscDataTypes.Align4(address.Length + 1);

            if (address == "#bundle")
            {
                // We don't use the timestamp data; Just skip it.
                offset += 8;

                // Keep reading until the next element begins.
                while (offset < next)
                {
                    // Get the length of the element.
                    var elementLength = OscDataTypes.ReadInt(buffer, offset);
                    offset += 4;

                    // Scan the bundle element in a recursive fashion.
                    ScanMessage(buffer, offset, elementLength);
                    offset += elementLength;
                }
            }
            else
            {
                // Retrieve the arguments and dispatch the message.
                _dataHandle.Scan(buffer, offset);
                _dispatcher.Dispatch(address, _dataHandle);
            }
        }

        #endregion
    }
}
