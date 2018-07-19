using UnityEngine;

namespace OscJack2
{
    public static class OscDataHandleExtensions
    {
        public static Vector2 GetValuesAsVector2(this OscDataHandle handle)
        {
            return new Vector2(
                handle.GetValueAsFloat(0),
                handle.GetValueAsFloat(1)
            );
        }

        public static Vector3 GetValuesAsVector3(this OscDataHandle handle)
        {
            return new Vector3(
                handle.GetValueAsFloat(0),
                handle.GetValueAsFloat(1),
                handle.GetValueAsFloat(2)
            );
        }

        public static Vector4 GetValuesAsVector4(this OscDataHandle handle)
        {
            return new Vector4(
                handle.GetValueAsFloat(0),
                handle.GetValueAsFloat(1),
                handle.GetValueAsFloat(2),
                handle.GetValueAsFloat(3)
            );
        }

        public static Vector2Int GetValuesAsVector2Int(this OscDataHandle handle)
        {
            return new Vector2Int(
                handle.GetValueAsInt(0),
                handle.GetValueAsInt(1)
            );
        }

        public static Vector3Int GetValuesAsVector3Int(this OscDataHandle handle)
        {
            return new Vector3Int(
                handle.GetValueAsInt(0),
                handle.GetValueAsInt(1),
                handle.GetValueAsInt(2)
            );
        }
    }
}
