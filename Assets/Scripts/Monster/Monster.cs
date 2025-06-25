using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    /// <summary>
    /// wayPoint ����
    /// </summary>
    private int wayPointsCount;
    /// <summary>
    /// wayPoint ����
    /// </summary>
    private Transform[] wayPoints;
    /// <summary>
    /// ���� ��ǥ ���� �ε���
    /// </summary>
    private int currentIndex;
    /// <summary>
    /// ������Ʈ �̵� ����
    /// </summary>
    private Movement movement;

    /// <summary>
    /// �̵��ϱ� �� ������ �ϴ� �Լ�
    /// </summary>
    /// <param name="wayPoints">�̵��ؾ��� ����</param>
    public void Setup(Transform[] wayPoints)
    {
        // movement�� ������Ʈ ��������
        movement = GetComponent<Movement>();

        // �� �̵� ��� wayPoints ���� ����
        // wayPointsCount�� wayPoints�� ������.
        wayPointsCount = wayPoints.Length;
        // wayPoints�� �޸𸮸� �Ҵ��ϴµ� �� ũ��� wayPointsCount�̴�.
        this.wayPoints = new Transform[wayPointsCount];
        // wayPoints�� ������ ���� �̸��� �Ű������� wayPoints�� ��
        this.wayPoints = wayPoints;

        // ���� ù ��ġ�� ù��° wayPoints�� ��ġ�� ����
        transform.position = wayPoints[0].position;

        StartCoroutine(HasReachedGoal());
    }

    /// <summary>
    /// ���� ��ǥ �������� �̵��� �� �ߴ��� Ȯ���ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator HasReachedGoal()
    {
        NextMove();

        // �ݺ����� ���� ���� ����
        while (true)
        {
            // ���� ���� ��ġ�� ���� ��ǥ ���������� �Ÿ��� 0.02������ �� ����
            // * movement.MoveSpeed�� �ϴ� ������ �̵��ӵ��� �ʹ� ������ �� �����ӿ�
            // 0.02���� �� ũ�� ������ �� �ֱ� ������.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement.MoveSpeed)
            {
                NextMove();
            }

            yield return null;
        }
    }

    /// <summary>
    /// ���� �̵� ������ �����ϴ� �Լ�
    /// </summary>
    private void NextMove()
    {
        // ���� ���� �̵��ؾ� �� wayPoint�� ���� ������ ����
        if(currentIndex < wayPointsCount -1)
        {
            // ������Ʈ�� ��ġ�� ���� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            // �̵��ؾ� �� ������ ���� ��ǥ ������ �������� ����
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement.MoveDir(direction);
        }
        // ���� �������� �̵��ؾ� �� wayPoint�� ���ٸ� ����
        else
        {
            // ���� ������Ʈ ����
            Destroy(gameObject);
        }
    }
}
