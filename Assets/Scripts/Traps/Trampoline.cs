using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] int koef;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (animator.GetBool(AnimationStrings.isReady) && collision.gameObject.layer == 13)
        {
            animator.SetTrigger(AnimationStrings.start);
            collision.gameObject.GetComponentInParent<PlayerMovement>().NeedForceJump(koef*transform.localScale.y);
            UIManager.Instance.ShakeCamera();
        }
        
    }
}
