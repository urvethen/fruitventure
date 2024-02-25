using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> colliders = new List<Collider2D>();
    Collider2D col;
    public UnityEvent<Collider2D> onCollisionDetected;
    public UnityEvent noCollisionRemain;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        colliders.Add(collision);
        onCollisionDetected?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision);
        if (colliders.Count <1 )
        {
            noCollisionRemain?.Invoke();
        }
    }
}
