using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 minMaxPosition;
    [SerializeField] Vector2 direction;
    [SerializeField] Transform t;

    private void Awake()
    {
        t = transform;
    }
    

    void FixedUpdate()
    {
        t.Translate(direction * speed * Time.fixedDeltaTime);
        if (t.position.x * direction.x > direction.x * minMaxPosition.y ||
            t.position.y * direction.y > direction.y * minMaxPosition.y)
        {
            t.position = new Vector3(minMaxPosition.x * direction.x, minMaxPosition.y * direction.y, t.position.z);
        }
    }
}
