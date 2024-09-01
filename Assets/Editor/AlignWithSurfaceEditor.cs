using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BoardGame
{
    public class SurfaceAlignTool : EditorWindow
    {
        private GameObject targetObject;

        [MenuItem("Tools/Surface Align Tool")]
        public static void ShowWindow()
        {
            GetWindow<SurfaceAlignTool>("Surface Align Tool");
        }

        private void OnGUI()
        {
            GUILayout.Label("Align Object with Surface", EditorStyles.boldLabel);

            targetObject = (GameObject)
                EditorGUILayout.ObjectField(
                    "Target Object",
                    targetObject,
                    typeof(GameObject),
                    true
                );

            if (targetObject == null)
            {
                EditorGUILayout.HelpBox("Please select an object to align.", MessageType.Warning);
                return;
            }

            if (GUILayout.Button("Align"))
            {
                AlignObjectWithSurface();
            }
        }

        private void AlignObjectWithSurface()
        {
            if (targetObject == null)
            {
                Debug.LogWarning("No object selected.");
                return;
            }

            Ray ray = new Ray(targetObject.transform.position, targetObject.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetObject.transform.forward = -hit.normal;
            }
            else
            {
                Debug.LogWarning("No surface detected.");
            }
        }
    }
}
