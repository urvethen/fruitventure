using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseEnemyMovement: MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] MovableDirection moveDirection;
    [SerializeField] Vector2 moveDirectionVector = Vector2.left;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Coroutine waitingCoroutine, moveSoundCoroutine;
    [SerializeField] bool isWaiting = false;
    [SerializeField] DetectionZone cliffDetection;
    [SerializeField] float waitTimer = 0.75f;
    [SerializeField] public bool lockVelocity = false;
    [SerializeField] float soundStepCD = 0.2f;
    SoundManager soundManager;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        bool flip = Random.Range(0f, 1f) > 0.5f ? true : false;
        if (flip)
        {
            FlipDirection();
        }
        soundManager = SoundManager.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (touchingDirections.IsGrounded)
        {
            if ((touchingDirections.IsOnWall || cliffDetection.colliders.Count == 0) && waitingCoroutine == null)
            {
                waitingCoroutine = StartCoroutine(WaitingToFlip(waitTimer));
            }
            if (!isWaiting && !animator.GetBool(AnimationStrings.hasTarget) && animator.GetBool(AnimationStrings.canMove))
            {
                rb.velocity = new Vector2(speed * moveDirectionVector.x, rb.velocity.y);
            }
            else if (!lockVelocity)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (Mathf.Abs(rb.velocity.x) > 0.15f)
            {
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }
        }
        

    }
    IEnumerator MovingSound()
    {
        while (IsMoving)
        {
            soundManager.PlayEnemyStep();
           
            yield return new WaitForSeconds(soundStepCD);
        }
        

    }
    private void FlipDirection()
    {
        if (MoveDirection == MovableDirection.Right)
        {
            MoveDirection = MovableDirection.Left;
        }
        else
        {
            MoveDirection = MovableDirection.Right;
        }
    }
    IEnumerator WaitingToFlip(float time)
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        isWaiting = false;
        FlipDirection();
        yield return new WaitForSeconds(0.25f);
        waitingCoroutine = null;

    }
    public MovableDirection MoveDirection
    {
        get { return moveDirection; }
        set
        {
            if (moveDirection != value)
            {
                
                if (value == MovableDirection.Right)
                {
                    moveDirectionVector = Vector2.right;
                    transform.localScale = new Vector2(-1, 1);
                }
                else
                {
                    moveDirectionVector = Vector2.left;
                    transform.localScale = new Vector2(1, 1);
                }
                moveDirection = value;
            }
            
        }
    }
    public bool IsMoving
    {
        get { return animator.GetBool(AnimationStrings.isMoving); }
        set
        {
            if (IsMoving != value)
            {
                animator.SetBool(AnimationStrings.isMoving, value);
                if (value && moveSoundCoroutine == null)
                {
                    moveSoundCoroutine = StartCoroutine(MovingSound());
                }
                else if (!value &&  moveSoundCoroutine != null)
                {
                    StopCoroutine(moveSoundCoroutine);
                    moveSoundCoroutine = null;
                }
            }
                
        }
    }
    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;
        }
    }
    public float WaitTimer
    {
        get { return waitTimer; }
        set
        {
            waitTimer = value;
        }
    }
    public float SoundSTEPCD
    {
        get { return soundStepCD; }
        set
        {
            if (soundStepCD != value)
            {
                if (moveSoundCoroutine != null)
                {
                    StopCoroutine(moveSoundCoroutine);
                    moveSoundCoroutine= null;
                }
                
                soundStepCD = value;
                if (moveSoundCoroutine == null)
                {
                    StartCoroutine(MovingSound());
                }
                
            }
        }
    }
    public enum MovableDirection
    {
        Right,
        Left
    }
}
