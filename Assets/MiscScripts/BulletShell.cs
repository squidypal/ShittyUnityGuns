using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour
{
    private bool Played = false;
    private bool Ready = false;

    private void Start()
    {
        StartCoroutine(ready());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Ready) return;
        if (!transform.name.Contains("Clone")) return;
        if (!Played)
        {
            transform.GetComponent<AudioSource>().Play();
            Played = true;
        }
    }

    IEnumerator ready()
    {
        yield return new WaitForSeconds(0.5f);
        Ready = true;
    }
}
