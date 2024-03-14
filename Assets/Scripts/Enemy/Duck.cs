using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck: BaseEnemyLogic
{
    [SerializeField] TouchingDirections touchingDirections;
    [SerializeField] DetectionZone wallDetection;
    [SerializeField] float rotateDelay;
    [SerializeField] Vector2 jumpPower, jumpDelay;
    Vector2 jumpVector;
    bool readyForJump = true, canRotate = true;
    Coroutine flying;
    SoundManager soundManager;
    // Start is called before the first frame update
    private void Start()
    {
        touchingDirections = GetComponent<TouchingDirections>();
        soundManager = SoundManager.Instance;
    }
    private void FixedUpdate()
    {
        if (readyForJump && flying == null && touchingDirections.IsGrounded)
        {
            //animator.SetTrigger(AnimationStrings.jump);
            rb.AddForce(JumpPower, ForceMode2D.Impulse);
            soundManager.PlayJump();
            readyForJump = false;
            flying = StartCoroutine(WaitingForLand());

        }
        if (wallDetection.colliders.Count > 0 && canRotate)
        {

            StartCoroutine(RotateDelay());
        }
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

    }
    IEnumerator WaitingForLand()
    {
        yield return new WaitForSeconds(0.5f);
        while (!touchingDirections.IsGrounded)
        {
            yield return null;
        }
        flying = null;
        yield return new WaitForSeconds(Random.Range(jumpDelay.x, jumpDelay.y));
        readyForJump = true;
    }
    IEnumerator RotateDelay()
    {
        Vector3 prevVelocity = rb.velocity;
        canRotate = false;
        float elapsedTime = 0f;

        bool flag = true;
        while (elapsedTime < rotateDelay)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            if (wallDetection.colliders.Count < 1)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            transform.localScale *= new Vector2(-1, 1);
            rb.velocity = new Vector2(-prevVelocity.x, prevVelocity.y);

        }

        canRotate = true;
    }
    Vector2 JumpPower
    {
        get
        {
            return new Vector2(-Random.Range(jumpPower.x, jumpPower.y) * Mathf.Sin(Mathf.PI / 6) * transform.localScale.x, Random.Range(jumpPower.x, jumpPower.y) * Mathf.Cos(Mathf.PI / 6));
        }
    }
}
