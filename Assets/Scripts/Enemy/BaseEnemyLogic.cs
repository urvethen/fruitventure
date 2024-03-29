using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyLogic: MonoBehaviour
{
    [SerializeField] protected DetectionZone hitZone;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Collider2D baseCollider2D;
    protected bool isAlive = true;
    protected PlayerMovement playerMovement;
    public virtual void OnHitZoneEnter(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out playerMovement))
        {
            playerMovement.NeedForceJump(1f);
            isAlive = false;
            DeathAction();
        }
    }
    public virtual void DeathAction()
    {
        SoundManager.Instance.PlayDeath();
        rb.bodyType = RigidbodyType2D.Kinematic;
        baseCollider2D.enabled = false;
        rb.velocity = Vector3.zero;
        animator.SetTrigger(AnimationStrings.hit);
        Destroy(gameObject, 0.4f);
        UIManager.Instance.ShakeCamera();
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive)
        {
            if (collision.gameObject.TryGetComponent<PlayerMovement>(out playerMovement))
            {
                playerMovement.IsAlive = false;
            }
        }

    }
}
