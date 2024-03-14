using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemyLogic
{
    [SerializeField] GameObject particlePrefab;
    [SerializeField] Vector2 spawnTime;
    [SerializeField] Transform spawnPoint;

    private void Start()
    {
        StartCoroutine(ParticleSpawn());
    }

    IEnumerator ParticleSpawn()
    {
        float spawnCD;
        while (animator.GetBool(AnimationStrings.isAlive))
        {
            spawnCD = Random.Range(spawnTime.x, spawnTime.y);
            yield return new WaitForSeconds(spawnCD);
            Instantiate(particlePrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
