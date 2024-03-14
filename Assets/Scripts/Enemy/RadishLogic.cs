using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadishLogic: BaseEnemyLogic
{

    bool firstContact = true;
    [SerializeField] BaseEnemyMovement movement;
    [SerializeField] Transform cliffDetection;
    [SerializeField] List<GameObject> parts = new List<GameObject>();
    [SerializeField] Transform spawnPos;
    [SerializeField] bool needSpawn = true;


    public override void OnHitZoneEnter(Collider2D collision)
    {
        if (!firstContact)
        {

            base.OnHitZoneEnter(collision);
        }
        else
        {
            SecondPhase(collision);
        }
    }
    protected virtual void SecondPhase(Collider2D collision)
    {
        GetComponent<AudioSource>().Stop();
        transform.localScale = Vector3.one;
        GetComponent<BeeMovement>().enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        animator.SetTrigger(AnimationStrings.hit);
        if (needSpawn)
            SpawnParts();
        StartCoroutine(Wait());
        cliffDetection.gameObject.SetActive(true);
        movement.enabled = true;
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out playerMovement))
        {
            playerMovement.NeedForceJump(1f);
        }
    }

    void SpawnParts()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            Rigidbody2D rbP = Instantiate(parts[i], spawnPos.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.PI / 4 + i * Mathf.PI / 2) * 3, Mathf.Sin(Mathf.PI / 4 + i * Mathf.PI / 2) * 3);
            rbP.AddForce(direction, ForceMode2D.Impulse);
            Destroy(rbP.gameObject, 2f);
        }
        needSpawn = false;
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        firstContact = false;
    }
}
