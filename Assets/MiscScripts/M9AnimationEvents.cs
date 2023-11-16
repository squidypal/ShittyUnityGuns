using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M9AnimationEvents : MonoBehaviour
{
         public ParticleSystem muzzleFlashEffect;
          public GameObject shell;
          public GameObject magazine;
          public GameObject firepoint;
          public GameObject gunshotSoundGO;
       
          public AudioSource cock;
          public AudioSource magIn;
          public AudioSource magOut;

          public float recoilIntensity = 4f;
          public float recoildDuration = 0.2f;
          public float raycastDistance = 100;

          public AudioSource takeout;

          void Takeout()
          {
             takeout.Play();
          }
          
          void FireGun()
          {
             PlayerGunInteraction.FireGun(firepoint, recoilIntensity, recoildDuration, raycastDistance);
             /*muzzleFlashEffect.Play();*/
             GameObject gameObject = Instantiate(gunshotSoundGO);
             gameObject.GetComponent<AudioSource>().Play();
          }
       
          void EmptyShell()
          {
             GameObject newShell = Instantiate(shell, shell.transform.position, shell.transform.rotation);
             newShell.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
             newShell.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
             newShell.GetComponent<BoxCollider>().enabled = true;
             newShell.GetComponent<Rigidbody>().AddForce(transform.right * 50);
             newShell.GetComponent<Rigidbody>().AddForce(transform.up * 50);
             newShell.layer = 0;
          }
       
          void EmptyMag()
          {
             GameObject emptyMag = Instantiate(magazine, magazine.transform.position, magazine.transform.rotation);
             emptyMag.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
             emptyMag.GetComponent<BoxCollider>().enabled = true;
             emptyMag.transform.localScale = new Vector3(2f, 2f, 2f);
             emptyMag.layer = 0;
          }
    }
