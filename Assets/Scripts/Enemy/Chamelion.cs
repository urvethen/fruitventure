using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chamelion : BaseEnemyLogic
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float attackDisctance = 1f;
    [SerializeField] float speed;
    [SerializeField] DetectionZone cliffDetection;
    [SerializeField] float attackDelay;
    [SerializeField] float attackCooldown;
    [SerializeField] float soundStepCD = 0.2f;
    bool attackReady = true;
    bool needSound = false;
    SoundManager soundManager;
    private void Start()
    {
        soundManager = SoundManager.Instance;
    }
    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            if (playerTransform.position.x > transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = Vector2.one;
            }
            if (Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDisctance)
            {                
                if (cliffDetection.colliders.Count > 0 && animator.GetBool(AnimationStrings.canMove))
                {
                    rb.velocity = new Vector2(-speed * transform.localScale.x, rb.velocity.y);
                    animator.SetBool(AnimationStrings.isMoving, true);
                    NeedSound = true;

                }                
                animator.SetBool(AnimationStrings.hasTarget, false);
            }
            else
            {
                animator.SetBool(AnimationStrings.hasTarget, true);
            }            
        }
        else
        {
            animator.SetBool(AnimationStrings.isMoving, false);
            NeedSound = false;
        }
        if (animator.GetBool(AnimationStrings.hasTarget))
        {
            StartCoroutine(AttackRepeatly());
        }
        
    }
    public void Attack()
    {
        
    }
    IEnumerator AttackRepeatly()
    {
        while (animator.GetBool(AnimationStrings.hasTarget))
        {
            yield return null;
            if (playerTransform != null && attackReady && Mathf.Abs(playerTransform.position.y - transform.position.y) < 1.5f)
            {
                StartCoroutine(Cooldown());
                animator.SetTrigger(AnimationStrings.attack);
                //Invoke("Attack", attackDelay);
            }
        }
        
    }
    IEnumerator MovingSound()
    {
        while (animator.GetBool(AnimationStrings.isMoving) && animator.GetBool(AnimationStrings.canMove))
        {
            soundManager.PlayEnemyStep();

            yield return new WaitForSeconds(soundStepCD);
        }


    }
    IEnumerator Cooldown()
    {
        attackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }

    public void OnPlayerDetection (Collider2D collider)
    {
        playerTransform = collider.transform;
    }

    public void OnPlayerOut ()
    {
        playerTransform = null;
    }

    bool NeedSound
    {
        get { return needSound; } 
        set
        {
            if (needSound != value)
            {
                needSound = value;
                if (needSound)
                {
                    StartCoroutine(MovingSound());
                }
                else
                {
                    StopCoroutine(MovingSound() );
                }
            }
        }
    }
}
