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
                    }
                }

                // Apply the position if the handle was moved
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(waypointManager, "Move Waypoint");
                    waypointManager.waypoints[i] = waypointPosition;
                }

                // Optionally, you can draw a label or marker at each waypoint
                Handles.Label(waypointPosition + Vector3.up * 0.5f, $"Waypoint {i + 1}");
                
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
            EditorGUILayout.LabelField("Snapping Options", EditorStyles.boldLabel);

            // Snapping toggle
            waypointManager.isSnappingEnabled = EditorGUILayout.Toggle("Enable Snapping", waypointManager.isSnappingEnabled);

            EditorGUI.BeginDisabledGroup(!waypointManager.isSnappingEnabled);

            // LayerMask dropdown for selecting which layers the raycast will interact with
            waypointManager.snapLayerMask = EditorGUILayout.LayerField("Snap Layer Mask", waypointManager.snapLayerMask);

            EditorGUI.EndDisabledGroup();
        }
    }
}
