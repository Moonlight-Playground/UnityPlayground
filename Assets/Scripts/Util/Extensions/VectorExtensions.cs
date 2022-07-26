using UnityEngine;

public static class VectorExtensions
{
    #region Vector3
    public static Vector3 SetAny(this Vector3 vector, float x = default, float y = default, float z = default) =>
        new Vector3(x == default ? vector.x : x, y == default ? vector.y : y, z == default ? vector.z : z);

    public static Vector3 SetX(this Vector3 vector, float x) => new Vector3(x, vector.y, vector.z);
    public static Vector3 SetY(this Vector3 vector, float y) => new Vector3(vector.x, y, vector.z);
    public static Vector3 SetZ(this Vector3 vector, float z) => new Vector3(vector.x, vector.y, z);

    public static Vector3 ZeroX(this Vector3 vector) => new Vector3(0f, vector.y, vector.z);
    public static Vector3 ZeroY(this Vector3 vector) => new Vector3(vector.x, 0f, vector.z);
    public static Vector3 ZeroZ(this Vector3 vector) => new Vector3(vector.x, vector.y, 0f);
    #endregion

    #region Vector2
    public static Vector2 SetAny(this Vector2 vector, float x = default, float y = default) =>
        new Vector2(x == default ? vector.x : x, y == default ? vector.y : y);

    public static Vector2 SetX(this Vector2 vector, float x) => new Vector2(x, vector.y);
    public static Vector2 SetY(this Vector2 vector, float y) => new Vector2(vector.x, y);

    public static Vector2 ZeroX(this Vector2 vector) => new Vector2(0f, vector.y);
    public static Vector2 ZeroY(this Vector2 vector) => new Vector2(vector.x, 0f);
    #endregion
}
