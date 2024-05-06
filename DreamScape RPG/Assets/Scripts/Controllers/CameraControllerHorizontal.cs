using UnityEngine;

namespace DreamScape.Controllers {

    public class CameraControllerHorizontal : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float angulerVeocity;

        private float HorizontalAxis {get {return Input.GetAxisRaw("Horizontal");}}

        private void LateUpdate() {

            transform.position = targetTransform.position;
            HorizontalMovement();
            
        }

        private void HorizontalMovement() {
            transform.Rotate(Vector3.up, -HorizontalAxis * angulerVeocity * Time.deltaTime);
        }
    }
}
