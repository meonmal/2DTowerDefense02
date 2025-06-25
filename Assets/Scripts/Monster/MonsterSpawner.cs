using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    /// <summary>
    /// �� ������
    /// </summary>
    [SerializeField]
    private GameObject monsterPrefab;
    /// <summary>
    /// �̵� ��� ����
    /// </summary>
    [SerializeField]
    private Transform[] wayPoints;
    /// <summary>
    /// ��ȯ �ð�
    /// </summary>
    [SerializeField]
    private float spawnTime;

    /// <summary>
    /// ���� ���� �ε� �ǰ� �ٷ� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Awake()
    {
        StartCoroutine(SpawnMonster());
    }

    /// <summary>
    /// ���͸� ��ȯ�ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnMonster()
    {
        // �ݺ����� ���� ���� ����
        while (true)
        {
            // �� ������Ʈ�� ����
            GameObject clone = Instantiate(monsterPrefab);
            // �� ������Ʈ���� ������Ʈ �ٿ��ֱ�
            Monster monster = clone.GetComponent<Monster>();

            // monster�� Setup�Լ� ȣ��(�Ű������� wayPoints ������ �־��ش�)
            monster.Setup(wayPoints);

            // spawnTime��ŭ ����ߴٰ� ����
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
