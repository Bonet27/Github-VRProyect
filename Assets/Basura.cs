using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basura : MonoBehaviour
{
    public int contador = 0;

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.tag == "Basura")
        {
            contador++;
        }
    }
}
