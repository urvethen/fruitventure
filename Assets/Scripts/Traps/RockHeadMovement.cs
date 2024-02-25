using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class RockHeadMovement: MonoBehaviour
{
    [SerializeField] MovementType moveType;
    [SerializeField] MovementDirect currentDirect;
    [SerializeField] float speed;
    [SerializeField] Vector2 blinkMinMaxDelay = Vector2.zero;
    [SerializeField] LayerMask groundLayer;
    bool isRight = true;
    bool isReady = true;
    //[SerializeField] Transform gfx;
    [SerializeField] Transform hitZone;
    bool stop;
    TouchingDirections wallDetection;
    Animator animator;
    Rigidbody2D rb;
    Queue<MovementDirect> movements;
    List<MovementDirect> moveDirectRight = new List<MovementDirect>() { MovementDirect.Left, MovementDirect.Top, MovementDirect.Right, MovementDirect.Bottom };
    List<MovementDirect> moveDirectLeft = new List<MovementDirect>() { MovementDirect.Left, MovementDirect.Bottom, MovementDirect.Right, MovementDirect.Top };
    Coroutine checkingWall;
    UIManager uiManager;
    private void Awake()
    {
        if(!TryGetComponent<Animator>(out animator))
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        wallDetection = GetComponent<TouchingDirections>();

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
        StartCoroutine(Blink());
        uiManager = UIManager.Instance;
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(blinkMinMaxDelay.x, blinkMinMaxDelay.y));
            animator.SetTrigger(AnimationStrings.start);
        }
        

    }
    private void CheckWall()
    {
        switch (currentDirect)
        {
            case MovementDirect.Right:
            case MovementDirect.Left:
                if (wallDetection.IsOnWall)
                {
                    Stop = true;
                    if (isReady)
                        animator.SetTrigger(AnimationStrings.hit_right);
                }
                break;
            case MovementDirect.Top:
                if (wallDetection.IsOnCeiling)
                {
                    Stop = true;
                    animator.SetTrigger(AnimationStrings.hit_top);
                }
                break;
            case MovementDirect.Bottom:
                if (wallDetection.IsGrounded)
                {
                    Stop = true;
                    animator.SetTrigger(AnimationStrings.hit_bot);
                }
                break;
        }
    }
    IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);
        IsReady = true;
        while (true)
        {
            if (checkingWall == null)
            {
                checkingWall = StartCoroutine(WathDog());
            }
            //rb.position = rb.position + speed * GetDirect()*Time.fixedDeltaTime;
            rb.velocity = speed * GetDirect();
            //transform.position = Vector3.MoveTowards(transform.position, transform.position + GetDirect(), speed * Time.fixedDeltaTime);
            if (stop)
            {
               // rb.velocity = Vector2.zero;\
                
                break;
            }
            yield return null;
        }
        StopCoroutine(checkingWall);
        checkingWall = null;
        SwapDirection();
    }
    IEnumerator WathDog()
    {
        yield return new WaitForSeconds(0.15f);
        while (true)
        {
            CheckWall();
            
            yield return null;
        }
     
    }
    private void SwapDirection()
    {
        Stop = false;
        switch (moveType)
        {
            case MovementType.Horizontal:

                if (currentDirect == MovementDirect.Left)
                {
                    CurrentDirect = MovementDirect.Right;
                }
                else
                {
                    CurrentDirect = MovementDirect.Left;
                }

                break;
            case MovementType.Vertical:

                if (currentDirect == MovementDirect.Top)
                {
                    CurrentDirect = MovementDirect.Bottom;
                }
                else
                {
                    CurrentDirect = MovementDirect.Top;
                }
                break;
            case MovementType.CircleRight:
                for (int i = 0; i < 4; i++)
                {
                    if (currentDirect == moveDirectRight[i])
                    {
                        if (i < 3)
                        {
                            CurrentDirect = moveDirectRight[i + 1];
                            break;
                        }
                        else
                        {
                            CurrentDirect = moveDirectRight[0];
                            break;
                        }
                    }
                }
                break;
            case MovementType.CircleLeft:
                for (int i = 0; i < 4; i++)
                {
                    if (currentDirect == moveDirectLeft[i])
                    {
                        print(i);
                        if (i < 3)
                        {
                            CurrentDirect = moveDirectLeft[i + 1];
                            break;
                        }
                        else
                        {
                            CurrentDirect = moveDirectLeft[0];
                            break;
                        }
                    }
                }
                break;
               
        }
        
        StartCoroutine(Move());
       

    }
    private Vector2 GetDirect()
    {
        switch (currentDirect)
        {
            case MovementDirect.Bottom:
                return Vector2.down;
            case MovementDirect.Left:
                return Vector2.left;
            case MovementDirect.Right:
                return Vector2.right;
            case MovementDirect.Top:
                return Vector2.up;
        }
        return Vector2.zero;
    }

    public MovementType MoveType { get { return moveType; } }
    public MovementDirect CurrentDirect
    {
        get { return currentDirect; }
        set
        {
            currentDirect = value;
            
            switch (currentDirect)
            {
                case MovementDirect.Bottom:
                    if (hitZone != null)
                    {
                        hitZone.localScale = Vector3.one;
                        hitZone.rotation = Quaternion.Euler(0f, 0f, 270f);
                    }                    
                    break;
                case MovementDirect.Left:
                    if (hitZone != null)
                    hitZone.rotation = Quaternion.Euler(0f, 0f, 0f);
                    IsRight = false;
                    break;
                case MovementDirect.Right:
                    if (hitZone != null)
                    hitZone.rotation = Quaternion.Euler(0f, 0f, 0f);
                    IsRight = true;
                    break;
                case MovementDirect.Top:
                    if (hitZone != null)
                    {
                        hitZone.localScale = Vector3.one;
                        hitZone.rotation = Quaternion.Euler(0f, 0f, 90f);
                    }
                    
                    break;
            }
        }
    }
    public bool IsRight
    {
        get { return isRight; }
        set
        {
            if (isRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            isRight = value;
        }
    }
    public bool IsReady
    {
        get { return animator.GetBool(AnimationStrings.isReady); }
        set
        {
            animator.SetBool(AnimationStrings.isReady, value);
        }
    }
    public bool Stop
    {
        get { return stop; }
        set
        {
            stop = value;
            if (value)
            {
               uiManager.ShakeCamera();
            }


        }
    }
    public enum MovementType
    {
        Horizontal,
        Vertical,
        CircleRight,
        CircleLeft,
    }
    public enum MovementDirect
    {
        Left,
        Top,
        Right,
        Bottom
    }
}
