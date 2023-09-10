using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToAnimator : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ArrowKeyMovement.instance.restrictMovement && !ArrowKeyMovement.instance.invincible)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.ResetTrigger("Attack");
        }
        if (!GameControl.instance.gameOver)
        {
            if (ArrowKeyMovement.instance.restrictMovement)
            {
                animator.speed = 0.0f;
            }
            else
            {
                animator.SetFloat("horizontal_input", ArrowKeyMovement.instance.lhorz_inp);
                animator.SetFloat("vertical_input", ArrowKeyMovement.instance.lvert_inp);
                if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                {
                    animator.speed = 0.0f;
                }
                else
                {
                    animator.speed = 1.0f;
                }
            }
        }
    }
}
