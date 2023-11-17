using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM40AnimationEvents : MonoBehaviour
{
    public GameObject shell;
    public GameObject magazine;
    public GameObject bullet;
    public GameObject firepoint;
    public GameObject gunshotSoundGO;
    public AudioSource cock;
    public AudioSource magIn;
    public AudioSource magOut;
    public AudioSource altCock;
    
    void Cock()
    {
        cock.Play();
    }

    void AltCock()
    {
     altCock.Play();   
    }

    void MagOut()
    {
        magOut.Play();
    }

    void MagIn()
    {
        magIn.Play();
    }
    
    void FireGun()
    {
        PlayerGunInteraction.FireGun(firepoint, 3, 0.2f, 100);
        GameObject gameObject = Instantiate(gunshotSoundGO);
        gameObject.GetComponent<AudioSource>().Play();
    }
    
    void EmptyShell()
    {
        GameObject newShell = Instantiate(shell, shell.transform.position, shell.transform.rotation);
        newShell.transform.localScale = new Vector3(2.5f, 1f, 2.5f);
        newShell.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        // Add force to the left
        newShell.GetComponent<Rigidbody>().AddForce(transform.right * -100);
        newShell.GetComponent<Rigidbody>().AddForce(transform.up * 200);
        newShell.GetComponent<Rigidbody>().AddTorque(transform.right / 5);
        newShell.GetComponent<BoxCollider>().enabled = true;
        newShell.layer = 0;
    }

    void EmptyMag()
    {
        GameObject emptyMag = Instantiate(magazine, magazine.transform.position, magazine.transform.rotation);
        emptyMag.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        emptyMag.GetComponent<BoxCollider>().enabled = true;
        emptyMag.transform.localScale = new Vector3(-0.0025f, -0.0025f, -0.0025f);
        emptyMag.layer = 0;
    }
}
