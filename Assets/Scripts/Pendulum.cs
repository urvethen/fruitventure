using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum: MonoBehaviour
{
    [SerializeField] float length = 5f;
    [SerializeField] bool fullCircle = false;
    [SerializeField] bool clockwise = true;
    [SerializeField] float angle = 45f; // Максимальный угол наклона маятника
    [SerializeField] float speed = 2f; // Скорость качания маятника
    [SerializeField] Chain chain;
    [SerializeField] bool needCalculateAngle;
    [SerializeField] float initialAxes, initialAngle;
    Vector3 direction;
    Rigidbody2D rb;
    float timeCounter = 0;
    Transform parent;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        parent = transform.parent;
    }
    private void Start()
    {
        chain.CreateChain(parent, transform);
        initialAngle = parent.localRotation.eulerAngles.z * Mathf.Deg2Rad;

        direction = clockwise ? Vector3.forward : Vector3.back;
    }
    void FixedUpdate()
    {
        if (!fullCircle)
        {
            timeCounter += Time.fixedDeltaTime * speed;
            float x = Mathf.Sin(direction.z * timeCounter + initialAngle) * angle;
            parent.localRotation = Quaternion.Euler(0, 0, x + initialAxes);
        }
        else
        {
            transform.RotateAround(parent.position, direction, speed * Time.fixedDeltaTime);
        }
        chain.MoveChain(parent, transform);
    }
}

