using UnityEngine;

public static class ComponentExtensions
{
    public static bool HasComponent<T>(this Component component) where T : Component => component.GetComponent<T>() != null;
}
