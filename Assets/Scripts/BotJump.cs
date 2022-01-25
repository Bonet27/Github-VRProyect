using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotJump : MonoBehaviour
{
    public Animator animator;

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Land()
    {
        animator.SetTrigger("Land");
    }
}
