using UnityEngine;

namespace RPG.Core.NavPoints
{
    public class WaypointManager : MonoBehaviour
    {
        public Vector3[] waypoints; // Array of waypoints (positions)
        
        // Serialized snapping settings for each instance of the GameObject
        [Header("Snapping Options")]
        public bool isSnappingEnabled = true; // Snapping toggle for this specific instance
        public LayerMask snapLayerMask = 0;  // Default to "Default" layer (0)

        public Vector3 GetWaypoint(int index)
        {
            return waypoints[index];
        }
    }
}
