using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class RinoMovement: MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float accTime;
    [SerializeField] float currentSpeed = 0f;
    [SerializeField] Vector2 forceVector;
    [SerializeField] float soundStepCD = 0.2f;
    bool lockVelocity = false;
    TouchingDirections touchingDirections;
    Rigidbody2D rb;
    SoundManager soundManager;
    Animator animator;
    bool isRight = false;
    Coroutine accelrationCoroutine, soundCoroutine;
    private void Awake()
    {
        touchingDirections = GetComponent<TouchingDirections>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        soundManager = SoundManager.Instance;
    }
    private void FixedUpdate()
    {
        if (HasTarget && touchingDirections.IsGrounded && !touchingDirections.IsOnWall && accelrationCoroutine == null)
        {
            accelrationCoroutine = StartCoroutine(Acceleration());
        }
        if (!lockVelocity && animator.GetBool(AnimationStrings.canMove))
        {
            rb.velocity = new Vector2(-transform.localScale.x * currentSpeed, rb.velocity.y);
            
        }
        else if (!animator.GetBool(AnimationStrings.canMove))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (touchingDirections.IsGrounded && Mathf.Abs(rb.velocity.x) > 0.2f)
        {
            animator.SetBool(AnimationStrings.isMoving, true);
        }
        else
        {
            animator.SetBool(AnimationStrings.isMoving, false);
        }
        if( soundCoroutine == null && animator.GetBool(AnimationStrings.needSound))
        {
            soundCoroutine = StartCoroutine(MovingSound());
        }
        else if (!animator.GetBool(AnimationStrings.needSound) && soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine);
            soundCoroutine = null;
        }
    }
    IEnumerator Acceleration()
    {
        float elapsedTime = 0f;
        //StartCoroutine(MovingSound());
        while (!touchingDirections.IsOnWall)
        {
            currentSpeed += elapsedTime * maxSpeed / accTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //StopCoroutine(MovingSound());
        soundManager.PlayStoneImpact();
        currentSpeed = 0f;
        lockVelocity = true;
        StartCoroutine(LockingVelocity());
        animator.SetTrigger(AnimationStrings.hit_side);
        //Толчок назад
        rb.AddForce(new Vector2(transform.localScale.x * forceVector.x, forceVector.y), ForceMode2D.Impulse);
        accelrationCoroutine = null;
    }
    IEnumerator LockingVelocity()
    {
        lockVelocity = true;
        yield return new WaitForSeconds(1.5f);
        while (!touchingDirections.IsGrounded)
        {
            yield return null;
        }
        lockVelocity = false;
        transform.localScale *= new Vector2(-1, 1);
    }
    IEnumerator MovingSound()
    {
        while (true)
        {
            soundManager.PlayEnemyStep();

            yield return new WaitForSeconds(soundStepCD);
        }


    }
    bool HasTarget
    {
        get { return animator.GetBool(AnimationStrings.hasTarget); }
    }
}
