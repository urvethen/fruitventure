using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLogic : MonoBehaviour
{
    [SerializeField] Animator animator;
    PlayerMovement playerMovement;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.transform.TryGetComponent(out playerMovement))
        {
            animator.SetBool(AnimationStrings.start, true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out playerMovement))
        {
            animator.SetBool(AnimationStrings.start, false);
        }
    }
}
