using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] bool isGray;
    [SerializeField] float speed = 1f;
    [SerializeField] Transform platform;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform startPoint;
    [SerializeField] Chain chain;
    bool onPlatform;
    Animator animator;
    Coroutine currentTask;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        chain = GetComponentInChildren<Chain>();
        platform = transform.GetChild(0);
        if (isGray)
        {
            animator.SetInteger(AnimationStrings.type, 1);
            animator.SetBool(AnimationStrings.start, true);
        }
        else
        {
            animator.SetInteger(AnimationStrings.type, 0);
        }
        chain.CreateChain(startPoint, endPoint);
    }
    private void Start()
    {
        if (isGray)
        {
            currentTask = StartCoroutine(MoveToEndPoint());
        }
    }

    
    
    
    IEnumerator MoveToEndPoint()
    {
        //float startTime = Time.time;
        float distance = Vector3.Distance(platform.position, endPoint.position);
        while(distance > 0.1f)
        {
            /*
            float distanceCovered = (Time.time - startTime) * speed;
            float journeyFraction = distanceCovered / distance;
            platform.position = Vector3.Lerp(startPoint.position, endPoint.position, journeyFraction);
            */
            platform.position = Vector3.MoveTowards(platform.position, endPoint.position, speed * Time.fixedDeltaTime);
            if ((!isGray && !onPlatform))
            {
                break;
            }
           distance = Vector3.Distance(platform.position, endPoint.position);
            yield return null;
           
        }
        if (isGray)
        {
            yield return new WaitForSeconds(0.5f);
            currentTask = StartCoroutine(MoveToStartPoint());
        }
        
    }
    
    IEnumerator MoveToStartPoint()
    {
       // float startTime = Time.time;
        float distance = Vector3.Distance(platform.position, startPoint.position);
        while (distance > 0.1f)
        {
            /*float distanceCovered = (Time.time - startTime) * speed;
            float journeyFraction = distanceCovered / distance;
            platform.position = Vector3.Lerp(endPoint.position, startPoint.position, journeyFraction);*/
            platform.position = Vector3.MoveTowards(platform.position, startPoint.position, speed * Time.fixedDeltaTime);
            if ((!isGray && onPlatform))
            {
                break;
            }
            distance = Vector3.Distance(platform.position, startPoint.position);
            yield return null;
        }
        if (isGray)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(MoveToEndPoint());
        }
        
    }
    #region Аксессоры
    public bool IsGray
    {
        get { return isGray; }
    }
    public bool OnPlatform
    {
        get { return onPlatform; }
        set
        {
            onPlatform = value;
            if (onPlatform)
            {
                animator.SetBool(AnimationStrings.start, true);
                StopCoroutine(MoveToStartPoint());
                currentTask = StartCoroutine(MoveToEndPoint());
            }
            else
            {
                animator.SetBool(AnimationStrings.start, false);
                StopCoroutine(MoveToEndPoint());
                currentTask = StartCoroutine(MoveToStartPoint());
            }
        }
    }
    #endregion
}
