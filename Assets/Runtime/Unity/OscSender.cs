using UnityEngine;
using System;
using System.Reflection;

namespace OscJack2
{
    public sealed class OscSender : MonoBehaviour
    {
        #region Editable attributes

        [SerializeField] string _ipAddress = "127.0.0.1";
        [SerializeField] int _udpPort = 9000;
        [SerializeField] string _oscAddress = "/bang";

        [SerializeField] Component _dataSource;
        [SerializeField] string _propertyName;

        #endregion

        #region Public manual sender methods

        public void Send()
        {
            _client.Send(_oscAddress, 0);
        }

        public void Send(int data)
        {
            _client.Send(_oscAddress, data);
        }

        public void Send(float data)
        {
            _client.Send(_oscAddress, data);
        }

        public void Send(Vector2 data)
        {
            _client.Send(_oscAddress, data.x, data.y);
        }

        public void Send(Vector3 data)
        {
            _client.Send(_oscAddress, data.x, data.y, data.z);
        }

        public void Send(Vector4 data)
        {
            _client.Send(_oscAddress, data.x, data.y, data.z, data.w);
        }

        public void Send(Vector2Int data)
        {
            _client.Send(_oscAddress, data.x, data.y);
        }

        public void Send(Vector3Int data)
        {
            _client.Send(_oscAddress, data.x, data.y, data.z);
        }

        public void Send(string data)
        {
            _client.Send(_oscAddress, data);
        }

        #endregion

        #region Internal sender methods

        int _intValue = Int32.MaxValue;

        void CheckSend(int data)
        {
            if (data == _intValue) return;
            _client.Send(_oscAddress, data);
            _intValue = data;
        }

        float _floatValue = Single.MaxValue;

        void CheckSend(float data)
        {
            if (data == _floatValue) return;
            _client.Send(_oscAddress, data);
            _floatValue = data;
        }

        Vector2 _vector2Value = new Vector2(Single.MaxValue, 0);

        void CheckSend(Vector2 data)
        {
            if (data == _vector2Value) return;
            _client.Send(_oscAddress, data.x, data.y);
            _vector2Value = data;
        }

        Vector3 _vector3Value = new Vector3(Single.MaxValue, 0, 0);

        void CheckSend(Vector3 data)
        {
            if (data == _vector3Value) return;
            _client.Send(_oscAddress, data.x, data.y, data.z);
            _vector3Value = data;
        }

        Vector4 _vector4Value = new Vector4(Single.MaxValue, 0, 0, 0);

        void CheckSend(Vector4 data)
        {
            if (data == _vector4Value) return;
            _client.Send(_oscAddress, data.x, data.y, data.z, data.w);
            _vector4Value = data;
        }

        Vector2Int _vector2IntValue = new Vector2Int(Int32.MaxValue, 0);

        void CheckSend(Vector2Int data)
        {
            if (data == _vector2IntValue) return;
            _client.Send(_oscAddress, data.x, data.y);
            _vector2IntValue = data;
        }

        Vector3Int _vector3IntValue = new Vector3Int(Int32.MaxValue, 0, 0);

        void CheckSend(Vector3Int data)
        {
            if (data == _vector3IntValue) return;
            _client.Send(_oscAddress, data.x, data.y, data.z);
            _vector3IntValue = data;
        }

        string _stringValue = string.Empty;

        void CheckSend(string data)
        {
            if (data == _stringValue) return;
            _client.Send(_oscAddress, data);
            _stringValue = data;
        }

        #endregion

        #region MonoBehaviour implementation

        OscClient _client;
        PropertyInfo _propertyInfo;

        void Start()
        {
            _client = OscMaster.GetClient(_ipAddress, _udpPort);
            _propertyInfo = _dataSource.GetType().GetProperty(_propertyName);
        }

        void Update()
        {
            var type = _propertyInfo.PropertyType;
            var value = _propertyInfo.GetValue(_dataSource, null); // boxing!!

                 if (type == typeof(int       )) CheckSend((int       )value);
            else if (type == typeof(float     )) CheckSend((float     )value);
            else if (type == typeof(string    )) CheckSend((string    )value);
            else if (type == typeof(Vector2   )) CheckSend((Vector2   )value);
            else if (type == typeof(Vector3   )) CheckSend((Vector3   )value);
            else if (type == typeof(Vector4   )) CheckSend((Vector4   )value);
            else if (type == typeof(Vector2Int)) CheckSend((Vector2Int)value);
            else if (type == typeof(Vector3Int)) CheckSend((Vector3Int)value);
        }

        #endregion
    }
}
