using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    private Animator animator;
    public int ammo;
    private int fullAmmo;
    public TextMeshProUGUI ammoText;
    public AudioSource emptySFX;

    public float speedADS = 1.0f;
    private Vector3 startPos;
    private Vector3 endPos;

    private Quaternion startRot;
    private Quaternion endRot;

    private float lerpTime = 0;
    private bool isADS = false;
    bool freeToADS = true;

    public float reloadSpeed = 1f;
    public float fireSpeed = 1f;
    public Camera playerCamera;
    private GameObject crossHair;

    private bool isReloadingShotgun;

    // 0 = Single Fire (pistol) 1 = Auto Fire (rifle) 2 = Shotgun 3 = Melee
    public int localWeaponType = 0;

    // Start is called before the first frame update
    void Start()
    {
        fullAmmo = ammo;
        animator = transform.GetChild(0).GetComponent<Animator>();
        startRot = transform.localRotation; 
        startPos = transform.localPosition;

        switch (transform.name)
        {
            case "PM40":
                endPos = new Vector3(-0.477f,  0.133f,  -0.276f);
                endRot = Quaternion.Euler(-1.583f, 0.04f, -0.246f);
                break;
            case "Deagle":
                endPos = new Vector3(-0.467f, 0.089f, -0.275f);
                endRot = Quaternion.Euler(-2.922f, 0.046f, -0.246f);
                break;
            case "AK47" :
                endPos = new Vector3(0.066f, -0.217f, 1.307f);
                endRot = Quaternion.Euler(-4.02f, 2.301f, -0.161f);
                break;
            case "R870" :
                endPos = new Vector3( -0.5687557f, 0.1967292f, -0.2272279f);
                endRot = Quaternion.Euler(0.38f, 1.76f, -1.783f);
                break;
        } 
        if(GameObject.Find("Canvas/Crosshair") != null)
            crossHair = GameObject.Find("Canvas/Crosshair");
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = localWeaponType != 3 ? ammo.ToString() : "--";

        if (ammo == 0)
        {
            animator.SetInteger("FreeToIdle", 0);
            animator.SetInteger("Empty", 1);
        }
        else
        {
            animator.SetInteger("FreeToIdle", 1);
            animator.SetInteger("Empty", 0);
        }

        switch (localWeaponType)
        {
            case 0:
            case 2:
            {
                if (Input.GetMouseButtonDown((int)MouseButton.Left))
                {
                    if (ammo > 0)
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Reload") ||
                            animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "ReloadFull"))
                        {
                            isADS = false;
                            return;
                        }
                        // If the shoot animation isn't already playing
                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Shoot"))
                        {
                            Debug.Log("Shot");
                            // Play the shoot animation
                            animator.Play(transform.name + "Shoot");
                            ammo -= 1;
                        }
                    }
                    else
                    {
                        if(!emptySFX.isPlaying)
                            emptySFX.Play();
                    }
                }

                break;
            }
            case 1:
            {
                if (Input.GetMouseButton((int)MouseButton.Left))
                {
                    if (ammo > 0)
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Reload") ||
                            animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "ReloadFull"))
                        {
                            isADS = false;
                            return;
                        }

                        // If the shoot animation isn't already playing
                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Shoot"))
                        {
                            Debug.Log("Shot");
                            // Play the shoot animation
                            animator.Play(transform.name + "Shoot");
                            ammo -= 1;
                        }
                    }
                    else
                    {
                        if (!emptySFX.isPlaying)
                            emptySFX.Play();
                    }
                }

                break;
            }
            case 3:
                if (Input.GetMouseButtonDown((int)MouseButton.Left))
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Slash"))
                        animator.Play(transform.name + "Slash");
                }
                break;
        } 

        if (Input.GetMouseButton((int)MouseButton.Right))
        {
            if (localWeaponType == 3 && Input.GetMouseButtonDown((int)MouseButton.Right))
            {
                if(!animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Stab"))
                    animator.Play(transform.name + "Stab");
            }
            else if (localWeaponType != 3)
            {
                crossHair.SetActive(false);
                lerpTime += Time.deltaTime * speedADS;
                if (freeToADS)
                    isADS = true;
            }
        }
        else if (isADS)
        {
            crossHair.SetActive(true);
            lerpTime -= Time.deltaTime * speedADS;
            if (lerpTime <= 0)
            {
                isADS = false;
            }
        }

        lerpTime = Mathf.Clamp(lerpTime, 0, 1);

        if (isADS)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, lerpTime);
            transform.localRotation = Quaternion.Lerp(startRot, endRot, lerpTime);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (localWeaponType == 2)
            {
                if (ammo < fullAmmo && ammo > 0)
                {
                    isReloadingShotgun = true;
                    animator.SetFloat("Reload", 1f);
                }
                else if (ammo < fullAmmo && ammo <= 0)
                {
                    animator.Play(transform.name + "ReloadFull");
                    ammo = fullAmmo;
                }
            }
            else
            {
                if (ammo < fullAmmo && ammo > 0)
                {

                    animator.Play(transform.name + "Reload");
                    ammo = fullAmmo;
                }
                else if (ammo < fullAmmo && ammo <= 0)
                {
                    animator.Play(transform.name + "ReloadFull");
                    ammo = fullAmmo;
                }
            }
        }

        if (isReloadingShotgun)
        {
            int amount = fullAmmo - ammo;
            Debug.Log("shtogun " +amount);
            if (amount == fullAmmo)
            {
                isReloadingShotgun = false;
                animator.SetFloat("Reload", 0f);
            } else if (animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Reload"))
            {
                StartCoroutine(cooldown());
                IEnumerator cooldown()
                {
                    yield return new WaitForSeconds(1);
                    ammo += 1;
                    amount += 1;
                }
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Reload") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "ReloadFull"))
        {
            animator.speed = reloadSpeed;
        } else if (animator.GetCurrentAnimatorStateInfo(0).IsName(transform.name + "Shoot"))
        {
            animator.speed = fireSpeed;
        }
        else
        {
            animator.speed = 1f;
        }
    }
}
