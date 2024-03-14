using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box: MonoBehaviour
{
    [SerializeField] private BoxType type;
    [Header("Настройки разрушения")]
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask playerHitLayer;
    [SerializeField] int endurance;
    [SerializeField] List<Vector2> position = new List<Vector2>();
    [SerializeField] List<GameObject> lbPiece = new List<GameObject>();
    [SerializeField] List<GameObject> mbPiece = new List<GameObject>();
    [SerializeField] List<GameObject> hbPiece = new List<GameObject>();
    [Header("Настройки выпадения")]
    [SerializeField] int fruitNumber = 3;
    [SerializeField] GameObject fruitPrefab;
    SoundManager soundManager;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        switch (type)
        {
            case BoxType.LightBox:
                animator.SetInteger(AnimationStrings.type, 0);
                Endurance = 1;
                break;
            case BoxType.MediumBox:
                animator.SetInteger(AnimationStrings.type, 1);
                Endurance = 3;
                break;
            case BoxType.HardBox:
                animator.SetInteger(AnimationStrings.type, 2);
                Endurance = 5;
                break;
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.layer != ground)
        {
            Endurance--;
        }
       
        animator.SetTrigger("hit");
        
        if (collision.gameObject.layer == playerHitLayer)
        {
            print("check");
            Endurance--;
            if (transform.position.y < collision.transform.position.y)
            {
                collision.gameObject.GetComponent<PlayerMovement>().NeedForceJump();
            }           
        }        
    }

*/
    private void Start()
    {
        GameManager.Instance.AddBox(transform);
        soundManager = SoundManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetTrigger(AnimationStrings.hit);
        soundManager.PlayBoxInteract();
        if (playerHitLayer == (playerHitLayer | (1 << collision.gameObject.layer)))
        {
            Endurance--;
            if (transform.position.y < collision.transform.parent.position.y)
            {
                collision.gameObject.GetComponentInParent<PlayerMovement>().NeedForceJump();
            }
        }
    }
    
    private void SpawnAction()
    {
        GameManager.Instance.RemoveBox(transform);
        for (int i = 0; i < position.Count; i++)
        {
            Rigidbody2D rb;
            switch (type)
            {
                case BoxType.LightBox:
                    rb = Instantiate(lbPiece[i], new Vector2(transform.position.x + position[i].x, transform.position.y + position[i].y), Quaternion.identity).GetComponent<Rigidbody2D>();
                    rb.AddForce(5 * position[i], ForceMode2D.Impulse);
                    break;
                case BoxType.MediumBox:
                    rb = Instantiate(mbPiece[i], new Vector2(transform.position.x + position[i].x, transform.position.y + position[i].y), Quaternion.identity).GetComponent<Rigidbody2D>();
                    rb.AddForce(5 * position[i], ForceMode2D.Impulse);
                    break;
                case BoxType.HardBox:
                    rb = Instantiate(hbPiece[i], new Vector2(transform.position.x + position[i].x, transform.position.y + position[i].y), Quaternion.identity).GetComponent<Rigidbody2D>();
                    rb.AddForce(5 * position[i], ForceMode2D.Impulse);
                    break;
            }

        }
        for (int i = 0; i < fruitNumber; i++)
        {
            Rigidbody2D rb = Instantiate(fruitPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.transform.GetChild(1).gameObject.SetActive(true);
            rb.AddForce(Random.Range(10f, 20f) * position[Random.Range(0, 4)], ForceMode2D.Impulse);
        }
    }
    public int Endurance
    {
        get { return endurance; }
        private set
        {
            endurance = value;
            animator.SetInteger(AnimationStrings.endurance, endurance);
            if (endurance < 1)
            {
                SpawnAction();
                Destroy(gameObject, 0.05f);
            }
        }
    }


    public enum BoxType
    {
        LightBox,
        MediumBox,
        HardBox
    }
}
