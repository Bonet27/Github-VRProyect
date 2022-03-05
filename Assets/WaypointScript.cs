using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    public BotAI ai;

    public int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        ai.count++;
        Debug.Log("La rata llegó al waypoint");
    }

    private void OnTriggerStay(Collider other)
    {
        ai.count++;
        Debug.Log("La rata esta en el waypoint");
    }
}
