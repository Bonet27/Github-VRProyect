using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luces : MonoBehaviour
{
    public GameObject Light;

    private void Start()
    {
        Light.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!Light.activeSelf)
            {
                Light.SetActive(true);
            }
            else
            {
                Light.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!Light.activeSelf)
            {
                Light.SetActive(true);
            }
            else
            {
                Light.SetActive(false);
            }
        }
    }
}
