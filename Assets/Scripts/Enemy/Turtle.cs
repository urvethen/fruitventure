using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : BaseEnemyLogic
{
    [SerializeField] GameObject spikes;
    [SerializeField] Vector2 time;
    void Start()
    {
        float random = Random.value;
        if (random > 0.5f)
        {
            animator.SetBool(AnimationStrings.isReady, false);
        }
        else
        {
            animator.SetBool(AnimationStrings.isReady, true);
        }
        StartCoroutine(Circle());
    }

    IEnumerator Circle()
    {
        float elapsedTime=0f;
        float timer;
        while (true)
        {
            if (animator.GetBool(AnimationStrings.isReady))
            {
                spikes.SetActive(true);
                timer = Random.Range(time.x, time.y);
                while (elapsedTime< timer)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                }
                elapsedTime = 0f;
                animator.SetBool(AnimationStrings.isReady, false);
            }
            else
            {
                spikes.SetActive(false);
                timer = Random.Range(time.x, time.y)/2;
                while (elapsedTime < timer)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                }
                elapsedTime = 0f;
                animator.SetBool(AnimationStrings.isReady, true);
            }
        }
    }
}
