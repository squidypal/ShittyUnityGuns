using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSway : MonoBehaviour
{
    public Rigidbody playerRigidbody;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Debug.Log("Current Speed: " + squidypalPlayerController.currentSpeed);
        switch (squidypalPlayerController.currentSpeed)
        {
            case > 0.3f and < 0.7f:
                animator.SetBool("isMoving", true);
                animator.speed = 1f;
                break;
            case > 0.71f:
                animator.speed = 1.4f;
                break;
            default:
                animator.SetBool("isMoving", false);
                break;
        }
    }
}
