using UnityEngine;
using DreamScape.Locomotion;
using DreamScape.Core;
using UnityEngine.AI;


namespace DreamScape.Combat {
    
    public class Combatant : MonoBehaviour, IAction
    {
        [Header("Combat Settings")]

        [SerializeField] private float hitRange = 3.0f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float timeBetweenAtttacks = 0.6f;

        [Header("External References")]

        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private CombatAnimations combatAntimations;

        private Transform target;
        private Health targetHealth;
        private Movement playerMovement;
        private float defaultRange = 0;
        private float stoppingOffset = 0.5f;
        private float timeSinceLastAttack;

        private bool isTargetAlive {get {return targetHealth.isAlive;}}

        private float CurrentRange {get {return navMeshAgent.stoppingDistance;}}

        // there is a stopping offset because the charecter stops abruplty
        // hence this gives a bit more distance to walk and then stop
        private Vector3 CurrentPositionWithOffset {get {return transform.position + (transform.forward * stoppingOffset);}}
        

        private void Awake() {
            TryGetComponent(out playerMovement);
        }

        private void Update() {

            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return; // begin combat sequence void Attack()
            
            if (!isTargetAlive) return;

            MovePlayerToTarget(); // move to the target
            
            if (CheckTargetReached()) { // if the player ahs reached the destination

                AttackSequence(); // then use your might to attack

            }
        }


        // interface function (IAction)
        public void CancelAction() {
            target = null;
            SetStoppingDistanceTo(defaultRange);
            combatAntimations.StopAttackAnimation();
        }
 
        public void StartCombatAction(Transform target) {
            ActionManager.Instance.StartAction(this);
            BeginCombat(target);
        }

        public void BeginCombat(Transform combatTarget) {
            target = combatTarget;
            targetHealth = target.GetComponent<Health>();
        }

        private void MovePlayerToTarget() {
            playerMovement.Move(target.position);
        }

        private void AttackSequence() {
            transform.LookAt(target); // stare your target as if your life depends in it

            playerMovement.Move(CurrentPositionWithOffset);

            if (timeSinceLastAttack >= timeBetweenAtttacks) {
                TriggerAttackAnimation();
            }
        }

        private void TriggerAttackAnimation() {
            timeSinceLastAttack = 0;
            combatAntimations.PlayAttackAniamtion();
        }
        
        private void SetStoppingDistanceTo(float stoppingDistance) {
            navMeshAgent.stoppingDistance = stoppingDistance;
        }

        private bool CheckTargetReached() {
            SetStoppingDistanceTo(hitRange);
            
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance - CurrentRange <= Mathf.Epsilon) return true;

            return false;
        }

        // Animation event
        public void Hit() {
            if (target != null) targetHealth.Damage(damage);
        }
    }

    [System.Serializable]
    internal class CombatAnimations {

        [SerializeField] private Animator animator;

        private const string ATTACK_TRIGGER = "Attack";
        private const string STOP_ATTACK_TRIGGER = "StopAttack";

        public void PlayAttackAniamtion() {
            animator.ResetTrigger(STOP_ATTACK_TRIGGER);
            animator.SetTrigger(ATTACK_TRIGGER);
        }

        public void StopAttackAnimation() {
            animator.ResetTrigger(ATTACK_TRIGGER);
            animator.SetTrigger(STOP_ATTACK_TRIGGER);
        }
    }

}