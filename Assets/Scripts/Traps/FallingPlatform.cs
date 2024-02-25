using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float workingTime = 0.5f;
    [SerializeField]LayerMask playerHitLayer;
    Animator animator;
    Rigidbody2D rb;
    PlatformEffector2D effector;
    SpriteRenderer spriteRenderer;
    ParticleSystem particle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        effector = GetComponent<PlatformEffector2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (playerHitLayer == (playerHitLayer | (1 << collision.gameObject.layer)))
            StartCoroutine(WaitToStop());
    }
    
    IEnumerator WaitToStop()
    {
        yield return new WaitForSeconds(workingTime);
        print("check");
        animator.SetBool(AnimationStrings.start, false);
        particle.Stop();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddTorque(5f);
        //StartCoroutine(Rotate());
        effector.enabled = false;
        //spriteRenderer.sortingLayerName = layer.ToString();
        GetComponent<DestroyAfterDelay>().enabled = true;
        
    }
    IEnumerator Rotate()
    {
        while (true)
        {
            rb.AddTorque(5f);
            print(rb.angularVelocity);
            yield return null;
        }
    }
}
