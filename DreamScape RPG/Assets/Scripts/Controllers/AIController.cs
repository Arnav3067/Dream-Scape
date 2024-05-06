using DreamScape.Combat;
using DreamScape.Locomotion;
using UnityEngine;

namespace DreamScape.Controllers {

    public class AIController : MonoBehaviour {
        
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private Combatant combat;
        [SerializeField] private Movement movement;
        [SerializeField] private Transform target;

        private void Update() {
            if (IsTargetInRange()) {
                combat.BeginCombat(target);
            }
            else {
                
            }
        }

        private bool IsTargetInRange() {

            Collider[] hits = Physics.OverlapSphere(transform.position, chaseRange);

            foreach (Collider hit in hits) {

                if (hit.transform == target) return true;

            }
            return false;
        }

    }
}