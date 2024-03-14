using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BirdFly: MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] float speed;
    [SerializeField] ParticleSystem particle;
    [SerializeField] Transform flag;
    [SerializeField] SoundManager soundManager;
    bool isUp = false;

    bool IsUp
    {
        get { return isUp; }
        set 
        { 
            if(isUp != value)
            {
                isUp = value;
                StopAllCoroutines();
                if (value)
                {                    
                    StartCoroutine(FlyUp());
                }
                else
                {
                    StartCoroutine(FlyDown());
                }
            } 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FlyDown());
        soundManager = SoundManager.Instance;
    }
    private void FixedUpdate()
    {
        if (flag.gameObject.activeSelf)
        {
           IsUp = true;
        }
        else
        {
            IsUp = false;
        }
    }

    IEnumerator FlyUp()
    {
        particle.Play();
        soundManager.PlayBirdFly();
        while (true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
           
            yield return null;
        }
    }
    IEnumerator FlyDown()
    {
        while (true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            yield return null;
        }
    }
}
