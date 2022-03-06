using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoTirable : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.tag == "Basura" || col.collider.gameObject.tag == "Rata")
        {
            Destroy(col.gameObject);
        }
    }
}
