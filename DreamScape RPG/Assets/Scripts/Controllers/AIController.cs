using DreamScape.Combat;
using DreamScape.Core;
using DreamScape.Locomotion;
using UnityEngine;

namespace DreamScape.Controllers {

    public class AIController : MonoBehaviour {
        
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float chasCoolDownTime = 5f;
        [SerializeField] private float suspicionTime = 2f;
        [SerializeField] private Combatant combat;
        [SerializeField] private Health health;
        [SerializeField] private Movement movement;
        [SerializeField] private Transform target;

        private float timeWithoutCombat = Mathf.Infinity;
        private float timeElapsedDuringSuspicion;
        private Vector3 gaurdPosition; 
        private ActionManager actionManager;

        private void Start() {
            actionManager = GetComponent<ActionManager>();
            gaurdPosition = transform.position;
        }

        private void Update() {

            if (!health.isAlive) return;

            if (IsTargetInRange()) {
                BeginCombat(); print("in range"); // this also chases the player if they are afar
            }
            else if (timeWithoutCombat <= chasCoolDownTime) {
                ChaseBehaviour();
            }
            else {
                ShowSuspicionAndResetPosition();
            }
        }

        private void ShowSuspicionAndResetPosition() {

            actionManager.CancelCurrentAction();

            timeElapsedDuringSuspicion += Time.deltaTime;

            if (timeElapsedDuringSuspicion >= suspicionTime) {
                movement.StartMoveAction(gaurdPosition);
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
            // chase id done throught the first begincombat funtion as the target is set to the player
        }

        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, chaseRange);
            Gizmos.color = Color.red;
        }

    }
}