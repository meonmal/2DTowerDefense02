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
    /// ������ �ϴ� �Լ�
    /// </summary>
    /// <param name="target">���� ���</param>
    public void Setup(Transform target)
    {
        movement = GetComponent<Movement>();
        this.target = target;
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

        // �� ��� �Լ� ȣ��
        collision.GetComponent<Monster>().OnDie();
        // ������Ʈ ����
        Destroy(gameObject);
    }
}
