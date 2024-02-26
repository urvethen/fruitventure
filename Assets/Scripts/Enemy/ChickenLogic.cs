using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenLogic: MonoBehaviour
{
    [SerializeField] Transform main;
    [SerializeField] Transform player;
    [SerializeField] DetectionZone playerDetection;
    [SerializeField] DetectionZone cliffDetection;
    [SerializeField] float roundedDisctance = 3f;
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] MovableDirection moveDirection;
    [SerializeField] Vector2 moveDirectionVector = Vector2.left;
    [SerializeField] TouchingDirections touchingDirections;
    [SerializeField] bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if  (main != null)
        {
            if (player!= null && ((touchingDirections.IsOnWall || cliffDetection.colliders.Count == 0) ||
            ((MoveDirection == MovableDirection.Right && (-player.position.x + main.position.x) > roundedDisctance) ||
            (MoveDirection == MovableDirection.Left && (player.position.x - main.position.x) > roundedDisctance))))
            {
                FlipDirection();
                //todo Условия поворота
            }
            if (isMoving && animator.GetBool(AnimationStrings.hasTarget) && animator.GetBool(AnimationStrings.canMove))
            {
                //todo Условия движения
                rb.velocity = new Vector2(speed * moveDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
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
    public void OnPlayerEnterInDetectionZone(Collider2D collision)
    {
        if (main != null)
        {
            if (collision.transform.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            {
                if (playerMovement != null)
                {
                    player = playerMovement.transform;
                    ChooseDirection();
                    IsMoving = true;
                    animator.SetBool(AnimationStrings.hasTarget, true);
                    //todo Запускаем бег
                }
            }
        }
        
    }

    private void ChooseDirection()
    {
        if (player.position.x > main.position.x)
        {
            MoveDirection = MovableDirection.Right;
        }
        else
        {
            MoveDirection = MovableDirection.Left;
        }
    }
    public void OnPlayerExitDetectionZone()
    {
        if (main != null)
        {
            player = null;
            animator.SetBool(AnimationStrings.hasTarget, false);
            IsMoving = false;
            //todo Останавливаем бег
        }

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
        get { return isMoving; }
        set
        {
            if (isMoving != value)
            {
                animator.SetBool(AnimationStrings.isMoving, value);
                isMoving = value;
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
    public enum MovableDirection
    {
        Right,
        Left
    }
}
