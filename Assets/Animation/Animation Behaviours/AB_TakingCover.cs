using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_TakingCover : StateMachineBehaviour
{
    private BotAI ai;
    private float timer = 0f;
    private Vector3 initialPosition;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<BotAI>();
        }
        initialPosition = animator.transform.position;
        ai.FindWaypoint();
        timer = ai.timeCovering;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Speed", ai.agent.velocity.sqrMagnitude);
        if (ai.GetDestinationDirection().sqrMagnitude < .01f)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                ai.anim_rata.SetBool("Walk", false);
            }
            if (timer <= 0)
            {
                timer = ai.timeToPatrol;
                //ai.FindWaypoint();
                //ai.agent.SetDestination(initialPosition);
                animator.SetBool("IsTakingCover", false);
                
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

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
