#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Moonlight
{
    [ExecuteInEditMode]
    public class MoonlightMenu
    {
        [MenuItem("Moonlight/Navmesh/Show Navmesh Renderers _F11")]
        private static void ShowNavmesh()
        {
            ToggleNavMesh(true);
        }

        [MenuItem("Moonlight/Navmesh/Hide Navmesh Renderers _F12")]
        private static void HideNavmesh()
        {
            ToggleNavMesh(false);
        }

        private static void ToggleNavMesh(bool enabled)
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                var rootObjects = prefabStage.scene.GetRootGameObjects();
                foreach (var gameObject in rootObjects)
                {
                    var colliders = gameObject.transform.GetComponentsInChildren<MeshCollider>();
                    foreach (var collider in colliders)
                    {
                        if (collider.CompareTag("Navmesh"))
                        {
                            var meshRenderer = collider.GetComponent<MeshRenderer>();
                            if (meshRenderer != null)
                            {
                                meshRenderer.enabled = enabled;
                            }
                        }
                    }
                }
            }
            else
            {
                GameObject[] navmeshObjects = GameObject.FindGameObjectsWithTag("Navmesh");
                foreach (GameObject navObject in navmeshObjects)
                {
                    if (navObject.GetComponent<MeshRenderer>() is MeshRenderer mesh)
                    {
                        mesh.enabled = enabled;
                    }
                }
            }
        }
    }
}
#endif
