using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Patrolling : StateMachineBehaviour
{
    private BotAI ai;
    private float timer = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<BotAI>();
        }
        //ai.FindRandomPoint();
        timer = ai.timeToPatrol;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai.HasTarget() == true || ai.DetectPlayer() == true)
        {
            //animator.SetBool("IsChasing", true);
            ai.FindCover();
            return;
        }
        animator.SetFloat("Speed", ai.agent.velocity.sqrMagnitude);
        if (ai.GetDestinationDirection().sqrMagnitude < .01f)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                timer = ai.timeToPatrol;
                ai.FindRandomPoint();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
