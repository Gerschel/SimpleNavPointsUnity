using UnityEngine;

namespace RPG.Core.NavPoints
{
    public class WaypointManager : MonoBehaviour
    {
        [SerializeField]
        public Vector3[] waypoints; // Array of waypoints (positions)
        
        //serialize field so it saves to the specific instance of the object
        #if UNITY_EDITOR
        [SerializeField]
        public bool isSnappingEnabled = true;

        [SerializeField]
        public LayerMask snapLayerMask = 0;  // Default to "Default" layer (0)
        #endif

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
