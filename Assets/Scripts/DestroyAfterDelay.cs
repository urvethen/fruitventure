using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] bool needFade;
    float elapsedTime = 0f, alpha;
    Color startColor;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        if (needFade)
        {
            if (!TryGetComponent<SpriteRenderer>(out spriteRenderer))
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
            if (spriteRenderer != null)
            {
                startColor = spriteRenderer.color;
            }
        }
        
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        if (startColor != null && needFade)
        {
            alpha = startColor.a * (1 - elapsedTime/delay);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }
        if (elapsedTime> delay)
        {
            Destroy(gameObject);
        }
    }
}
