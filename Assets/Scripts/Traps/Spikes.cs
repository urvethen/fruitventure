using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement playerMovement = collision.transform.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.IsAlive = false;
        }
    }
}
