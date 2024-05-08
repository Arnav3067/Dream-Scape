using UnityEngine;

namespace DreamScape.Controllers {

    public class PatrolPath : MonoBehaviour {

        [SerializeField] private float waypointGizmosRadius = 0.5f;

        private int numberOfWaypoints {get {return transform.childCount;}}

        private void OnDrawGizmos() {
            for (int i = 0;i < numberOfWaypoints; i++) {

                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmosRadius);

                int j = GetNextWaypointIndex(i);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public Vector3 GetWaypoint(int i) {
            return transform.GetChild(i).position;
        }

        public int GetNextWaypointIndex(int i) {
            if (i + 1 == numberOfWaypoints) return 0;
            return i + 1;
        }
    }
}