using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit: MonoBehaviour
{
    [SerializeField] bool isCollected;
    /*! ������ �������
     * 0 - ������
     * 1 - �����
     * 2 - �����
     * 3 - ����
     * 4 - ����
     * 5 - ��������
     * 6 - ������
     * 7 - ��������
     */
    [SerializeField] int fruitNumber = 8;
    [SerializeField] bool randomFruit;
    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if(randomFruit)
        {
            animator.SetInteger(AnimationStrings.fruitNumber, Random.Range(0, fruitNumber));
        }
    }
    private void Start()
    {
       StartCoroutine(WaitForNextUpdate());
    }
    IEnumerator WaitForNextUpdate()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.NeedToCollect(transform);
    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject, 0.5f);
            animator.SetBool(AnimationStrings.isCollected, true);
            GameManager.Instance.WasCollected(transform);
        }
        
    }
}
