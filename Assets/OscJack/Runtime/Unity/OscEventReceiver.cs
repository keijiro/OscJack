// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace OscJack
{
    [AddComponentMenu("OSC/Event Receiver")]
    public sealed class OscEventReceiver : MonoBehaviour
    {
        #region Receiver data types

        public enum DataType
        {
            None, Int, Float, String,
            Vector2, Vector3, Vector4,
            Vector2Int, Vector3Int
        }

        #endregion

        #region Receiver event classes

        [System.Serializable] class IntEvent        : UnityEvent<int> {}
        [System.Serializable] class FloatEvent      : UnityEvent<float> {}
        [System.Serializable] class StringEvent     : UnityEvent<string> {}
        [System.Serializable] class Vector2Event    : UnityEvent<Vector2> {}
        [System.Serializable] class Vector3Event    : UnityEvent<Vector3> {}
        [System.Serializable] class Vector4Event    : UnityEvent<Vector4> {}
        [System.Serializable] class Vector2IntEvent : UnityEvent<Vector2Int> {}
        [System.Serializable] class Vector3IntEvent : UnityEvent<Vector3Int> {}

        #endregion

        #region Editable fields

        [SerializeField] int _udpPort = 9000;
        [SerializeField] string _oscAddress = "/unity";
        [SerializeField] DataType _dataType;

        [SerializeField] UnityEvent _event;
        [SerializeField] IntEvent _intEvent;
        [SerializeField] FloatEvent _floatEvent;
        [SerializeField] Vector2Event _vector2Event;
        [SerializeField] Vector3Event _vector3Event;
        [SerializeField] Vector4Event _vector4Event;
        [SerializeField] Vector2IntEvent _vector2IntEvent;
        [SerializeField] Vector3IntEvent _vector3IntEvent;
        [SerializeField] StringEvent _stringEvent;

        #endregion

        #region Internal members

        // Used to store values on initialization
        int _currentPort;
        string _currentAddress;

        // Incoming data queues
        int _bangCount;
        Queue<int> _intQueue;
        Queue<float> _floatQueue;
        Queue<string> _stringQueue;

        #endregion

        #region MonoBehaviour implementation

        void OnEnable()
        {
            if (string.IsNullOrEmpty(_oscAddress))
            {
                _currentAddress = null;
                return;
            }

            var server = OscMaster.GetServer(_udpPort);
            server.MessageDispatcher.AddCallback(_oscAddress, OnDataReceive);

            _currentPort = _udpPort;
            _currentAddress = _oscAddress;

            switch (_dataType)
            {
                case DataType.Int:
                case DataType.Vector2Int:
                case DataType.Vector3Int:
                    if (_intQueue == null) _intQueue = new Queue<int>(4);
                    break;

                case DataType.Float:
                case DataType.Vector2:
                case DataType.Vector3:
                case DataType.Vector4:
                    if (_floatQueue == null) _floatQueue = new Queue<float>(4);
                    break;

                case DataType.String:
                    if (_stringQueue == null) _stringQueue = new Queue<string>();
                    break;
            }
        }

        void OnDisable()
        {
            if (string.IsNullOrEmpty(_currentAddress)) return;

            var server = OscMaster.GetServer(_currentPort);
            server.MessageDispatcher.RemoveCallback(_currentAddress, OnDataReceive);

            _currentAddress = null;
        }

        void OnValidate()
        {
            if (Application.isPlaying)
            {
                OnDisable();
                OnEnable();
            }
        }

        void Update()
        {
            switch (_dataType)
            {
                case DataType.None:
                    while (_bangCount > 0)
                    {
                        _event.Invoke();
                        _bangCount--;
                    }
                    break;

                case DataType.Int:
                    while (_intQueue.Count > 0)
                        _intEvent.Invoke(_intQueue.Dequeue());
                    break;

                case DataType.Float:
                    while (_floatQueue.Count > 0)
                        _floatEvent.Invoke(_floatQueue.Dequeue());
                    break;

                case DataType.String:
                    while (_stringQueue.Count > 0)
                        _stringEvent.Invoke(_stringQueue.Dequeue());
                    break;

                case DataType.Vector2:
                    while (_floatQueue.Count > 0)
                        _vector2Event.Invoke(new Vector2(
                            _floatQueue.Dequeue(),
                            _floatQueue.Dequeue()
                        ));
                    break;

                case DataType.Vector3:
                    while (_floatQueue.Count > 0)
                        _vector3Event.Invoke(new Vector3(
                            _floatQueue.Dequeue(),
                            _floatQueue.Dequeue(),
                            _floatQueue.Dequeue()
                        ));
                    break;

                case DataType.Vector4:
                    while (_floatQueue.Count > 0)
                        _vector3Event.Invoke(new Vector4(
                            _floatQueue.Dequeue(),
                            _floatQueue.Dequeue(),
                            _floatQueue.Dequeue(),
                            _floatQueue.Dequeue()
                        ));
                    break;

                case DataType.Vector2Int:
                    while (_intQueue.Count > 0)
                        _vector2IntEvent.Invoke(new Vector2Int(
                            _intQueue.Dequeue(),
                            _intQueue.Dequeue()
                        ));
                    break;

                case DataType.Vector3Int:
                    while (_intQueue.Count > 0)
                        _vector3IntEvent.Invoke(new Vector3Int(
                            _intQueue.Dequeue(),
                            _intQueue.Dequeue(),
                            _intQueue.Dequeue()
                        ));
                    break;
            }
        }

        #endregion

        #region OSC event callback

        void OnDataReceive(string address, OscDataHandle data)
        {
            switch (_dataType)
            {
                case DataType.None:
                    _bangCount++;
                    break;

                case DataType.Int:
                    _intQueue.Enqueue(data.GetElementAsInt(0));
                    break;

                case DataType.Float:
                    _floatQueue.Enqueue(data.GetElementAsFloat(0));
                    break;

                case DataType.String:
                    _stringQueue.Enqueue(data.GetElementAsString(0));
                    break;

                case DataType.Vector2:
                    _floatQueue.Enqueue(data.GetElementAsFloat(0));
                    _floatQueue.Enqueue(data.GetElementAsFloat(1));
                    break;

                case DataType.Vector3:
                    _floatQueue.Enqueue(data.GetElementAsFloat(0));
                    _floatQueue.Enqueue(data.GetElementAsFloat(1));
                    _floatQueue.Enqueue(data.GetElementAsFloat(2));
                    break;

                case DataType.Vector4:
                    _floatQueue.Enqueue(data.GetElementAsFloat(0));
                    _floatQueue.Enqueue(data.GetElementAsFloat(1));
                    _floatQueue.Enqueue(data.GetElementAsFloat(2));
                    _floatQueue.Enqueue(data.GetElementAsFloat(3));
                    break;

                case DataType.Vector2Int:
                    _intQueue.Enqueue(data.GetElementAsInt(0));
                    _intQueue.Enqueue(data.GetElementAsInt(1));
                    break;

                case DataType.Vector3Int:
                    _intQueue.Enqueue(data.GetElementAsInt(0));
                    _intQueue.Enqueue(data.GetElementAsInt(1));
                    _intQueue.Enqueue(data.GetElementAsInt(2));
                    break;
            }
        }

        #endregion
    }
}
