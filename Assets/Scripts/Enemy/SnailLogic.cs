using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailLogic : BaseEnemyLogic
{
    [SerializeField] Transform newSprite;
    [SerializeField] Transform shellPoint;
    [SerializeField] float timer = 0.25f;
    [SerializeField] GameObject shellPrefab;
    
    public override void DeathAction()
    {
        SoundManager.Instance.PlayDeath();
        rb.velocity = Vector3.zero;
        animator.SetTrigger(AnimationStrings.hit);
        Invoke("OnDeathAction", timer);
        UIManager.Instance.ShakeCamera();
        
       
    }
    private void OnDeathAction()
    {
        baseCollider2D.enabled = false;
        Instantiate(shellPrefab, shellPoint.position, Quaternion.identity);
        GetComponent<BaseEnemyMovement>().enabled = false;
        GetComponent<TouchingDirections>().enabled = false; 
        GetComponent<SpriteRenderer>().enabled = false;
        newSprite.gameObject.SetActive(true);
        Destroy(gameObject, 3f);
    }
}
