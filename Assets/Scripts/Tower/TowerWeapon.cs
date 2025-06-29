using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// SearchTarget = ���� ã�� ����
/// AttackTarget = ���� ����
/// </summary>
public enum TowerState { SearchTarget = 0, AttackTarget, }
public class TowerWeapon : MonoBehaviour
{
    /// <summary>
    /// �߻�ü ������
    /// </summary>
    [SerializeField]
    private GameObject bullet;
    /// <summary>
    /// �߻�ü ��ȯ ��ġ
    /// </summary>
    [SerializeField]
    private Transform spawnPosition;
    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    [SerializeField]
    private float attackRate;
    /// <summary>
    /// ���� ����
    /// </summary>
    [SerializeField]
    private float attackRange;
    /// <summary>
    /// Ÿ���� ���ݷ�
    /// </summary>
    [SerializeField]
    private float attackDamage;
    /// <summary>
    /// Ÿ���� ����
    /// </summary>
    [SerializeField]
    private int level;
    /// <summary>
    /// Ÿ���� ���� ����
    /// </summary>
    private TowerState towerState = TowerState.SearchTarget;
    /// <summary>
    /// ���� ���
    /// </summary>
    private Transform attackTarget = null;
    /// <summary>
    /// ���� ����� ã�� ���� ����(����Ʈ�� ���� ã��)
    /// </summary>
    private MonsterSpawner monsterSpawner;

    public float Damage => attackDamage;
    public float Rate => attackRate;
    public float Range => attackRange;
    public int Level => level;

    /// <summary>
    /// ó���� ���¸� �����ϴ� �Լ�
    /// </summary>
    /// <param name="monsterSpawner"></param>
    public void Setup(MonsterSpawner monsterSpawner)
    {
        this.monsterSpawner = monsterSpawner;

        // ó���� ���¸� Ž�� ���·� �Ѵ�.
        ChangeState(TowerState.SearchTarget);
    }

    /// <summary>
    /// Ÿ���� ���¸� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="towerState">Ÿ���� ����</param>
    public void ChangeState(TowerState newState)
    {
        // ������ ���¸� ���߰�
        StopCoroutine(towerState.ToString());
        // ���¸� �����ؼ�
        towerState = newState;
        // ����� ���¸� ����Ѵ�.
        StartCoroutine(towerState.ToString());
    }

    /// <summary>
    /// �� ������ ���� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Update()
    {
        // ���� ���� ����� ���ٸ� ����
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }

    /// <summary>
    /// Ÿ���� ���� �ٶ󺸵��� �ϴ� �Լ�
    /// </summary>
    private void RotateToTarget()
    {
        // Ÿ���� ��ġ�� ������ ��ġ�� x, y ��ǥ�� ����ؼ� ������ ������ش�.
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        // ������ ���� x��� y���� ��ǥ�� ����ؼ� z���� ȸ������ ���Ѵ�.
        // Atan2 �Լ��� y��ǥ�� x��ǥ�� �̿��ؼ� ������ ���Ѵ�.
        // �ٸ� �� �� ������ ������ ������ ���� �����̱⿡
        // �̰� ���� ������� Mathf.Rad2Deg�� �����ش�.
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        // ������ ����� �͵��� �̿��� z���� ȸ�������ش�.
        transform.rotation = Quaternion.Euler(0, 0, degree);

        // ���� ���� ����� ������ ����
        if (attackTarget != null)
        {
            Vector3 dir = attackTarget.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // Y�� �ø� ����: �Ʒ����� ���� ���� ����
            if (angle > 90 || angle < -90)
            {
                transform.localScale = new Vector3(1, -1, 1); // ������ �� ����
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    /// <summary>
    /// ���͸� ã�Ƴ��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator SearchTarget()
    {
        // �ݺ����� ���� ���� �����Ѵ�.
        while (true)
        {
            // ���� �����̿� �ִ� ���� ã�� ���� ó�� �Ÿ��� ���Ѵ�� ����
            float closestDistSqr = Mathf.Infinity;

            // �ݺ���. ���� �ʿ� �ִ� ��� ���͸� �˻��Ѵ�.
            for(int i = 0; i< monsterSpawner.MonsterList.Count; i++)
            {
                // ���� �ʿ� �ִ� ���� �ڽ��� �Ÿ��� ����Ѵ�.
                float distance = Vector3.Distance(monsterSpawner.MonsterList[i].transform.position, transform.position);
                // ���� ������ ����� �Ÿ��� ���� ���� �ȿ� �ְ� ���� �˻��� ������ ������ ����
                if(distance <= attackRange && distance <= closestDistSqr)
                {
                    // ���� �ű⸦ closestDistSqr�� ����
                    closestDistSqr = distance;
                    // ���ǿ� �´� ���� ���� ������� ����
                    attackTarget = monsterSpawner.MonsterList[i].transform;
                }
            }

            // ���� ���� ����� ������ ����
            if (attackTarget != null)
            {
                // ���¸� ���� ���·� ����
                ChangeState(TowerState.AttackTarget);
            }

            yield return null;
        }
    }

    /// <summary>
    /// ���� �����ϴ� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackTarget()
    {
        // �ݺ����� ���� ���� ����
        while (true)
        {
            // ���� ���� ����� ���ٸ� ����
            if(attackTarget == null)
            {
                // ���¸� �� Ž�� ���·� �ٲٰ�
                ChangeState(TowerState.SearchTarget);
                // �ݺ����� �������´�.
                break;
            }

            // ���Ϳ� Ÿ���� �Ÿ��� ����Ѵ�.
            float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
            // ���� �� �Ÿ��� ���� ���� �ۿ� �ִٸ� ����
            if (distance > attackRange)
            {
                // ���� ����� ���� ������ ����
                attackTarget = null;
                // ���¸� �� Ž�� ���·� �ٲ۴�.
                ChangeState(TowerState.SearchTarget);
                // �ݺ����� ���� ���´�.
                break;
            }

            // ���� ��Ÿ�Ӹ�ŭ ����ߴٰ� ����
            yield return new WaitForSeconds(attackRate);

            SpawnBullet();
        }
    }

    /// <summary>
    /// �߻�ü�� ��ȯ�ϴ� �Լ�
    /// </summary>
    private void SpawnBullet()
    {
        // �߻�ü ����
        // �߻�ü��, ���������ǿ� ȸ�� ���� ��ȯ
        GameObject clone = Instantiate(bullet, spawnPosition.position, Quaternion.identity);
        // ������ �߻�ü���� ���ݴ���� ������ ����
        clone.GetComponent<Bullet>().Setup(attackTarget, attackDamage);
    }
}
