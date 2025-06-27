using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
    /// ���� �����Ǿ� �����ϴ� ��� ������ ����
    /// </summary>
    private List<Monster> monsterList;

    /// <summary>
    /// �б� ���� ������Ƽ.
    /// ���� �� ������ ��¥�� �� ��ũ��Ʈ���� �ϱ⿡ set�� �ʿ� ����.
    /// </summary>
    public List<Monster> MonsterList => monsterList;

    /// <summary>
    /// ���� ���� �ε� �ǰ� �ٷ� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Awake()
    {
        // �� ����Ʈ�� �޸𸮿� �Ҵ����ش�.
        monsterList = new List<Monster>();

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
            monster.Setup(this, wayPoints);
            // ����Ʈ�� ��� ������ ������ ������ �����Ѵ�.
            monsterList.Add(monster);

            // spawnTime��ŭ ����ߴٰ� ����
            yield return new WaitForSeconds(spawnTime);
        }
    }

    /// <summary>
    /// ���Ͱ� ���� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="monster"></param>
    public void DestroyMonster(Monster monster)
    {
        // ���� ���͸� ���� ����Ʈ���� �����Ѵ�.
        monsterList.Remove(monster);

        // ���� ���� ������Ʈ ����
        Destroy(monster.gameObject);
    }
}
