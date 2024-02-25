using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHeadTrigger : MonoBehaviour
{
    Transform parent;
    RockHeadMovement rockHead;
    PlayerMovement playerMovement;
    [SerializeField] LayerMask groundLayer, playerLayer;
    private void Awake()
    {
        rockHead = GetComponentInParent<RockHeadMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            parent = collision.transform.parent;
            collision.transform.parent = transform;
            playerMovement = collision.GetComponent<PlayerMovement>();
        }
        if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)) && playerMovement !=null)
        {
            playerMovement.IsAlive = false;            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            collision.transform.parent = parent;
            parent = null;
            playerMovement = null;
        }
    }
}
