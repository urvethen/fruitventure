using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet: MonoBehaviour
{
    [SerializeField] List<GameObject> parts = new List<GameObject>();
    [SerializeField] Transform createPosition;
    [SerializeField] float power;
    [SerializeField] int partsExists = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        if (partsExists ==0)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                Rigidbody2D partsRB = Instantiate(parts[i], createPosition.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                Vector2 forceVector = transform.rotation.eulerAngles.z > 90 ? new Vector2(-1 * Mathf.Cos(Mathf.PI / 4 - i * Mathf.PI / 2), 1 * Mathf.Sin(Mathf.PI / 4 - i * Mathf.PI / 2)) : new Vector2(Mathf.Cos(Mathf.PI / 4 - i * Mathf.PI / 2), Mathf.Sin(Mathf.PI / 4 - i * Mathf.PI / 2));
                print(forceVector);
                partsRB.AddForce(power * forceVector, ForceMode2D.Impulse);
                partsExists++;
                Destroy(partsRB.gameObject, 2f);
            }
        }
        


        Destroy(gameObject, 0.1f);
    }
}
