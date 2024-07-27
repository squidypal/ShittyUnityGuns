using System;
using UnityEngine;

public class FollowParent : MonoBehaviour {

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        // calculate target rotation (reversed)
        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right); // Inverted the direction here
        Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up); // Inverted the direction here

        Quaternion targetRotation = rotationX * rotationY;

        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
