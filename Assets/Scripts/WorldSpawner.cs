using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldSpawner : MonoBehaviour
{
    public GameObject worldPrefab;
    public Transform worldParent;
    private NavMeshSurface[] surfaces;

    private void Start() 
    {
        surfaces = worldParent.GetComponents<NavMeshSurface>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            Spawn();
        }
    }

    void Spawn()
    {
        Instantiate(worldPrefab, new Vector3(40,0,0), worldPrefab.transform.rotation, surfaces[0].transform);
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
