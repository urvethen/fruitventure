using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementByPathfinding : MonoBehaviour
{
    [SerializeField] protected Transform main;
    [SerializeField] protected Transform startPoint, endPoint;
    [SerializeField] protected List<Vector3> route = new List<Vector3>();
    [SerializeField] protected Pathfinding pathfinding;
    [SerializeField] protected Animator animator;
    [SerializeField] protected float speed = 1f;
    [SerializeField]protected int index = 0;
   
    protected virtual void Start()
    {
        StartMovement();
    }
    protected virtual void StartMovement()
    {
        RouteRequest();
        FindNearestPointInRoute();
        StartCoroutine(Move(route[index], route[index + 1]));
    }
    protected virtual void RouteRequest()
    {
        route = pathfinding.CreateRoute(startPoint.position, endPoint.position);
    }

    protected virtual void FindNearestPointInRoute()
    {
        float distance = 100f;
        
        for (int i = 0; i < route.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, route[i]);
            if (dist < distance)
            {
                distance = dist;
                index = i;
            }
        }
    }
    protected virtual IEnumerator Move(Vector3 A, Vector3 B)
    {
        float distance = Vector3.Distance(A, B);
        if(A.x >  B.x)
        {
            main.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            main.localScale = new Vector3(-1, 1, 1);
        }
        while (distance > 0.01f)
        {
            
            main.position = Vector3.MoveTowards(main.position, B, Time.deltaTime * speed);
            distance = Vector3.Distance(main.position, B);
            
            yield return null;
            if (!IsAlive)
                break;
        }
        if (index+1 < route.Count - 1)
        {
            index++;
            if (IsAlive)
                StartCoroutine(Move(route[index], route[index + 1]));
        }
        else
        {
            EndRoute();
        }
        
    }
    
    protected virtual void EndRoute()
    {
        index = 0;
        route.Reverse();
        if (IsAlive)
            StartCoroutine(Move(route[index], route[index + 1]));
    }
    private bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }
}
