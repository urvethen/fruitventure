using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPiece : MonoBehaviour
{
    [SerializeField] bool isEndLoad;
    [SerializeField] Vector3 startScale = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField] Vector3 endScale = new Vector3(5f, 5f, 5f);
    [SerializeField] float timer = 0.5f;
    float elapsedTime = 0f;
    float endTime;

    private void Awake()
    {
        transform.localScale = startScale;
        elapsedTime = 0f;
        endTime = elapsedTime + timer;
        if (!isEndLoad)
        {
            StartCoroutine(CreateField());
        }
       
    }
    public void Clear(float time)
    {
        
        StartCoroutine(ClearField(time));
    }
    IEnumerator ClearField(float time)
    {
        yield return new WaitForSeconds(time);
        float elapsed = 0f;
        while (elapsed < timer)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / timer);
            yield return null;
        }
        yield return null;
      //  print("check)");
        
        
    }
    IEnumerator CreateField()
    {
        float elapsed = 0f;
        while (elapsed < timer)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime/timer);
            yield return null;
        }
    }
}
