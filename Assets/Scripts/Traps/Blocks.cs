using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] List<GameObject> parts = new List<GameObject>();
    [SerializeField] Vector2 spawnPoint;
    [SerializeField] LayerMask playerHitLayer;
    [SerializeField] bool needJump;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerHitLayer == (playerHitLayer | (1 << collision.gameObject.layer)) )
        {
            animator.SetTrigger(AnimationStrings.hit_top);
            if (transform.position.y < collision.transform.parent.position.y && needJump)
            {
                collision.gameObject.GetComponentInParent<PlayerMovement>().NeedForceJump();
            }
            Rigidbody2D rb;
            for (int i = 0; i < parts.Count; i++)
            {
                rb = Instantiate(parts[i], new Vector2(transform.position.x + spawnPoint.x * (1 - 2 * i), transform.position.y + spawnPoint.y * (1 - 2 * i)), Quaternion.identity).GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 10, ForceMode2D.Impulse);
            }
            Destroy(gameObject, 0.05f);
        }
    }
    
}
