using UnityEngine;

public class ObjectPool : GenericObjectPool<GameObject>
{
    protected override void InitializeNewObject(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
    }

    protected override void ActivateObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    protected override void DeactivateObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
