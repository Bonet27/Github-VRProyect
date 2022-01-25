using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Platform : MonoBehaviour
{
    public float speed = 1f;

    private Vector3 destination;

    private NavMeshAgent agent;

    void Start()
    {
        destination = transform.position + new Vector3(15, 0, 0);
    }

    IEnumerator CRT_Move()
    {
        Vector3 _direction = destination - transform.position;
        while (_direction.sqrMagnitude > .01f)
        {
            _direction = destination - transform.position;
            transform.position += _direction.normalized * speed * Time.deltaTime;
            // agent.Warp(transform.position + _direction.normalized * speed * Time.deltaTime);
            agent.destination += _direction.normalized * speed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Enemy"))
        {
            agent = _other.GetComponent<NavMeshAgent>();
            StartCoroutine(CRT_Move());
        }
    }
}
