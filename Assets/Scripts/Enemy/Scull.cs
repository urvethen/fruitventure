
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
        if (level == (level | (1 << collision.gameObject.layer))) // ���������, �������� �� LayerMask ��������� ����
        {

            Vector2 averageContactNormal = Vector2.zero;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                averageContactNormal += contact.normal;
            }

            averageContactNormal /= collision.contacts.Length; // ������� ������� ������� ���������

            Vector2 reflectedDirection = Vector2.Reflect(rb.velocity, averageContactNormal); // �������� ������� ����������� ������� ������������ ������� �������

            Vector2 avoidedReflection = AvoidCollision(collision, reflectedDirection); // �������� ������������� ������� � ������ ������

            rb.velocity = avoidedReflection; // ��������� �������� ��������� � �������
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
            float dotProduct = Vector2.Dot(avoidedReflection, contact.normal); // ��������� ��������� ������������ ����� ���������� ������������ � �������� ��������

            if (dotProduct < 0) // ���� ���������� ����������� ���������� � ������� ��������
            {
                avoidedReflection -= dotProduct * contact.normal; // ��������� ���������� �����������, ����� �������� �������������
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

        float randomAngle = Random.Range(0f, Mathf.PI * 2); // ���������� ��������� ���� � �������� 360 ��������

        float x = Mathf.Cos(randomAngle); // ��������� x ����������
        float y = Mathf.Sin(randomAngle); // ��������� y ����������

        return new Vector3(x, y); // ���������� ��������� ����� � �������� �����
    }
}
