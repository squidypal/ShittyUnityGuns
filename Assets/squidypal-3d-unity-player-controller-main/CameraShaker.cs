using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShaker : MonoBehaviour
{
    private bool isRecoiling;
    private bool isShakingCamera;
    private GameObject playerCamera; 
    private Vector3 originalLocalPosition; 
    private Quaternion originalLocalRotation;
    public Transform leftLeanTransform;
    public Transform rightLeanTransform;
    private Vector3 currentDesiredPosition;
    private Quaternion currentDesiredRotation;
    public GameObject light;
    public bool leaningEnabled = true;

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        playerCamera = transform.gameObject;
    }

    private Vector3 positionDampVelocity = Vector3.zero;
    private float rotationDampVelocity = 0f;
    private float smoothTime = 0.3f; // You can adjust this value to achieve the smoothness you want

    void Update()
    {
        if(!leaningEnabled) return;
        if (Input.GetKey(KeyCode.Q))
        {
            currentDesiredPosition = leftLeanTransform.localPosition;
            currentDesiredRotation = leftLeanTransform.localRotation;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            currentDesiredPosition = rightLeanTransform.localPosition;
            currentDesiredRotation = rightLeanTransform.localRotation;
        }
        else
        {
            currentDesiredPosition = originalLocalPosition;
            currentDesiredRotation = originalLocalRotation;
        }

        // Smoothly damp towards the desired position and rotation
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, currentDesiredPosition, ref positionDampVelocity, smoothTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, currentDesiredRotation, Time.deltaTime * (1 / smoothTime));
    }


    // Function to trigger the camera shake with a specified duration and intensity
    public void ShakeCamera(float duration, float intensity)
    {
        if (!isShakingCamera)
        {
            StartCoroutine(Shake(duration, intensity));
        }
    }


    // Coroutine to handle the camera shaking
    private IEnumerator Shake(float duration, float intensity)
    {
        isShakingCamera = true;

        Vector3 originalPosition = playerCamera.transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float reducingMultiplier = 1f - (elapsed / duration);
            float x = Random.Range(-1f, 1f) * intensity * reducingMultiplier;
            float y = Random.Range(-1f, 1f) * intensity * reducingMultiplier;

            playerCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        isShakingCamera = false;
        
        // Interpolation speed
        float lerpSpeed = 1f;
        float lerpElapsed = 0f;

        while (lerpElapsed < 1f)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, currentDesiredPosition, lerpElapsed);

            lerpElapsed += Time.deltaTime * lerpSpeed;

            yield return null;
        }

        // Ensure the camera is exactly at its original position
        playerCamera.transform.localPosition = currentDesiredPosition;
    }


    // Function to simulate camera look-up (recoil)
    public void CameraRecoil(float duration, float intensity)
    {
        if (!isRecoiling)
        {
            StartCoroutine(Recoil(duration, intensity));
        }
    }

    // Coroutine to handle the camera recoil
    private IEnumerator Recoil(float duration, float intensity)
    {
        isRecoiling = true;
        light.SetActive(true);

        Quaternion originalRotation = playerCamera.transform.localRotation;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float pitch = intensity * Mathf.Sin(Mathf.PI * elapsed / duration);
            playerCamera.transform.localRotation = originalRotation * Quaternion.Euler(-pitch, 0f, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        playerCamera.transform.localRotation = currentDesiredRotation;
        isRecoiling = false;
    }
}
