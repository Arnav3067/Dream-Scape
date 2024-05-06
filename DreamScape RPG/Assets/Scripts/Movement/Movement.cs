using UnityEngine;
using DreamScape.Core;
using UnityEngine.AI;
 
namespace DreamScape.Locomotion {
    
    [RequireComponent(typeof(NavMeshAgent))]

    public class Movement : MonoBehaviour, IAction
    {

        [SerializeField] private MovementAniamtions aniamtions;

        private NavMeshAgent navMeshAgent;

        public Vector3 LocalVelocity {
            get {
                return transform.InverseTransformDirection(navMeshAgent.velocity);
            }
        }

        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            PlayMovementAniamtions();
        }


        private void PlayMovementAniamtions() {
            aniamtions.MapBlendTreeMovementVelocity(LocalVelocity.z);
        }

        public void Move(Vector3 position) {
            navMeshAgent.SetDestination(position);
            navMeshAgent.isStopped = false;
        }

        public void StartMoveAction(Vector3 position) {
            ActionManager.Instance.StartAction(this);
            Move(position);
        }

        public void CancelAction() {
            navMeshAgent.isStopped = true;
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


