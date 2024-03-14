using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject prefabBullet;
    [SerializeField] Vector2 cooldown = new Vector2(0.1f, 1f);
    [SerializeField] float bulletSpeed, attackDelay;
    [SerializeField] bool isReady = true;
    [SerializeField] Animator animator;
    SoundManager soundManager;
    Coroutine attackCoroutine;
    private void Start()
    {
        StartCoroutine(AttackRepeatly());
        soundManager = SoundManager.Instance;
    }

    public void Attack()
    {
        soundManager.PlayAttack();
        GameObject bullet = Instantiate(prefabBullet, attackPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * Vector2.down;
    }
    IEnumerator AttackRepeatly()
    {
        while (true)
        {
            yield return null;
            if (isReady && animator.GetBool(AnimationStrings.isReady))
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
        isReady = false;
        yield return new WaitForSeconds(Random.Range(cooldown.x, cooldown.y));
        isReady = true;
    }
}
