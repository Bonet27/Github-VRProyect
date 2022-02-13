using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private float timeToAct = 2f;

    private float timer = 0f;

    private SM_Enemy enemy;

    public IdleState(SM_Enemy _enemy)
    {
        displayName = "IDLE";
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        timer = timeToAct;
    }

    public override void Update()
    {
        base.Update();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0f)
        {
            enemy.sm.ChangeState(new PatrolState(enemy));
            return;
        }
    }
}
