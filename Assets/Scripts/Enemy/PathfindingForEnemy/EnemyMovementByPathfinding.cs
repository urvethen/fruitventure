using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementByPathfinding : MonoBehaviour
{
    [SerializeField] Transform main;
    [SerializeField] Transform startPoint, endPoint;
    [SerializeField] List<Vector3> route = new List<Vector3>();
    [SerializeField] Pathfinding pathfinding;
    [SerializeField] Animator animator;
    [SerializeField] float speed = 1f;
    int index = 0;
    void Start()
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
        while (distance > 0.1f)
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
        }
        else
        {
            index = 0;
            route.Reverse();
        }
        if(IsAlive)
        StartCoroutine(Move(route[index], route[index+1]));
    }
    
    private bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }
}
