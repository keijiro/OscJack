// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEditor;
using UnityEngine;

namespace OscJack
{
    [CanEditMultipleObjects, CustomEditor(typeof(OscConnection))]
    class OscConnectionEditor : Editor
    {
        SerializedProperty _type;
        SerializedProperty _host;
        SerializedProperty _port;

        GUIContent[] _typeLabels = { new GUIContent("UDP") };
        int[] _typeValues = { 0 };

        void OnEnable()
        {
            _type = serializedObject.FindProperty("type");
            _host = serializedObject.FindProperty("host");
            _port = serializedObject.FindProperty("port");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.IntPopup(_type, _typeLabels, _typeValues);
            EditorGUILayout.PropertyField(_host);
            EditorGUILayout.PropertyField(_port);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
