using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement: MonoBehaviour
{
    [SerializeField] Vector2 moveInput;
    [SerializeField] float moveSpeed, airSpeed;
    [SerializeField] float iceStep = 0.2f;
    [SerializeField] float minGravity, maxGravity;
    [SerializeField] bool isMoving;
    [SerializeField] bool lockVelocity;
    [SerializeField] bool isRight = true;
    [SerializeField] float jumpPower = 5;
    [SerializeField] float jumpFromWallTimer = 0.1f;
    [SerializeField] int jumpCounter = 0;
    [SerializeField] ParticleSystem runningParticle, jumpParticle;
    [SerializeField] Transform runningParticleTransform;
    [SerializeField]ParticleSystemRenderer runningParticleRenderer, jumpParticleRenderer;
    
    [SerializeField] List<Sprite> dustList;
    Rigidbody2D rb;
    [SerializeField]Transform gfxHolder;
    [SerializeField] List<Material> materialList = new List<Material>();
    Vector2 currentIceSpeed;
    Animator animator;
    TouchingDirections touchingDirections;
    GameManager gameManager;
    Coroutine lockingVelocityCoroutine, resetJumpCounterCoroutine, forceJumpCoroutine, runninCoroutine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        //gfxHolder = transform.GetChild(1);
    }
    void Start()
    {
        gameManager = GameManager.Instance;
    }


    void FixedUpdate()
    {
        if (IsSliding && touchingDirections.OnSwamp && !lockVelocity)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = 1f;
        }
        if (!lockVelocity && CanMove && IsAlive && !touchingDirections.OnIce)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
            currentIceSpeed = rb.velocity;            
        }
        else if (!lockVelocity && CanMove && IsAlive && touchingDirections.OnIce)
        {
            print(rb.velocity);
            if (rb.velocity.x == moveInput.x * CurrentSpeed)
            {
                rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
                currentIceSpeed = rb.velocity;
            }
            else if (moveInput.x != 0 && Mathf.Abs( currentIceSpeed.x) < CurrentSpeed)
            {
                float iceSpeed = currentIceSpeed.x + moveInput.x * iceStep;
                rb.velocity = new Vector2(iceSpeed, rb.velocity.y);
                currentIceSpeed = rb.velocity;
            }
            else
            {
                rb.velocity = new Vector2(currentIceSpeed.x, rb.velocity.y);
            }
        }
        if(Mathf.Abs(rb.velocity.x)>0.05f && touchingDirections.IsGrounded)
        {
            if(runninCoroutine == null)
            runninCoroutine = StartCoroutine(PlayRunningParticle());
        }
        else
        {
            if (runninCoroutine != null)
            {
                StopCoroutine(runninCoroutine);
                runninCoroutine = null;
            }            
        }
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        CheckGravity();
        if (touchingDirections.IsGrounded && (touchingDirections.OnSwamp || touchingDirections.OnSand))
        {
            gfxHolder.transform.localPosition = new Vector3(0, -0.2f, 0);
            

        }
        else if (touchingDirections.IsOnWall && touchingDirections.OnSwamp)
        {
            gfxHolder.transform.localPosition = new Vector3(0.25f,0, 0);            
        }
        else
        {
            gfxHolder.transform.localPosition = Vector3.zero;            
        }
        if (touchingDirections.OnSand)
        {
            runningParticleRenderer.material = materialList[1];
            jumpParticleRenderer.material = materialList[1];
        }
        else if (touchingDirections.OnSwamp)
        {
            runningParticleRenderer.material = materialList[2];
            jumpParticleRenderer.material = materialList[2];
        }
        else if (touchingDirections.OnIce)
        {
            runningParticleRenderer.material = materialList[3];
            jumpParticleRenderer.material = materialList[3];
        }
        else
        {
            if (!touchingDirections.IsGrounded)
            {
                StartCoroutine(SwapMaterial(0.3f));
            }
            else
            {
                StartCoroutine(SwapMaterial(0.01f));
            }
        }
    }

    IEnumerator SwapMaterial(float delay)
    {
        yield return new WaitForSeconds(delay);
        runningParticleRenderer.material = materialList[0];
        jumpParticleRenderer.material = materialList[0];
    }
    private void CheckGravity()
    {
        if (IsSliding && !touchingDirections.OnSwamp)
        {
            rb.gravityScale = minGravity;
        }
        else if (IsSliding && touchingDirections.OnSwamp)
        {
            rb.gravityScale = 0;

        }
        else
        {
            rb.gravityScale = maxGravity;
        }
    }
    #region Прыжок
    public void OnJump(InputAction.CallbackContext cntx)
    {
        if (cntx.started && IsAlive && !gameManager.IsPaused && !gameManager.IsWin)
        {
            
            if (touchingDirections.IsGrounded)
            {
                jumpParticle.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                StartLockingVelocity(jumpFromWallTimer);
                //StartCoroutine(DeleyBeforeRisingJumpCounter());
                
                print("прыжок от земли");
            }
            else if (!touchingDirections.IsGrounded && IsSliding)
            {
                jumpParticle.Play();
                IsRight = !IsRight;
                rb.velocity = new Vector2(0.7f * transform.localScale.x * jumpPower, jumpPower);
                StartLockingVelocity(jumpFromWallTimer);
                JumpCounter++;
                
                print("прыжок от стены");
            }
            else if (JumpCounter < 2)
            {
                jumpParticle.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                StartLockingVelocity(jumpFromWallTimer);
                JumpCounter += 2;
                print("второй прыжок");
            }
        }       
    }
    public void NeedForceJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        
        if (forceJumpCoroutine == null)
        {
            jumpCounter = 0;
            forceJumpCoroutine = StartCoroutine(ForceJump(1));
        }
            
    }
    public void NeedForceJump(float koef)
    {
        print("прыжок от врага");
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (forceJumpCoroutine == null)
        {
            jumpCounter = 0;
            forceJumpCoroutine = StartCoroutine(ForceJump(koef));
        }
            
    }
    public void ForceJumpByAngle(float angle, Vector2 force)
    {
        jumpCounter = 0;
        print(angle);
        if (angle > 360)
        {
            angle -= 360;
        }
        if (angle < 0)
        {
            angle += 360;
        }
        float angleInRadians = angle * Mathf.Deg2Rad;
        Vector2 forceDirection = new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));
        print(Mathf.Cos(angleInRadians));
        print(Mathf.Rad2Deg * Mathf.Acos(angleInRadians));
        print(Mathf.Rad2Deg * Mathf.Asin(angleInRadians));
        rb.velocity = new Vector2(0 + Mathf.Cos(angleInRadians) * force.x, 0 + Mathf.Sin(angleInRadians) * force.y);
        jumpParticle.Play();
        StartLockingVelocity(0.2f);
    }
    IEnumerator ForceJump(float koef)
    {
       
        float elapsedTime = 0;
        rb.velocity = new Vector2(rb.velocity.x, koef * jumpPower);
        jumpParticle.Play();
        print("прыжок от объекта");
        while (elapsedTime < 0.1f)
        {
            if (touchingDirections.IsGrounded)
            {
                if (touchingDirections.IsGrounded)
                {                    
                    
                    break;
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        forceJumpCoroutine = null;
    }
    IEnumerator DeleyBeforeRisingJumpCounter()
    {
        yield return new WaitForSeconds(0.02f);
        JumpCounter++;
    }
    IEnumerator WaitingForResetJumpCounter()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if ((touchingDirections.IsGrounded || touchingDirections.IsOnWall)&& !touchingDirections.IsOnCeiling)
            {
                jumpCounter = 0;
                resetJumpCounterCoroutine = null;
                break;
            }
        }
    }
    #endregion

    #region Спрыгивание
    public void OnFall(InputAction.CallbackContext cntx)
    {
        
        if (cntx.started && IsAlive && !gameManager.IsPaused && !gameManager.IsWin)
        {
            
            StartCoroutine(Fall());
        }
    }
    IEnumerator Fall()
    {
        print("check");
        Physics2D.IgnoreLayerCollision(7, 9, true);
       
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreLayerCollision(7, 9, false);
        print("check end");
    }
    #endregion
    public void OnMove(InputAction.CallbackContext cntx)
    {
        if (IsAlive && !gameManager.IsPaused && !gameManager.IsWin)
        {
            moveInput = cntx.ReadValue<Vector2>();
            SetFacingDirection();
            IsMoving = moveInput != Vector2.zero;
        }

    }
    IEnumerator PlayRunningParticle()
    {
        while (true)
        {
            
            runningParticle.Play();
            yield return new WaitForSeconds(0.1f);
            
        }
    }
    public void StartLockingVelocity(float timer)
    {
        if (lockingVelocityCoroutine != null)
        {
            StopCoroutine(lockingVelocityCoroutine);
        }
        lockingVelocityCoroutine = StartCoroutine(LockingVelocity(timer));
    }
    IEnumerator LockingVelocity(float timer)
    {
        lockVelocity = true;
        SetFacingDirection();
        yield return new WaitForSeconds(timer);
        lockVelocity = false;
        SetFacingDirection();
        lockingVelocityCoroutine = null;
    }
    private void SetFacingDirection()
    {

        if (!IsRight && moveInput.x > 0)
        {
            IsRight = true;
        }
        else if (IsRight && moveInput.x < 0)
        {
            IsRight = false;
        }
    }
    public bool CheckPlayersPosition()
    {
        if (touchingDirections.IsGrounded || touchingDirections.IsOnCeiling)
        {
            return true;
        }
        return false;
    }
    #region Аксессоры
    public bool IsMoving
    {
        get { return isMoving; }
        private set
        {
            if (isMoving != value)
            {
                isMoving = value;

                animator.SetBool(AnimationStrings.isMoving, value);


            }

        }
    }
    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }
    public bool LockVelocity
    {
        get { return lockVelocity; }

    }
    public bool IsSliding
    {
        get { return animator.GetBool(AnimationStrings.isSliding); }
    }
    public float CurrentSpeed
    {
        get
        {
            if (isMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    if (touchingDirections.OnSand)
                    {
                        return moveSpeed / 4;
                    }
                    else if (touchingDirections.OnSwamp)
                    {
                        animator.SetBool(AnimationStrings.isMoving, false);
                        return 0f;
                    }
                    else
                    {
                        return moveSpeed;
                    }
                }
                else
                {
                    return airSpeed;
                }
            }
            else
            {
                return 0f;
            }
        }
    }
    public int JumpCounter
    {
        get { return jumpCounter; }
        set
        {
            jumpCounter = value;
            if (jumpCounter > 1)
            {
                animator.SetTrigger(AnimationStrings.doubleJump);
            }
            if (jumpCounter > 0)
            {
                if (resetJumpCounterCoroutine == null)
                {
                    resetJumpCounterCoroutine = StartCoroutine(WaitingForResetJumpCounter());
                }
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
                runningParticleTransform.localRotation = value ? Quaternion.Euler(0f, 0, 0f) : Quaternion.Euler(0f, 0, 180);
            }
            isRight = value;
        }
    }
    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
        set
        {
            if(!gameManager.IsWin)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
                GetComponent<Collider2D>().enabled = false;
                animator.SetBool(AnimationStrings.isAlive, value);
                UIManager.Instance.ShakeCamera();
                GetComponent<DestroyAfterDelay>().enabled = true;
                gameManager.RestartLevel();

            }
           
        }
    }
    #endregion
}
