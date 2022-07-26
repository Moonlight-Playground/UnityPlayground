using UnityEngine;

public static class GameobjectExtensions
{
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
        {
            t.gameObject.SetLayerRecursively(layer);
        }
    }

    public static void DestroyChildren(this GameObject parent)
    {
        Transform[] children = new Transform[parent.transform.childCount];
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i);
        }
        for (int i = 0; i < children.Length; i++)
        {
            GameObject.Destroy(children[i].gameObject);
        }
    }

    public static void MoveChildren(this GameObject from, GameObject to, bool keepWorldPosition = false)
    {
        Transform[] children = new Transform[from.transform.childCount];
        for (int i = 0; i < from.transform.childCount; i++)
        {
            children[i] = from.transform.GetChild(i);
        }
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetParent(to.transform, keepWorldPosition);
        }
    }
}
