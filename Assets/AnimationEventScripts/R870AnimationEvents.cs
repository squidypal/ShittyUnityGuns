using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R870AnimationEvents : MonoBehaviour
{
         public ParticleSystem muzzleFlashEffect;
          public GameObject shell;
          public GameObject firepoint;
          public GameObject gunshotSoundGO;
       
          public AudioSource pumpBack;
          public AudioSource pumpForward;
          public AudioSource magIn;
          public AudioSource magOut;

          public float recoilIntensity = 9f;
          public float recoildDuration = 0.3f;
          public AudioSource takeout;

          void Takeout()
          {
              takeout.Play();
          }

          void PumpBack()
          {
              pumpBack.Play();
          }

          void PumpForward()
          {
              pumpForward.Play();
          }
          
          void FireGun()
          {
             PlayerGunInteraction.FireGun(firepoint, recoilIntensity, recoildDuration, 100);
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
             newShell.GetComponent<Rigidbody>().AddForce(transform.right * 60);
             newShell.GetComponent<Rigidbody>().AddForce(transform.up * 80);
             newShell.layer = 0;
          }
}
