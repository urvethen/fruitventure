
using UnityEngine;

public class Scull: BaseEnemyLogic
{
    [SerializeField] float speed;
    [SerializeField] LayerMask level;
    float elapsedTime = 0f;
    bool flag = true;
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (level == (level | (1 << collision.gameObject.layer))) // Проверяем, содержит ли LayerMask указанный слой
        {

            Vector2 averageContactNormal = Vector2.zero;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                averageContactNormal += contact.normal;
            }

            averageContactNormal /= collision.contacts.Length; // Находим среднюю нормаль контактов

            Vector2 reflectedDirection = Vector2.Reflect(rb.velocity, averageContactNormal); // Отражаем текущее направление объекта относительно средней нормали

            Vector2 avoidedReflection = AvoidCollision(collision, reflectedDirection); // Избегаем проникновение объекта в другой объект

            rb.velocity = avoidedReflection; // Применяем скорость отражения к объекту
            if (rb.velocity.magnitude < speed)
            {
                float koef = speed / rb.velocity.magnitude;
                rb.velocity *= koef;
            }
        }


    }
    Vector2 AvoidCollision(Collision2D collision, Vector2 reflection)
    {
        Vector2 avoidedReflection = reflection;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            float dotProduct = Vector2.Dot(avoidedReflection, contact.normal); // Вычисляем скалярное произведение между отраженным направлением и нормалью контакта

            if (dotProduct < 0) // Если отраженное направление направлено в сторону контакта
            {
                avoidedReflection -= dotProduct * contact.normal; // Уменьшаем отраженное направление, чтобы избежать проникновения
            }
        }

        return avoidedReflection;
    }
    private void Start()
    {
        rb.velocity = speed * GetRandomPointInCircle();
    }
    public void OnCollisionStay2D()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.1f && flag)
        {
            float tan = rb.velocity.y / rb.velocity.x;
            float atan = Mathf.Atan(tan);

            if (rb.velocity.x < 0)
            {
                atan += Mathf.PI;
            }
            if (atan > 0 && atan < Mathf.PI / 2)
            {
                rb.velocity = new Vector2(-speed * Mathf.Sqrt(2), -speed * Mathf.Sqrt(2));
            }
            else if (atan > Mathf.PI / 2 && atan < Mathf.PI)
            {
                rb.velocity = new Vector2(speed, -speed * Mathf.Sqrt(2));
            }
            else if (atan > Mathf.PI && atan < 3 * Mathf.PI / 2)
            {
                rb.velocity = new Vector2(speed, speed * Mathf.Sqrt(2));
            }
            else if (atan > 3 * Mathf.PI / 2 && atan < 2 * Mathf.PI)
            {
                rb.velocity = new Vector2(-speed, speed * Mathf.Sqrt(2));
            }
            flag = false;
        }
    }
    public void OnCollisionExit2D()
    {
        elapsedTime = 0f;
        flag = true;
    }
    Vector2 GetRandomPointInCircle()
    {

        float randomAngle = Random.Range(0f, Mathf.PI * 2); // Генерируем случайный угол в пределах 360 градусов

        float x = Mathf.Cos(randomAngle); // Вычисляем x координату
        float y = Mathf.Sin(randomAngle); // Вычисляем y координату

        return new Vector3(x, y); // Возвращаем случайную точку в пределах круга
    }
}
