using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement playerMovement;
        if (collision.TryGetComponent<PlayerMovement>(out playerMovement))
        {
            transform.parent.GetComponent<Animator>().SetTrigger(AnimationStrings.start);
            playerMovement.NeedForceJump();
            GameManager.Instance.WinByFinish();
        }
    }
}
