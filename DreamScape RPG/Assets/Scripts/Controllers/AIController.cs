using DreamScape.Combat;
using DreamScape.Core;
using DreamScape.Locomotion;
using UnityEngine;

namespace DreamScape.Controllers {

    public class AIController : MonoBehaviour {
        
        [Header("settings")]

        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float chasCoolDownTime = 5f;
        [SerializeField] private float suspicionTime = 2f;
        [SerializeField] private float waypointThreshold = 1f;
        [SerializeField] private float waypointsDwellTime = 2f;
        [SerializeField] private PatrolPath patrolPath;
        
        [Header("External References")]

        [SerializeField] private Combatant combat;
        [SerializeField] private Health health;
        [SerializeField] private Movement movement;
        [SerializeField] private Transform target;

        private float timeWithoutCombat = Mathf.Infinity;
        private float timeElapsedDuringSuspicion = Mathf.Infinity;
        private float timeSinceLastWaypoint = Mathf.Infinity;
        private Vector3 gaurdPosition; 
        private ActionManager actionManager;
        private int currentWaypointIndex = 0;

        private void Start() {
            actionManager = GetComponent<ActionManager>();
            gaurdPosition = transform.position;
        }

        private void Update() {

            if (!health.isAlive) return;

            if (IsTargetInRange()) {
                BeginCombat();
            }
            else if (timeWithoutCombat <= chasCoolDownTime) {
                ChaseBehaviour();
            }
            else {
                ShowSuspicionAndPatrol();
            }
        }



        private void BeginCombat() {
            combat.StartCombatAction(target);
            timeWithoutCombat = 0;
            timeElapsedDuringSuspicion = 0;
        }

        private bool IsTargetInRange() {

            Collider[] hits = Physics.OverlapSphere(transform.position, chaseRange);

            foreach (Collider hit in hits) {

                if (hit.transform == target) return true;

            }
            return false;
        }

        private void ChaseBehaviour() {
            timeWithoutCombat += Time.deltaTime;
            // chase is done throught the first begincombat funtion as the target is set to the player
        }

        private void ShowSuspicionAndPatrol() {

            actionManager.CancelCurrentAction();

            timeElapsedDuringSuspicion += Time.deltaTime;

            if (timeElapsedDuringSuspicion >= suspicionTime) {
                PatrolBehaviour();
            }
        }

        private void PatrolBehaviour() {

            timeSinceLastWaypoint += Time.deltaTime;
            Vector3 nextPosition = gaurdPosition;

            if (patrolPath != null) {
                
                if (OnWaypoint()) {
                    timeSinceLastWaypoint = 0;
                    CycleWaypoints();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceLastWaypoint >= waypointsDwellTime) {
                movement.StartMoveAction(nextPosition);
            }
        }

        private bool OnWaypoint() {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint <= waypointThreshold;
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoints() {
            currentWaypointIndex = patrolPath.GetNextWaypointIndex(currentWaypointIndex);
        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, chaseRange);
            Gizmos.color = Color.red;
        }

    }
}