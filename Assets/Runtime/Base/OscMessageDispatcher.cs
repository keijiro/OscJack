using System;
using System.Collections.Generic;

namespace OscJack2
{
    public sealed class OscMessageDispatcher
    {
        #region Callback delegate definition

        public delegate void MessageCallback(string address, OscDataHandle data);

        #endregion

        #region Public accessors

        public void AddCallback(string address, MessageCallback callback)
        {
            lock (_delegatesLock)
            {
                if (_callbackMap.ContainsKey(address))
                    _callbackMap[address] += callback;
                else
                    _callbackMap[address] = callback;
            }
        }

        public void RemoveCallback(string address, MessageCallback callback)
        {
            lock (_delegatesLock)
            {
                var temp = _callbackMap[address] - callback;
                if (temp != null)
                    _callbackMap[address] = temp;
                else
                    _callbackMap.Remove(address);
            }
        }

        #endregion

        #region Handler invocation

        internal void Dispatch(string address, OscDataHandle data)
        {
            lock (_delegatesLock)
            {
                MessageCallback callback;

                // Address-specified callback
                if (_callbackMap.TryGetValue(address, out callback))
                    callback(address, data);

                // Monitor callback
                if (_callbackMap.TryGetValue(string.Empty, out callback))
                    callback(address, data);
            }
        }

        #endregion

        #region Private fields

        Dictionary<string, MessageCallback>
            _callbackMap = new Dictionary<string, MessageCallback>();

        Object _delegatesLock = new Object();

        #endregion
    }
}
