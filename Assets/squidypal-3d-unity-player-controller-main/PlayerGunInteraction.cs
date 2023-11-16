using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunInteraction : MonoBehaviour
{
    private bool isHoldingGun = false;
    private GameObject gun;
    public GameObject gunStorage;
    public TextMeshProUGUI gunName;
    private GameObject recentFiredBullet;
    private static GameObject hitDecal;

    private static Transform playerCamera;
    private bool runBulletDropWait;
    private bool isCouritineRunning;

    private int currentGunIndex = 0;
    
    public bool NoArms = false;

    private static Quaternion originalRotation;
    private static float currentRecoil = 0; // The current recoil amount

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = transform.GetChild(0);
        originalRotation = playerCamera.localRotation;
        hitDecal = GameObject.Find("HitDecal");
    }

    // Update is called once per frame
    void Update()
    {
        if (NoArms)
        {
            // Foreach gameobject.find "arms" that contains a mesh renderer, disable it
            foreach (GameObject arms in GameObject.FindGameObjectsWithTag("arms"))
            {
                arms.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject arms in GameObject.FindGameObjectsWithTag("arms"))
            {
                arms.GetComponent<SkinnedMeshRenderer>().enabled = true;
            }
        }
        for(var i = 0; i < gunStorage.transform.childCount; i++)
        {
            if (gunStorage.transform.GetChild(i).gameObject.activeSelf)
            {
                gun = gunStorage.transform.GetChild(i).gameObject;
                isHoldingGun = true;
                gunName.text = gun.name;
            }
            else
            {
                isHoldingGun = false;
                gunName.name = "";
            }
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0f)
        {
            SwitchToNextGun();
        }
        else if (scrollInput < 0f)
        {
            SwitchToPreviousGun();
        }
    
        // Number key input to switch guns
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToGunAtIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToGunAtIndex(1);
        }
    }

    void SwitchToNextGun()
    {
        int nextGunIndex = (currentGunIndex + 1) % gunStorage.transform.childCount;
        SwitchToGunAtIndex(nextGunIndex);
    }

    void SwitchToPreviousGun()
    {
        int previousGunIndex = (currentGunIndex - 1 + gunStorage.transform.childCount) % gunStorage.transform.childCount;
        SwitchToGunAtIndex(previousGunIndex);
    }

    void SwitchToGunAtIndex(int index)
    {
        // Deactivate the currently active gun
        gun.SetActive(false);
    
        // Activate the gun at the specified index
        GameObject selectedGun = gunStorage.transform.GetChild(index).gameObject;
        selectedGun.SetActive(true);
    
        // Update the current gun index
        currentGunIndex = index;
    }

    public static void FireGun(GameObject firepoint, float recoilIntensity, float recoilDuration, float raycastDistance)
    {
        Camera playerCam = playerCamera.GetChild(0).GetComponent<Camera>();
        GameObject cameraShake = playerCamera.gameObject;
        Ray ray = playerCam.ScreenPointToRay(new Vector3(playerCam.pixelWidth / 2, playerCam.pixelHeight / 2, 0));
        CameraShaker cameraShaker = cameraShake.GetComponent<CameraShaker>();
        //TODO: Allow customization
        cameraShaker.CameraRecoil(recoilDuration, recoilIntensity);
        cameraShaker.ShakeCamera(recoilDuration / 2f, recoilIntensity / 50);
        Debug.Log("Fired Gun with " + recoilIntensity /2 + "shake and " + recoilIntensity + "recoil");
        // Adding Raycast
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, raycastDistance) && hitInfo.transform.gameObject.layer != 7)
        {
            Vector3 hitPosition = hitInfo.point; // Get the hit position
            Debug.Log("Hit " + hitInfo.transform.gameObject.name + " at " + hitPosition);

            // Instantiate the hit decal
            GameObject decal = Instantiate(hitDecal, hitPosition, Quaternion.identity);

            // Calculate the rotation to align the decal with the surface normal
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            decal.transform.rotation = rotation;
            if (!hitInfo.transform.GetComponent<Rigidbody>()) return;
            hitInfo.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 300);
            hitInfo.transform.GetComponent<Rigidbody>().AddForce(firepoint.transform.forward * 300);
        }
    }
}
