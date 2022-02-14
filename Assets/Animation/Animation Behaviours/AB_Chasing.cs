using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Chasing : StateMachineBehaviour
{
    private BotAI ai;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<BotAI>();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai.HasTarget() == true)
        {
            //ai.StartCoroutine(ai.RataCogida());
            ai.agent.enabled = false;
            ai.anim_rata.SetBool("Walk", true);
            return;
        }
        else
        {
            ai.agent.enabled = true;
            ai.FindCover();
            return;
        }

        if (ai.DetectPlayer() == false)
        {
            animator.SetBool("IsChasing", false);
            return;
        }
        animator.SetFloat("Speed", ai.agent.velocity.sqrMagnitude);
        //Si la distancia es menor que el rango de ataque, pasamos al estado de Attacking
        if (ai.GetTargetDirection().sqrMagnitude < ai.attackRange * ai.attackRange)
        {
            animator.SetBool("IsChasing", false);
            ai.FindCover();
            return;
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
