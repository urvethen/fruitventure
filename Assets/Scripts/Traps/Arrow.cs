using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow: MonoBehaviour
{
    [SerializeField] Vector2 power;

    Vector2 forceVector;
    Animator animator;
    Rigidbody2D rb;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, 0);
        */
        SoundManager.Instance.PlayArrowInteract();
        UIManager.Instance.ShakeCamera();
        collision.gameObject.GetComponent<PlayerMovement>().ForceJumpByAngle(transform.rotation.eulerAngles.z + 90f, power);
        animator.SetTrigger(AnimationStrings.hit);
        
    }

    void ApplyForceAtAngle(float angleInDegrees, float forceMagnitude)
    {
        // Конвертируем угол из градусов в радианы
        float angleInRadians = (angleInDegrees+90) * Mathf.Deg2Rad;        
        // Вычисляем вектор направления силы
        Vector2 forceDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
        // Применяем силу к объекту под заданным углом
        
        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Force);        
    }
}
