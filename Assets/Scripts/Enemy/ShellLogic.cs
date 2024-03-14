using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellLogic : BaseEnemyLogic
{
    [SerializeField] bool firstContact = true;
    [SerializeField] BaseEnemyMovement movement;
    BaseEnemyLogic baseEnemyLogic;
    Coroutine waiting;
    AudioSource audioSource;
    public override void OnHitZoneEnter(Collider2D collision)
    {
        if (!firstContact)
        {
            animator.SetBool(AnimationStrings.canMove, false);
            base.OnHitZoneEnter(collision);
        }
        else
        {
            StartCoroutine(Wait());
            movement.enabled = true;
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            if (collision.gameObject.TryGetComponent<PlayerMovement>(out playerMovement))
            {
                playerMovement.NeedForceJump(1f);
            }
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        firstContact = false;
    }
    
    public void OnKillTriggerEnter(Collider2D collision)
    {
        if (isAlive && !firstContact)
        {
            if (collision.gameObject.TryGetComponent<BaseEnemyLogic>(out baseEnemyLogic))
            {
                baseEnemyLogic.DeathAction();
                animator.SetTrigger(AnimationStrings.hit_side);
            }
        }
    }
}
