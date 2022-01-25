using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SM_Enemy : MonoBehaviour
{
    public float patrolRange = 3f;
    public float visionRange = 5f;
    public LayerMask targetLayer;
    public Transform target;
    public StateMachine sm;
    public NavMeshAgent Agent { get; private set; }

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        sm = new StateMachine();
        sm.ChangeState(new IdleState(this));
    }

    void Update()
    {
        sm.SMUpdate();
    }

    public void FindTarget()
    {
        Collider[] _targets = Physics.OverlapSphere(transform.position, visionRange, targetLayer);
        if (_targets.Length > 0)
        {
            target = _targets[0].transform;
        }
        else
        {
            target = null;
        }
    }


}
