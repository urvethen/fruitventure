using System.Collections;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] bool alwaysOn = false;
    [SerializeField] bool startFromOn = true;
    [SerializeField] bool isPlayerTrigger = false;
    [SerializeField] float delay = 3f;
    [SerializeField] float workTime = 3f;
    [SerializeField] float size = 5f;
    [SerializeField, Tooltip("Для вертикального - 25 норм, для горизантольного цифры поряка 200")] float force = 5f;
    Vector2 forceVector = Vector2.zero;
    Animator animator;
    ParticleSystem particle;
    ParticleSystem.MainModule mainParticle;
    BoxCollider2D mainCollider;

    private void OnEnable()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        mainParticle = particle.main;
        if (!alwaysOn)
        {
            mainParticle.duration = workTime - 0.5f;
        }
        else
        {
            mainParticle.loop = true;
        }
     
    }
    private void Awake()
    {
        
        animator = GetComponent<Animator>();
        
        mainCollider = GetComponent<BoxCollider2D>();
        float angle = transform.rotation.eulerAngles.z;
        switch (angle)
        {
            case 0: 
                forceVector = new Vector2(0, force);
                break;
            case -270:
            case 90:
                forceVector = new Vector2(-force, 0);
                break;
            case -180:
            case 180:
                forceVector = new Vector2(0, force);
                break;
            case -90:
            case 270:
                forceVector = new Vector2(force, 0);
                break;
        }
    }

    private void Start()
    {
        mainCollider.size = new Vector2(mainCollider.size.x, size);
        mainCollider.offset = new Vector2(0, size/2);
        mainParticle.startSpeed = size;
        if (alwaysOn)
        {
            StartCoroutine(FanOn());
        }
        else
        {
            
            if (startFromOn)
            {
                StartCoroutine(FanOn());
            }
            else
            {
                StartCoroutine(FanOff());
            }
        }
    }

    IEnumerator FanOn()
    {
        animator.SetBool(AnimationStrings.start, true);
        mainCollider.enabled = true;
       
        if (!alwaysOn)
        {
            particle.Play();
            yield return new WaitForSeconds(workTime-0.5f);
            animator.SetBool(AnimationStrings.start, false);
            yield return new WaitForSeconds( 0.5f);
            StartCoroutine(FanOff());
        }
        else
        {
            particle.Play();
        }
    }
    IEnumerator FanOff()
    {
        animator.SetBool(AnimationStrings.start, false);
        mainCollider.enabled= false;
        //  particle.Stop();
        if (!alwaysOn)
        {
            float elapsedTime = 0f;
            while (elapsedTime < delay)
            {
                yield return new WaitForFixedUpdate();
                elapsedTime += Time.fixedDeltaTime;
            }
            StartCoroutine(FanOn());
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
     
        if(collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(forceVector);
        }
    }
}
