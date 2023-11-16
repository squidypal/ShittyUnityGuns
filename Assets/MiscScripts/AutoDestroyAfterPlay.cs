using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyAfterPlay : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.name.Contains("Clone"))
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                Destroy(gameObject);
            }
        }       
    }
}
