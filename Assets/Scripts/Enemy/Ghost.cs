using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ghost: BaseEnemyLogic
{
    [SerializeField] LayerMask enemyLayer, noCollisionLayer;
    [SerializeField] Vector2 cooldown = new Vector2(1f, 4f);
    [SerializeField] bool ready = true;
    SpriteRenderer spriteRenderer;
    SoundManager soundManager;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        soundManager = SoundManager.Instance;
    }

    private void FixedUpdate()
    {
        if (ready)
        {
            StartCoroutine(InvisibleCycle());
        }
        if (!IsAppear)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        }
    }
    IEnumerator InvisibleCycle()
    {
        ready = false;
        float elapsedTime = 0f;
        animator.SetTrigger(AnimationStrings.hide);
        soundManager.SetInvisible();
        gameObject.layer = GetLayerIndexFromLayerMask(noCollisionLayer);
        hitZone.gameObject.SetActive(false);
        float timer = Random.Range(cooldown.x, cooldown.y);
        while (true)
        {
            if (animator.GetBool(AnimationStrings.isReady))
            {                
                while (elapsedTime < timer)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                }
                elapsedTime = 0f;
                animator.SetTrigger(AnimationStrings.show);
                soundManager.SetVisible();
                break;
            }
            yield return null;
        }
        timer = Random.Range(cooldown.x, cooldown.y);
        while (true)
        {
            if (IsAppear)
            {
                gameObject.layer = GetLayerIndexFromLayerMask(enemyLayer);
                hitZone.gameObject.SetActive(true);
                while (elapsedTime < timer)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                }
                break;
            }
            yield return null;
        }
        ready = true;
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

    bool IsAppear
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAppear);
        }
    }
}
