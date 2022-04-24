// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using System;
using System.Reflection;

namespace OscJack
{
    [AddComponentMenu("OSC/Property Sender")]
    public sealed class OscPropertySender : MonoBehaviour
    {
        #region Editable fields

        [SerializeField] OscConnection _connection = null;
        [SerializeField] string _oscAddress = "/unity";
        [SerializeField] Component _dataSource = null;
        [SerializeField] string _propertyName = "";
        [SerializeField] bool _keepSending = false;

        #endregion

        #region Internal members

        OscClient _client;
        PropertyInfo _propertyInfo;

        void UpdateSettings()
        {
            if (_connection != null)
                _client = OscMaster.GetSharedClient(_connection.host, _connection.port);
            else
                _client = null;

            if (_dataSource != null && !string.IsNullOrEmpty(_propertyName))
                _propertyInfo = _dataSource.GetType().GetProperty(_propertyName);
            else
                _propertyInfo = null;
        }

        #endregion

        #region MonoBehaviour implementation

        void Start()
        {
            UpdateSettings();
        }

        void OnValidate()
        {
            if (Application.isPlaying) UpdateSettings();
        }

        void Update()
        {
            if (_client == null || _propertyInfo == null) return;

            var type = _propertyInfo.PropertyType;
            var value = _propertyInfo.GetValue(_dataSource, null); // boxing!!

                 if (type == typeof(int       )) Send((int       )value);
            else if (type == typeof(float     )) Send((float     )value);
            else if (type == typeof(string    )) Send((string    )value);
            else if (type == typeof(Vector2   )) Send((Vector2   )value);
            else if (type == typeof(Vector3   )) Send((Vector3   )value);
            else if (type == typeof(Vector4   )) Send((Vector4   )value);
            else if (type == typeof(Vector2Int)) Send((Vector2Int)value);
            else if (type == typeof(Vector3Int)) Send((Vector3Int)value);
        }

        #endregion

        #region Sender methods

        int _intValue = Int32.MaxValue;

        public void Send(int data)
        {
            if (!_keepSending && data == _intValue) return;
            _client.Send(_oscAddress, data);
            _intValue = data;
        }

        float _floatValue = Single.MaxValue;

        public void Send(float data)
        {
            if (!_keepSending && data == _floatValue) return;
            _client.Send(_oscAddress, data);
            _floatValue = data;
        }

        Vector2 _vector2Value = new Vector2(Single.MaxValue, 0);

        public void Send(Vector2 data)
        {
            if (!_keepSending && data == _vector2Value) return;
            _client.Send(_oscAddress, data.x, data.y);
            _vector2Value = data;
        }

        Vector3 _vector3Value = new Vector3(Single.MaxValue, 0, 0);

        public void Send(Vector3 data)
        {
            if (!_keepSending && data == _vector3Value) return;
            _client.Send(_oscAddress, data.x, data.y, data.z);
            _vector3Value = data;
        }

        Vector4 _vector4Value = new Vector4(Single.MaxValue, 0, 0, 0);

        public void Send(Vector4 data)
        {
            if (!_keepSending && data == _vector4Value) return;
            _client.Send(_oscAddress, data.x, data.y, data.z, data.w);
            _vector4Value = data;
        }

        Vector2Int _vector2IntValue = new Vector2Int(Int32.MaxValue, 0);

        public void Send(Vector2Int data)
        {
            if (!_keepSending && data == _vector2IntValue) return;
            _client.Send(_oscAddress, data.x, data.y);
            _vector2IntValue = data;
        }

        Vector3Int _vector3IntValue = new Vector3Int(Int32.MaxValue, 0, 0);

        public void Send(Vector3Int data)
        {
            if (!_keepSending && data == _vector3IntValue) return;
            _client.Send(_oscAddress, data.x, data.y, data.z);
            _vector3IntValue = data;
        }

        string _stringValue = string.Empty;

        public void Send(string data)
        {
            if (!_keepSending && data == _stringValue) return;
            _client.Send(_oscAddress, data);
            _stringValue = data;
        }

        #endregion
    }
}
