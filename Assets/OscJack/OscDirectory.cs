using System;
using System.Collections.Generic;

namespace OscJack
{
    public class OscDirectory
    {
        #region Single Pattern

        static OscDirectory _instance;

        public static OscDirectory Instance {
            get {
                if (_instance == null)
                    _instance = new OscDirectory();
                return _instance;
            }
        }

        #endregion

        OscServer _server;
        Dictionary<string, OscMessage> _dataMap;

        public OscDirectory()
        {
            _server = new OscServer();
            _dataMap = new Dictionary<string, OscMessage>();
        }

        public void StartServer()
        {
            _server.Start();
        }

        public void TerminateServer()
        {
            _server.Close();
        }

        public void Update()
        {
            while (_server.MessageCount > 0)
            {
                var msg = _server.PopMessage();
                _dataMap[msg.path] = msg;
            }
        }

        public Object[] GetData(string path)
        {
            OscMessage msg;
            _dataMap.TryGetValue(path, out msg);
            return msg.data;
        }
    }
}
