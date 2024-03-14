using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap: MonoBehaviour
{
    [SerializeField] float timer = 5f;
    [SerializeField] float prepareTimer = 0.5f;
    Animator animator;
    AudioSource audioSource;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsReady)
        {
            PlayerMovement playerMovement = collision.transform.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                StartCoroutine(Prepare());
            }
        }
        
    }
    IEnumerator Prepare()
    {
        IsReady = false;
        Start = true;
        yield return new WaitForSeconds(prepareTimer);
        StartCoroutine(Fire() );
    }
    IEnumerator Fire()
    {
        audioSource.Play();
        Hit = true;
        yield return new WaitForSeconds(timer);
        audioSource.Stop();
        Hit = false;
        Start = false;
        IsReady = true;
    }
    public bool IsReady
    {
        get { return animator.GetBool(AnimationStrings.isReady); }
        set { animator.SetBool(AnimationStrings.isReady, value); }
    }
    public bool Hit
    {
        get { return animator.GetBool(AnimationStrings.hit); }
        set { animator.SetBool(AnimationStrings.hit, value); }
    }
    public bool Start
    {
        get { return animator.GetBool(AnimationStrings.start); }
        set { animator.SetBool(AnimationStrings.start, value); }
    }

}
