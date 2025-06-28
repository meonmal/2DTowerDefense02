using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// �̵��� ���� �����Ʈ ����
    /// </summary>
    private Movement movement;
    /// <summary>
    /// ���� ���
    /// </summary>
    private Transform target;
    /// <summary>
    /// �߻�ü�� �ִ� ������
    /// </summary>
    private float damage;

    /// <summary>
    /// ������ �ϴ� �Լ�
    /// </summary>
    /// <param name="target">���� ���</param>
    public void Setup(Transform target, float damage)
    {
        movement = GetComponent<Movement>();
        // Ÿ���� �������� ���� ���
        this.target = target;
        // Ÿ���� �������� ������
        this.damage = damage;
    }

    private void Update()
    {
        // ���� ���� ����� �����ϸ� ����
        if(target != null)
        {
            // �߻�ü�� ���� ����� �Ÿ��� ��� ��
            Vector3 direction = (target.position - transform.position).normalized;
            // �̵� ����
            movement.MoveDir(direction);
        }
        // ���� ���� ���ٸ� ����
        else
        {
            // ������Ʈ ����
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���� ����� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="collision">���� ���</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ����� �±װ� Monster�� �ƴϸ� ����
        if (!collision.CompareTag("Monster"))
        {
            // ���
            return;
        }

        // ���� ����� ���� �ƴ� �� ����
        if(collision.transform != target)
        {
            // ���
            return;
        }

        //// �� ��� �Լ� ȣ��
        //collision.GetComponent<Monster>().OnDie();
        // ������ ü���� damage��ŭ ����
        collision.GetComponent<MonsterHP>().TakeDamage(damage);
        // ������Ʈ ����
        Destroy(gameObject);
    }
}
