using System;
using DreamScape.Combat;
using DreamScape.Core;
using DreamScape.Locomotion;
using UnityEngine;


namespace DreamScape.Controllers
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Movement playerMovement;
        [SerializeField] private Combatant combat;
        [SerializeField] private Health playerHealth;
        [SerializeField] private ActionManager actionManager;
        [SerializeField] private GroundCheck groundCheck;
        [SerializeField] private float maxClickRange;
        [SerializeField] private float groundTolerance = 2;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask clickableLayer;

        public static PlayerController controller {get; private set;}

        public Vector3 mousePiosition{get {return Input.mousePosition;}}

        private void Awake() {
            controller = this;
        }

        private void LateUpdate() {

            if (!playerHealth.isAlive) return;
            if (CombastIfPossible()) return;
            if (MoveIfPossible()) return;
            
        }

        private bool CombastIfPossible() {

            RaycastHit[] hits = Physics.RaycastAll(GenerateCursorRay(), maxClickRange, clickableLayer);

            foreach (RaycastHit hit in hits) {
                AttackableTag tag = hit.transform.GetComponent<AttackableTag>();
                if (tag == null) continue; // if the object is not atatckable then just skip the lines bellow;

                if (GetMouseButtonDown(MouseButton.Left)) {
                    combat.StartCombatAction(hit.transform);                    
                }

                return true; // either hovering or attacking we know that the 
                //cursor is over something that is attakable
            } 
            return false;
        
        }

        private bool MoveIfPossible() {

            if (GetScreenToPointRayOutput(groundLayer, out RaycastHit hit)) {

                if (GetMouseButton(MouseButton.Left)) {
                    playerMovement.StartMoveAction(hit.point);
                }

                print(groundCheck.IsGrounded());
                
                if (Input.GetKeyDown(KeyCode.Space)){
                    print("jump");
                    playerMovement.Jump();
                }

                return true; // if the raycast is hitting something, 
                //it is true (either if the mouse button is pressed  or not) (hovering or clicked)
            }
            return false;
        }

        private bool GetScreenToPointRayOutput(LayerMask layerMask, out RaycastHit hit) {

            bool hasHit = Physics.Raycast(GenerateCursorRay(), out RaycastHit hitInfo, maxClickRange, layerMask);
            hit = hitInfo;

            return hasHit;
        }

        private bool GetMouseButtonDown(MouseButton button) {
            return Input.GetMouseButtonDown((int) button);
        }

        private bool GetMouseButton(MouseButton button) {
            return Input.GetMouseButton((int) button);
        }

        private Ray GenerateCursorRay() {
            return mainCamera.ScreenPointToRay(mousePiosition);
        }
    }

    public enum MouseButton {
        Left,
        Middle,
        right
    }
}


