using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyInstantiateObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("starting");
        if (!transform.name.Contains("Clone")) return;
        Debug.Log("Destroying " + transform.name);
        StartCoroutine(WaitToDestroy());
    }

    private IEnumerator WaitToDestroy()
    {
        Debug.Log("waiting");
        yield return new WaitForSeconds(30);
        Destroy(gameObject);
        Debug.Log("destroyed");
    }
}
