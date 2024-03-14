using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : BaseEnemyLogic
{
    [SerializeField] bool isSecondPhase = false;
    [SerializeField] float secondPhaseTimer = 7f;
    [SerializeField] BaseEnemyMovement movement;
    Coroutine secondPhase;
    public override void OnHitZoneEnter(Collider2D collision)
    {
        if (IsSecondPhase)
        {
            base.OnHitZoneEnter(collision);
        }
        else
        {
            print("check");            
            if (collision.gameObject.TryGetComponent<PlayerMovement>(out playerMovement))
            {
                playerMovement.NeedForceJump(1f);
            }
            animator.SetTrigger(AnimationStrings.hit);
            if (secondPhase == null)
            {
                secondPhase = StartCoroutine(SecondPhaseTimer());
            }
        }
    }

   

    IEnumerator SecondPhaseTimer()
    {
        float elapsedTime = 0f;
        movement.Speed *= 3;
        movement.WaitTimer = 0f;
        movement.SoundSTEPCD /= 2;
        while (elapsedTime < secondPhaseTimer)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (animator.GetBool(AnimationStrings.isAlive))
        {
            IsSecondPhase = false;
            movement.Speed /= 3;
            movement.WaitTimer = 0.4f;
            movement.SoundSTEPCD *= 2;
        }

        secondPhase = null;
    }

    public bool IsSecondPhase
    {
        get { return animator.GetBool(AnimationStrings.isReady); }
        set
        {
            animator.SetBool(AnimationStrings.isReady, value);
        }
    }
}
