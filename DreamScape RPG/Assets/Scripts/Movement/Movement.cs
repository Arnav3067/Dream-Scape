using UnityEngine;
using DreamScape.Core;
using UnityEngine.AI;
using System;

namespace DreamScape.Locomotion {
    
    [RequireComponent(typeof(NavMeshAgent))]

    public class Movement : MonoBehaviour, IAction
    {
        [SerializeField] private float jumpForce = 100f;
        [SerializeField] private MovementAniamtions aniamtions;
        [SerializeField] private Health health;
        [SerializeField] private Rigidbody body;
        
        private ActionManager actionManager;
        private NavMeshAgent navMeshAgent;

        public Vector3 LocalVelocity {
            get {
                return transform.InverseTransformDirection(navMeshAgent.velocity);
            }
        }

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            actionManager = GetComponent<ActionManager>();
        }

        private void Start() {
            health.OnDeath += OnCharecterDeath;
        }

        private void Update() {
            PlayMovementAniamtions();
        }

        public void Move(Vector3 position) {
            navMeshAgent.SetDestination(position);
            navMeshAgent.isStopped = false;
        }

        public void Jump() {
            actionManager.CancelCurrentAction();
            navMeshAgent.enabled = false;
            body.AddForce((transform.up + transform.forward) * jumpForce, ForceMode.Impulse);
        }

        public void StartMoveAction(Vector3 position) {
            actionManager.StartAction(this);
            Move(position);
        }

        public void CancelAction() {
            navMeshAgent.isStopped = true;
        }

        private void PlayMovementAniamtions() {
            aniamtions.MapBlendTreeMovementVelocity(LocalVelocity.z);
        }

        private void OnCharecterDeath(object sender, EventArgs e) {
            navMeshAgent.enabled = false;
            health.OnDeath -= OnCharecterDeath;
        }

    }

    [System.Serializable]
    internal class MovementAniamtions  {

        [SerializeField] private Animator animator;

        public void MapBlendTreeMovementVelocity(float velocity) {
            animator.SetFloat("MovementSpeed", velocity);
        }
    }

}


