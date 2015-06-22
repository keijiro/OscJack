using UnityEngine;
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

        #region Public Methods

        public OscDirectory()
        {
            _server = new OscServer();
            _dataMap = new Dictionary<string, OscMessage>();

            #if UNITY_EDITOR
            _messageHistory = new Queue<OscMessage>();
            #endif

            _server.Start();
        }

        public void StartServer()
        {
            _server.Start();
        }

        public void TerminateServer()
        {
            _server.Close();
        }

        public System.Object[] GetData(string path)
        {
            UpdateIfNeeded();
            OscMessage message;
            _dataMap.TryGetValue(path, out message);
            return message.data;
        }

        #endregion

        #region Private Objects And Functions

        // Server object and path-to-data map
        OscServer _server;
        Dictionary<string, OscMessage> _dataMap;

        // Last update frame number
        int _lastFrame;

        void UpdateIfNeeded()
        {
            if (Application.isPlaying)
            {
                var frame = Time.frameCount;
                if (frame != _lastFrame) {
                    UpdateState();
                    _lastFrame = frame;
                }
            }
            else
            {
                #if UNITY_EDITOR
                if (CheckUpdateInterval()) UpdateState();
                #endif
            }
        }

        void UpdateState()
        {
            while (_server.MessageCount > 0)
            {
                var message = _server.PopMessage();
                _dataMap[message.path] = message;

                #if UNITY_EDITOR
                // Record the message.
                _totalMessageCount++;
                _messageHistory.Enqueue(message);
                #endif
            }

            #if UNITY_EDITOR
            // Truncate the history.
            while (_messageHistory.Count > 8)
                _messageHistory.Dequeue();
            #endif
        }

        #endregion

        #region Editor Support

        #if UNITY_EDITOR

        // Update timer
        const float _updateInterval = 1.0f / 10;
        float _lastUpdateTime;

        bool CheckUpdateInterval()
        {
            var current = Time.realtimeSinceStartup;
            if (current - _lastUpdateTime > _updateInterval || current < _lastUpdateTime) {
                _lastUpdateTime = current;
                return true;
            }
            return false;
        }

        // Total message count
        int _totalMessageCount;

        public int TotalMessageCount {
            get {
                UpdateIfNeeded();
                return _totalMessageCount;
            }
        }

        // Message history
        Queue<OscMessage> _messageHistory;

        public Queue<OscMessage> History {
            get { return _messageHistory; }
        }

        #endif

        #endregion
    }
}
