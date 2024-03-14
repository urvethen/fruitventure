using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement: EnemyMovementByPathfinding
{
    [SerializeField] float maxDistance = 5f; // ћаксимальное рассто€ние от центра
    [SerializeField]Vector3 startPos;

    protected override void Start()
    {
        startPos = main.transform.position;
        base.Start();
    }
    protected override void StartMovement()
    {
       
        RouteRequest();
        
        if (route.Count > 2)
        {
            StartCoroutine(Move(route[index], route[index + 1]));
        }
        else
        {
            StartMovement();
        }

    }
    protected override void RouteRequest()
    {
        route.Clear();
        route = pathfinding.CreateRoute(main.position, GetRandomPointInCircle());
    }
    Vector3 GetRandomPointInCircle()
    {
        float randomDistance = Random.Range(0f, maxDistance); // √енерируем случайное рассто€ние в пределах максимального
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // √енерируем случайный угол в пределах 360 градусов

        float x = randomDistance * Mathf.Cos(randomAngle); // ¬ычисл€ем x координату
        float y = randomDistance * Mathf.Sin(randomAngle); // ¬ычисл€ем y координату

        return new Vector3(startPos.x + x, startPos.y + y, 0f); // ¬озвращаем случайную точку в пределах круга
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    protected override void EndRoute()
    {
       
        route.Clear();
        index = 0;
        StartMovement();
    }
}
