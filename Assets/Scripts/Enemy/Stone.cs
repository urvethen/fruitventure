using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone: BaseEnemyLogic
{
    [SerializeField] GameObject prefab;
    [SerializeField] float power;
    [SerializeField] float invincibleTime = 0.5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] BaseEnemyMovement enemyMovement;
    public override void DeathAction()
    {
        
        if (prefab != null)
        {
            for (int i = 0; i <2; i++)
            {
                Vector2 powerVector = new Vector2((1-2*i)*power*Mathf.Sin(Mathf.PI/4), power * Mathf.Cos(Mathf.PI/4));
                print(powerVector);
                Rigidbody2D rbStone = Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                rbStone.AddForce(powerVector, ForceMode2D.Impulse);
            }
        }
        base.DeathAction();
    }
    private void Awake()
    {
        enemyMovement = GetComponent<BaseEnemyMovement>();
        enemyMovement.lockVelocity = true;
    }
    private void Start()
    {
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        
        float elapsedTime = 0f;
        while (elapsedTime < invincibleTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        hitZone.gameObject.SetActive(true);
        gameObject.layer = GetLayerIndexFromLayerMask(enemyLayer);
        animator.SetBool(AnimationStrings.canMove, true);
        enemyMovement.lockVelocity = false;
    }
    int GetLayerIndexFromLayerMask(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }

        return layerNumber - 1;

    }
}
