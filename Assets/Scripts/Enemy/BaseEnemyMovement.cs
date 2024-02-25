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
    Coroutine waitingCoroutine;
    [SerializeField] bool isWaiting = false;
    [SerializeField] DetectionZone cliffDetection;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((touchingDirections.IsOnWall || cliffDetection.colliders.Count == 0) && waitingCoroutine == null)
        {
            waitingCoroutine = StartCoroutine(WaitingToFlip(0.75f));
        }
        if (!isWaiting && !animator.GetBool(AnimationStrings.hasTarget) && animator.GetBool(AnimationStrings.canMove))
        {
            rb.velocity = new Vector2(speed * moveDirectionVector.x, rb.velocity.y);
        }
        else
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
                transform.localScale *= new Vector2(-1, 1);
                if (value == MovableDirection.Right)
                {
                    moveDirectionVector = Vector2.right;
                }
                else
                {
                    moveDirectionVector = Vector2.left;
                }
            }
            moveDirection = value;
        }
    }
    public bool IsMoving
    {
        get { return animator.GetBool(AnimationStrings.isMoving); }
        set
        {
            if (IsMoving != value)
                animator.SetBool(AnimationStrings.isMoving, value);
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
    public enum MovableDirection
    {
        Right,
        Left
    }
}
