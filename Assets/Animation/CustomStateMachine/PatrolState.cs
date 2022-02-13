using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private SM_Enemy enemy;
    public PatrolState(SM_Enemy _enemy)
    {
        displayName = "PATROL";
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        FindRandomPos();
    }

    public override void Update()
    {
        base.Update();
        if ((enemy.Agent.destination - enemy.transform.position).sqrMagnitude < .01f)
        {
            enemy.sm.ChangeState(new IdleState(enemy));
            return;
        }
    }

    void FindRandomPos()
    {
        Vector3 _randomDir = new Vector3(
            Random.Range(-1, 2),
            0f,
            Random.Range(-1, 2)
        ) * enemy.patrolRange;
        if (NavMesh.SamplePosition(_randomDir, out NavMeshHit _hit, 2f, NavMesh.AllAreas))
        {
            enemy.Agent.SetDestination(_hit.position);
        }
        else
        {
            FindRandomPos();
        }
    }
}
