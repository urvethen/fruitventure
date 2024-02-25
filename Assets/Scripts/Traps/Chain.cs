using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] GameObject chainPrefab;
    [SerializeField] float period;
    List<GameObject> chainList = new List<GameObject>();

    public void CreateChain(Transform A, Transform B)
    {
        Vector3 vector = B.position - A.position;
        float distance = vector.magnitude;
        int count = Mathf.RoundToInt( distance / period);
        for (int i = 0; i < count; i++)
        {
            Vector3 place = Vector3.Lerp(A.position, B.position, i * period);
            GameObject element = Instantiate(chainPrefab, place, Quaternion.identity, transform);
            chainList.Add(element);  
        }
    }
    public void MoveChain(Transform A, Transform B)
    {
      //  Vector3 vector = Vector3.Normalize( B.position - A.position);
        for (int i = 0;i < chainList.Count;i++)
        {
            chainList[i].transform.position = Vector3.Lerp(A.position, B.position, i * period);
        }
    }
}
