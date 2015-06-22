using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

namespace OscJack
{
    class OscJackWindow : EditorWindow
    {
        #region Custom Editor Window Code

        [MenuItem("Window/OSC Jack")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<OscJackWindow>("OSC Jack");
        }

        void OnGUI()
        {
            // Message history
            var temp = "Recent OSC messages:";
            foreach (var message in OscDirectory.Instance.History)
                temp += "\n" + message.ToString();
            EditorGUILayout.HelpBox(temp, MessageType.None);
        }

        #endregion

        #region Update And Repaint

        const int _updateInterval = 20;
        int _countToUpdate;
        int _lastMessageCount;

        void Update()
        {
            if (--_countToUpdate > 0) return;

            var mcount = OscDirectory.Instance.TotalMessageCount;
            if (mcount != _lastMessageCount) {
                Repaint();
                _lastMessageCount = mcount;
            }

            _countToUpdate = _updateInterval;
        }

        #endregion
    }
}

