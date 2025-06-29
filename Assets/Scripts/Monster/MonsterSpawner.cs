using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    ///// <summary>
    ///// �� ������
    ///// </summary>
    //[SerializeField]
    //private GameObject monsterPrefab;
    /// <summary>
    /// Slider ������
    /// </summary>
    [SerializeField]
    private GameObject monsterSlider;
    /// <summary>
    /// canvas ������Ʈ�� Transform
    /// </summary>
    [SerializeField]
    private Transform canvasTransform;
    /// <summary>
    /// �̵� ��� ����
    /// </summary>
    [SerializeField]
    private Transform[] wayPoints;
    ///// <summary>
    ///// ��ȯ �ð�
    ///// </summary>
    //[SerializeField]
    //private float spawnTime;
    /// <summary>
    /// �÷��̾��� ü�� ������Ʈ
    /// </summary>
    [SerializeField]
    private PlayerHP playerHP;
    /// <summary>
    /// �÷��̾��� ��� ������Ʈ
    /// </summary>
    [SerializeField]
    private PlayerGold playerGold;
    /// <summary>
    /// ���� ���̺� ����
    /// </summary>
    private Wave currentWave;
    /// <summary>
    /// ���� �����ִ� ������ ����
    /// </summary>
    private int currentMonsterCount;

    /// <summary>
    /// ���� �����Ǿ� �����ϴ� ��� ������ ����
    /// </summary>
    private List<Monster> monsterList;
    /// <summary>
    /// �б� ���� ������Ƽ.
    /// ���� �� ������ ��¥�� �� ��ũ��Ʈ���� �ϱ⿡ set�� �ʿ� ����.
    /// </summary>
    public List<Monster> MonsterList => monsterList;
    public int CurrentMonsterCount => currentMonsterCount;
    public int MaxMonsterCount => currentWave.maxMonsterSpawnCount;

    /// <summary>
    /// ���� ���� �ε� �ǰ� �ٷ� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Awake()
    {
        // �� ����Ʈ�� �޸𸮿� �Ҵ����ش�.
        monsterList = new List<Monster>();

        // StartCoroutine(SpawnMonster());
    }

    /// <summary>
    /// ���̺긦 �����ϴ� �Լ�
    /// </summary>
    /// <param name="wave"></param>
    public void StartWave(Wave wave)
    {
        // �Ű������� �޾ƿ� ���̺��� ������ ����
        currentWave = wave;
        currentMonsterCount = currentWave.maxMonsterSpawnCount;
        // ���� ��ȯ ����
        StartCoroutine(SpawnMonster());
    }

    /// <summary>
    /// ���͸� ��ȯ�ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnMonster()
    {
        // ���� ���̺꿡�� ������ ���� ����
        int spawnMonsterCount = 0;

        // �ݺ����� ���� ���� ����
        //while (true)
        //{
        //    // �� ������Ʈ�� ����
        //    GameObject clone = Instantiate();
        //    // �� ������Ʈ���� ������Ʈ �ٿ��ֱ�
        //    Monster monster = clone.GetComponent<Monster>();

        //    // monster�� Setup�Լ� ȣ��(�Ű������� wayPoints ������ �־��ش�)
        //    monster.Setup(this, wayPoints);
        //    // ����Ʈ�� ��� ������ ������ ������ �����Ѵ�.
        //    monsterList.Add(monster);

        //    SpawnMonsterSlider(clone);

        //    // spawnTime��ŭ ����ߴٰ� ����
        //    yield return new WaitForSeconds(spawnTime);
        //}

        // ���� ���̺꿡�� ������ ���� ���ں��� �̹� ���̺꿡�� �����ؾ� �� ���� ���� �� ���ٸ� ����
        while(spawnMonsterCount < currentWave.maxMonsterSpawnCount)
        {
            // ���� ���̺꿡�� �����ϴ� ���Ͱ� ���� ������ ���
            // �������� �������� �����Ѵ�.
            int monsterIndex = Random.Range(0, currentWave.monsters.Length);
            // ���� ���̺��� �ִ� ������ ������ŭ ����
            GameObject clone = Instantiate(currentWave.monsters[monsterIndex]);
            // ��� ������ ������ ������Ʈ �ٿ��ֱ�.
            Monster monster = clone.GetComponent<Monster>();

            // wayPoints�� �Ű������� Setup()�Լ� ȣ��
            monster.Setup(this, wayPoints);
            // ���� ����Ʈ�� ��� ������ ������ ������ �����Ѵ�.
            monsterList.Add(monster);

            SpawnMonsterSlider(clone);

            // ���� ���̺꿡�� ������ ���� ���ڸ� +1 ���ش�.
            spawnMonsterCount++;

            // ���� ���̺��� ��ȯ�ð� ��ŭ ����ϰ� ����
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }

    /// <summary>
    /// ���Ͱ� ���� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="monster"></param>
    public void DestroyMonster(MonsterDieType type, Monster monster, int gold)
    {
        // ���� ���Ͱ� ���ָ� �ؼ� �׾��ٸ� ����
        if(type == MonsterDieType.Arrive)
        {
            // �÷��̾��� ���� ü���� 1 ���δ�.
            playerHP.TakeDamage(1);
        }
        // ���� �÷��̾�� �׾����� ����
        else if(type == MonsterDieType.Kill)
        {
            // ���� ��忡�� ����� ȹ���ϴ� ��带 �߰��Ѵ�.
            playerGold.CurrentGold += gold;
        }

        currentMonsterCount--;
        // ���� ���͸� ���� ����Ʈ���� �����Ѵ�.
        monsterList.Remove(monster);

        // ���� ���� ������Ʈ ����
        Destroy(monster.gameObject);
    }

    /// <summary>
    /// MonsterSlider�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="monster"></param>
    private void SpawnMonsterSlider(GameObject monster)
    {
        // ���� �����̴��� �����Ѵ�.
        GameObject slider = Instantiate(monsterSlider);
        // �� ���� �����̵带 ĵ������ �ڽ����� �����Ѵ�.
        // ĵ������ �ڽ����� �־�߸� ȭ�鿡 ���δ�.
        slider.transform.SetParent(canvasTransform);
        // ĵ���������� ũ�Ⱑ �̻������� ���� �����Ѵ�.(1,1,1)�� ����
        slider.transform.localScale = Vector3.one;

        // �����̴��� �Ѿƴٴ� ����� ���ͷ� ����
        slider.GetComponent<MonsterSliderPosition>().Setup(monster.transform);
        // �����̴��� ������ HP�� ǥ���ϵ��� �����Ѵ�.
        slider.GetComponent<MonsterSlider>().Setup(monster.GetComponent<MonsterHP>());
    }
}
