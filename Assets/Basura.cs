using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basura : MonoBehaviour
{
    public int contador = 0;
    public int contador_ratas = 0;

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.tag == "Basura")
        {
            contador++;
        }

        if (col.collider.gameObject.tag == "Rata")
        {
            contador_ratas++;
        }
    }
}
