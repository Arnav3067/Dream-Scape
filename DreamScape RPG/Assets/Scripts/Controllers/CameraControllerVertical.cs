using UnityEngine;
using Cinemachine;


namespace DreamScape.Controllers {

    public class CameraControllerVertical : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera CMcamera;
        [SerializeField] private float velocity;
        [SerializeField] private float maxRange;
        [SerializeField] private float minRange;

        private CinemachineTransposer transposer;

        private float VerticalAxis {get {return Input.GetAxis("Vertical");}}
        
        private void Awake() {
            transposer = CMcamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void LateUpdate () {
            VerticalMovement();
        }

        private void VerticalMovement() {
            transposer.m_FollowOffset.y += VerticalAxis * velocity * Time.deltaTime;
        }
    }
}
