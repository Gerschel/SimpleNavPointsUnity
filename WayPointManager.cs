using UnityEngine;

namespace RPG.Core.NavPoints
{
    public class WaypointManager : MonoBehaviour
    {
        [SerializeField]
        private Vector3[] waypoints; // Array of waypoints (positions)
        
        [SerializeField]
        private bool isSnappingEnabled = true; // Snapping toggle for this specific instance

        [SerializeField]
        private LayerMask snapLayerMask = 0;  // Default to "Default" layer (0)

        public Vector3 GetWaypoint(int index)
        {
            return waypoints[index];
        }

        public Vector3 GetNextWaypoint(int index)
        {
            return waypoints[(index + 1) % waypoints.Length];
        }

        public Vector3 GetPreviousWaypoint(int index)
        {
            return waypoints[(index - 1 + waypoints.Length) % waypoints.Length];
        }
    }
}
