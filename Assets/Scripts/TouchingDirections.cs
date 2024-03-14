using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.UI.Image;

public class TouchingDirections: MonoBehaviour
{
    public ContactFilter2D groundFilter, platformFilter;
    public ContactFilter2D ice, sand, swamp;
    public float groundDisctance = 0.03f, wallDistance = 0.02f;
    [SerializeField] bool isGrounded = true;
    [SerializeField] bool isOnWall, isOnCeiling;
    Collider2D collide;
    bool needDeepCheck;
    [SerializeField] bool onIce, onSand, onSwamp;
    [SerializeField] Animator animator;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilHits = new RaycastHit2D[5];
    RaycastHit2D[] iceHits = new RaycastHit2D[5];
    RaycastHit2D[] sandHits = new RaycastHit2D[5];
    RaycastHit2D[] swampHits = new RaycastHit2D[5];
    void Start()
    {
        collide = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (CompareTag("Player"))
        {
            needDeepCheck = true;
        }
    }
    private Vector2 WallCheckDirection
    {
        get
        {
            if (CompareTag("Enemy"))
            {
                if(transform.localScale.x> 0)
                {
                    return Vector2.left;
                }
                else
                {
                    return Vector2.right;
                }
            }
            else
            {
                if (transform.localScale.x > 0)
                {
                    return Vector2.right;
                }
                else
                {
                    return Vector2.left;
                }
            }
        }
    }
    void FixedUpdate()
    {
        //IsGrounded = capsuleCollider.Cast(Vector2.down, groundFilter, groundHits, groundDisctance) > 0;
        if (collide.Cast(Vector2.down, groundFilter, groundHits, groundDisctance) > 0 ||
             (collide.Cast(Vector2.down, platformFilter, groundHits, groundDisctance) > 0 && Mathf.Abs(animator.GetFloat(AnimationStrings.yVelocity)) < 0.02f))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
        IsOnWall = collide.Cast(WallCheckDirection, groundFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = collide.Cast(Vector2.up, groundFilter, ceilHits, groundDisctance) > 0;
       // Debug.DrawLine(transform.position, transform.position + transform.forward * wallDistance, Color.red);
        if (needDeepCheck)
        {

            onIce = collide.Cast(Vector2.down, ice, iceHits, groundDisctance) > 0;
            onSand = collide.Cast(Vector2.down, sand, sandHits, groundDisctance) > 0;
            onSwamp = collide.Cast(Vector2.down, swamp, swampHits, groundDisctance) > 0;

            if (IsOnWall)
            {
                onSwamp = collide.Cast(WallCheckDirection, swamp, swampHits, wallDistance) > 0;
            }

        }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }
        private set
        {
            isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }
    public bool IsOnWall
    {
        get { return isOnWall; }
        private set
        {
            isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }
    public bool IsOnCeiling
    {
        get { return isOnCeiling; }
        private set
        {
            isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }
    public bool OnIce
    {
        get { return onIce; }
    }
    public bool OnSand
    {
        get { return onSand; }
    }
    public bool OnSwamp
    {
        get { return onSwamp; }
    }
}
