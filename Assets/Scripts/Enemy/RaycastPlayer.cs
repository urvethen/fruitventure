using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastPlayer: MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] LayerMask layers;
    [SerializeField] Vector3 offset = new Vector3(0, 0.4f, 0);
    [SerializeField] float detectionDistance = 15f;
    [SerializeField] bool hasTarget;
    Animator animator;
    public UnityEvent findTarget;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Instance.PlayerTransform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.position.y - transform.position.y < 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + offset.x*transform.localScale.x, transform.position.y + offset.y * transform.localScale.y), Vector2.left * transform.localScale.x, detectionDistance, layers);
            //Debug.DrawRay(new Vector2(transform.position.x + offset.x * transform.localScale.x, transform.position.y + offset.y * transform.localScale.y), Vector2.left * transform.localScale.x * detectionDistance, Color.green);
            if (hit.collider != null && hit.collider.CompareTag("Player")  )
            {
                HasTarget = true;

            }
            else
            {
             
                HasTarget = false;
            }
        }
        else
        {
            HasTarget = false;
        }

    }
    public bool HasTarget
    {
        get { return hasTarget; }
        private set
        {
            if (hasTarget != value)
            {
                hasTarget = value;
                animator.SetBool(AnimationStrings.hasTarget, value);
                if (hasTarget)
                {
                    findTarget?.Invoke();
                }
            }
        }
    }
}
