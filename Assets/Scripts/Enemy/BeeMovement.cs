using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement: EnemyMovementByPathfinding
{
    [SerializeField] float maxDistance = 5f; // ������������ ���������� �� ������
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
        float randomDistance = Random.Range(0f, maxDistance); // ���������� ��������� ���������� � �������� �������������
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // ���������� ��������� ���� � �������� 360 ��������

        float x = randomDistance * Mathf.Cos(randomAngle); // ��������� x ����������
        float y = randomDistance * Mathf.Sin(randomAngle); // ��������� y ����������

        return new Vector3(startPos.x + x, startPos.y + y, 0f); // ���������� ��������� ����� � �������� �����
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
