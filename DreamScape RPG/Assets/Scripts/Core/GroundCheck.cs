using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float groundTolerance = 0.2f;

    public bool IsGrounded() {
        return Physics.Raycast(transform.position, -transform.up, groundTolerance);
    }

}