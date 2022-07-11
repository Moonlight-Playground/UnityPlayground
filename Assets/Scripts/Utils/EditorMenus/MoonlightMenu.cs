using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MoonlightMenu
{
    [MenuItem("Moonlight/Navmesh/Enable Navmesh Objects")]
    private static void EnableNavmesh()
    {
        foreach (GameObject go in GetObjectsInNavmeshLayer())
        {
            go.SetActive(true);
        }
    }

    [MenuItem("Moonlight/Navmesh/Disable Navmesh Objects")]
    private static void DisableNavmesh()
    {
        foreach (GameObject go in GetObjectsInNavmeshLayer())
        {
            go.SetActive(false);
        }
    }

    private static List<GameObject> GetObjectsInNavmeshLayer()
    {
        var gameObjects = new List<GameObject>();

        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.hideFlags == HideFlags.None && go.layer == (int)Moonlight.Layers.NavmeshGeneration)
            {
                gameObjects.Add(go);
            }
        }

        return gameObjects;
    }
}
