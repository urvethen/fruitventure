using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFly: MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] float speed;
    [SerializeField] ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlyDown());
    }

    IEnumerator FlyUp()
    {
        
        float elapsedTime = 0f;
        while (elapsedTime < cooldown / 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FlyDown());
    }
    IEnumerator FlyDown()
    {
        float elapsedTime = 0f;
        while (elapsedTime < cooldown / 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        particle.Play();
        StartCoroutine(FlyUp());
    }
}
