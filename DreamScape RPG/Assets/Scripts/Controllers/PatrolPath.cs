using UnityEngine;

namespace DreamScape.Controllers {

    public class PatrolPath : MonoBehaviour {

        [SerializeField] private float waypointGizmosRadius = 0.5f;

        private int numberOfWaypoints {get {return transform.childCount;}}

        private void OnDrawGizmos() {

            for (int i = 0; i < numberOfWaypoints; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmosRadius);
                
                if (i == transform.childCount - 1) {

                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i - numberOfWaypoints + 1).position);

                } else {

                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
                    
                }
            }
        }
    }
}