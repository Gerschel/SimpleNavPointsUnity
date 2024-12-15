using UnityEditor;
using UnityEngine;

namespace RPG.Core.NavPoints
{
    [CustomEditor(typeof(WaypointManager))]
    public class WaypointManagerEditor : Editor
    {
        void OnSceneGUI()
        {
            WaypointManager waypointManager = (WaypointManager)target;

            if (waypointManager.waypoints == null || waypointManager.waypoints.Length == 0)
                return; // Prevent NullReferenceException if the array is null or empty

            // Loop through each waypoint and display handles for them
            for (int i = 0; i < waypointManager.waypoints.Length; i++)
            {
                // Draw a handle for each waypoint
                Vector3 waypointPosition = waypointManager.waypoints[i];
                Handles.Label(waypointPosition + Vector3.up * 0.5f, $"Waypoint {i + 1}");
                EditorGUI.BeginChangeCheck();
                
                // Draw the position handle
                waypointPosition = Handles.PositionHandle(waypointPosition, Quaternion.identity);

                // If snapping is enabled, apply snapping logic
                if (waypointManager.isSnappingEnabled)
                {

                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    RaycastHit hit;




                    // Perform a raycast with the selected layer mask
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << waypointManager.snapLayerMask)))
                    {
                        // Snap to the hit point
                        waypointPosition = hit.point;
                        //draw disc at handle oriented to normal of hit point
                        Handles.color = Color.green;
                        Handles.DrawWireDisc(hit.point, hit.normal, 0.5f);
                        //reset color
                        Handles.color = Color.white;
                    }
                }

                // Apply the position if the handle was moved
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(waypointManager, "Move Waypoint");
                    waypointManager.waypoints[i] = waypointPosition;
                }

                // Draw a line between the waypoints
                Handles.DrawLine(waypointManager.waypoints[i], waypointManager.waypoints[(i + 1) % waypointManager.waypoints.Length]);
            }

            // Repaint to make sure the handles refresh
            SceneView.RepaintAll();
        }

        // Custom Inspector GUI to display snapping options
        public override void OnInspectorGUI()
        {
            WaypointManager waypointManager = (WaypointManager)target;

            // Display the array of waypoints in the Inspector
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("waypoints"), true);
            serializedObject.ApplyModifiedProperties();

            // Add a foldout for snapping options
            EditorGUILayout.Space();
            //section divider
            EditorGUILayout.LabelField("Snapping Options", EditorStyles.boldLabel);
            


            // Snapping toggle
            EditorGUILayout.LabelField("Grab handles axis locks are (mostly) meaningless when raycast snapping is enabled", EditorStyles.helpBox);
            waypointManager.isSnappingEnabled = EditorGUILayout.Toggle("Enable Raycast Snapping", waypointManager.isSnappingEnabled);

            EditorGUI.BeginDisabledGroup(!waypointManager.isSnappingEnabled);

            // LayerMask dropdown for selecting which layers the raycast will interact with
            waypointManager.snapLayerMask = EditorGUILayout.LayerField("Snap To ... on layer", waypointManager.snapLayerMask);

            EditorGUI.EndDisabledGroup();
        }
    }
}
