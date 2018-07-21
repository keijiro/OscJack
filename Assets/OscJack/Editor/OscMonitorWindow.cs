// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

namespace OscJack
{
    class OscMonitorWindow : EditorWindow
    {
        [MenuItem("Window/OSC Monitor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<OscMonitorWindow>("OSC Monitor");
        }

        // Used to divide the update cycle
        const int _updateInterval = 20;
        int _countToUpdate;

        // Server list which have been already under observation
        List<OscServer> _knownServers = new List<OscServer>();

        // Log line array and log counter (used to detect updates)
        StringBuilder _stringBuilder = new StringBuilder();
        string[] _logLines = new string[32];
        int _logCount;
        int _lastLogCount;

        void MonitorCallback(string address, OscDataHandle data)
        {
            _stringBuilder.Length = 0;
            _stringBuilder.Append(address).Append(": ");

            var ecount = data.GetElementCount();
            for (var i = 0; i < ecount; i++)
            {
                _stringBuilder.Append(data.GetElementAsString(i));
                if (i < ecount - 1) _stringBuilder.Append(", ");
            }

            _logLines[_logCount] = _stringBuilder.ToString();
            _logCount = (_logCount + 1) % _logLines.Length;
        }

        void Update()
        {
            // We put some intervals between updates to decrease the CPU load.
            if (--_countToUpdate > 0) return;
            _countToUpdate = _updateInterval;

            // Register the monitor callback to newly created servers.
            foreach (var server in OscServer.ServerList)
            {
                if (_knownServers.Contains(server)) continue;
                server.MessageDispatcher.AddCallback(string.Empty, MonitorCallback);
            }

            // Copy the current server list to knownServers.
            _knownServers.Clear();
            foreach (var server in OscServer.ServerList) _knownServers.Add(server);

            // Invoke repaint if there are new log lines.
            if (_logCount != _lastLogCount) Repaint();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            var maxLog = _logLines.Length;
            for (var i = 0; i < maxLog; i++)
            {
                var idx = (_logCount + maxLog - 1 - i) % maxLog;
                var line = _logLines[idx];
                if (line == null) break;
                EditorGUILayout.LabelField(line);
            }

            EditorGUILayout.EndVertical();

            _lastLogCount = _logCount;
        }
    }
}
