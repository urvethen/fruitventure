using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkLogic: BaseEnemyLogic
{
    [SerializeField] GameObject prefabBullet;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float attackCooldown, attackDelay = 0.21f;
    [SerializeField] bool attackReady;
    [SerializeField] Transform attackPoint;
    Coroutine attackCoroutine;
    public void OnAttack()
    {

        if (attackCoroutine == null)
        {
            StartCoroutine(AttackRepeatly());
        }

    }

    public void Attack()
    {
        Vector3 rotation = transform.localScale.x > 0 ? new Vector3(0, 0, 0) : new Vector3(0, 0, 180f);
        GameObject bullet = Instantiate(prefabBullet, attackPoint.position, Quaternion.Euler(rotation));
        bullet.GetComponent<Rigidbody2D>().velocity = transform.localScale.x > 0 ? bulletSpeed * Vector2.left : bulletSpeed *  Vector2.right;


    }
    IEnumerator AttackRepeatly()
    {
        while (animator.GetBool(AnimationStrings.hasTarget))
        {
            yield return null;
            if (attackReady)
            {
                StartCoroutine(Cooldown());
                animator.SetTrigger(AnimationStrings.attack);
                Invoke("Attack", attackDelay);
            }
        }
        attackCoroutine = null;
    }
    IEnumerator Cooldown()
    {
        attackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }
}
