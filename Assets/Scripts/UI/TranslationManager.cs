using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TranslationManager : MonoBehaviour
{
    [SerializeField] bool isEndLoad;
    [SerializeField] GameObject prefab;
    [SerializeField] float distance = 100f;
    [SerializeField] float timer = 1f;
    bool ready = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!isEndLoad)
        {
            StartCoroutine(CreateField());
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<TransitionPiece>().Clear((float)i*timer / 100f);
            }
            
            Destroy(gameObject, 2f);
            
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    IEnumerator CreateField()
    {
        float elapsedTime = 0f;
        while (elapsedTime < timer)
        {
            Instantiate(prefab, transform);
            yield return new WaitForSeconds(timer/100);
            elapsedTime += timer / 100;
        }
        ready = true;
    }
    public bool Ready
    {
        get { return ready; }
    }
}
