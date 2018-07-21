using UnityEngine;
using UnityEditor;

namespace OscJack2
{
    [CanEditMultipleObjects, CustomEditor(typeof(OscReceiver))]
    class OscReceiverEditor : Editor
    {
        SerializedProperty _udpPort;
        SerializedProperty _oscAddress;
        SerializedProperty _dataType;

        SerializedProperty _event;
        SerializedProperty _intEvent;
        SerializedProperty _floatEvent;
        SerializedProperty _vector2Event;
        SerializedProperty _vector3Event;
        SerializedProperty _vector4Event;
        SerializedProperty _vector2IntEvent;
        SerializedProperty _vector3IntEvent;
        SerializedProperty _stringEvent;

        static class Labels
        {
            public static readonly GUIContent UDPPortNumber = new GUIContent("UDP Port Number");
            public static readonly GUIContent OSCAddress = new GUIContent("OSC Address");
        }

        void OnEnable()
        {
            _udpPort    = serializedObject.FindProperty("_udpPort");
            _oscAddress = serializedObject.FindProperty("_oscAddress");
            _dataType   = serializedObject.FindProperty("_dataType");

            _event           = serializedObject.FindProperty("_event");
            _intEvent        = serializedObject.FindProperty("_intEvent");
            _floatEvent      = serializedObject.FindProperty("_floatEvent");
            _vector2Event    = serializedObject.FindProperty("_vector2Event");
            _vector3Event    = serializedObject.FindProperty("_vector3Event");
            _vector4Event    = serializedObject.FindProperty("_vector4Event");
            _vector2IntEvent = serializedObject.FindProperty("_vector2IntEvent");
            _vector3IntEvent = serializedObject.FindProperty("_vector3IntEvent");
            _stringEvent     = serializedObject.FindProperty("_stringEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_udpPort, Labels.UDPPortNumber);
            EditorGUILayout.PropertyField(_oscAddress, Labels.OSCAddress);
            EditorGUILayout.PropertyField(_dataType);

            if (!_dataType.hasMultipleDifferentValues)
            {
                switch ((OscReceiver.DataType)_dataType.enumValueIndex)
                {
                    case OscReceiver.DataType.None:       EditorGUILayout.PropertyField(_event);           break;
                    case OscReceiver.DataType.Int:        EditorGUILayout.PropertyField(_intEvent);        break;
                    case OscReceiver.DataType.Float:      EditorGUILayout.PropertyField(_floatEvent);      break;
                    case OscReceiver.DataType.Vector2:    EditorGUILayout.PropertyField(_vector2Event);    break;
                    case OscReceiver.DataType.Vector3:    EditorGUILayout.PropertyField(_vector3Event);    break;
                    case OscReceiver.DataType.Vector4:    EditorGUILayout.PropertyField(_vector4Event);    break;
                    case OscReceiver.DataType.Vector2Int: EditorGUILayout.PropertyField(_vector2IntEvent); break;
                    case OscReceiver.DataType.Vector3Int: EditorGUILayout.PropertyField(_vector3IntEvent); break;
                    case OscReceiver.DataType.String:     EditorGUILayout.PropertyField(_stringEvent);     break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
