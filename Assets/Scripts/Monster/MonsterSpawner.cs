using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    ///// <summary>
    ///// 적 프리팹
    ///// </summary>
    //[SerializeField]
    //private GameObject monsterPrefab;
    /// <summary>
    /// Slider 프리팹
    /// </summary>
    [SerializeField]
    private GameObject monsterSlider;
    /// <summary>
    /// canvas 오브젝트의 Transform
    /// </summary>
    [SerializeField]
    private Transform canvasTransform;
    /// <summary>
    /// 이동 경로 정보
    /// </summary>
    [SerializeField]
    private Transform[] wayPoints;
    ///// <summary>
    ///// 소환 시간
    ///// </summary>
    //[SerializeField]
    //private float spawnTime;
    /// <summary>
    /// 플레이어의 체력 컴포넌트
    /// </summary>
    [SerializeField]
    private PlayerHP playerHP;
    /// <summary>
    /// 플레이어의 골드 컴포넌트
    /// </summary>
    [SerializeField]
    private PlayerGold playerGold;
    /// <summary>
    /// 현재 웨이브 정보
    /// </summary>
    private Wave currentWave;
    /// <summary>
    /// 현재 남아있는 몬스터의 숫자
    /// </summary>
    private int currentMonsterCount;

    /// <summary>
    /// 현재 생성되어 존재하는 모든 몬스터의 정보
    /// </summary>
    private List<Monster> monsterList;
    /// <summary>
    /// 읽기 전용 프로퍼티.
    /// 생성 및 삭제는 어짜피 이 스크립트에서 하기에 set은 필요 없다.
    /// </summary>
    public List<Monster> MonsterList => monsterList;
    public int CurrentMonsterCount => currentMonsterCount;
    public int MaxMonsterCount => currentWave.maxMonsterSpawnCount;

    /// <summary>
    /// 게임 씬이 로드 되고 바로 호출되는 함수
    /// </summary>
    private void Awake()
    {
        // 적 리스트를 메모리에 할당해준다.
        monsterList = new List<Monster>();

        // StartCoroutine(SpawnMonster());
    }

    /// <summary>
    /// 웨이브를 시작하는 함수
    /// </summary>
    /// <param name="wave"></param>
    public void StartWave(Wave wave)
    {
        // 매개변수로 받아온 웨이브의 정보를 저장
        currentWave = wave;
        currentMonsterCount = currentWave.maxMonsterSpawnCount;
        // 몬스터 소환 시작
        StartCoroutine(SpawnMonster());
    }

    /// <summary>
    /// 몬스터를 소환하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnMonster()
    {
        // 현재 웨이브에서 생성한 적의 숫자
        int spawnMonsterCount = 0;

        // 반복문을 조건 없이 실행
        //while (true)
        //{
        //    // 적 오브젝트를 생성
        //    GameObject clone = Instantiate();
        //    // 적 오브젝트에게 컴포넌트 붙여주기
        //    Monster monster = clone.GetComponent<Monster>();

        //    // monster의 Setup함수 호출(매개변수로 wayPoints 정보를 넣어준다)
        //    monster.Setup(this, wayPoints);
        //    // 리스트에 방금 생성된 몬스터의 정보를 저장한다.
        //    monsterList.Add(monster);

        //    SpawnMonsterSlider(clone);

        //    // spawnTime만큼 대기했다가 실행
        //    yield return new WaitForSeconds(spawnTime);
        //}

        // 현재 웨이브에서 생성한 적의 숫자보다 이번 웨이브에서 생성해야 할 적의 수가 더 많다면 실행
        while(spawnMonsterCount < currentWave.maxMonsterSpawnCount)
        {
            // 현재 웨이브에서 등장하는 몬스터가 여러 마리일 경우
            // 랜덤으로 나오도록 설정한다.
            int monsterIndex = Random.Range(0, currentWave.monsters.Length);
            // 현재 웨이브의 최대 몬스터의 갯수만큼 생성
            GameObject clone = Instantiate(currentWave.monsters[monsterIndex]);
            // 방금 생성된 적에게 컴포넌트 붙여주기.
            Monster monster = clone.GetComponent<Monster>();

            // wayPoints를 매개변수로 Setup()함수 호출
            monster.Setup(this, wayPoints);
            // 몬스터 리스트에 방금 생성된 몬스터의 정보를 저장한다.
            monsterList.Add(monster);

            SpawnMonsterSlider(clone);

            // 현재 웨이브에서 생성된 적의 숫자를 +1 해준다.
            spawnMonsterCount++;

            // 현재 웨이브의 소환시간 만큼 대기하고 실행
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }

    /// <summary>
    /// 몬스터가 죽을 때 실행되는 함수
    /// </summary>
    /// <param name="monster"></param>
    public void DestroyMonster(MonsterDieType type, Monster monster, int gold)
    {
        // 만약 몬스터가 완주를 해서 죽었다면 실행
        if(type == MonsterDieType.Arrive)
        {
            // 플레이어의 현재 체력을 1 줄인다.
            playerHP.TakeDamage(1);
        }
        // 적이 플레이어에게 죽었으면 실행
        else if(type == MonsterDieType.Kill)
        {
            // 현재 골드에서 사망시 획득하는 골드를 추가한다.
            playerGold.CurrentGold += gold;
        }

        currentMonsterCount--;
        // 죽은 몬스터를 몬스터 리스트에서 삭제한다.
        monsterList.Remove(monster);

        // 몬스터 게임 오브젝트 삭제
        Destroy(monster.gameObject);
    }

    /// <summary>
    /// MonsterSlider를 생성하는 함수
    /// </summary>
    /// <param name="monster"></param>
    private void SpawnMonsterSlider(GameObject monster)
    {
        // 먼저 슬라이더를 생성한다.
        GameObject slider = Instantiate(monsterSlider);
        // 그 다음 슬라이드를 캔버스의 자식으로 설정한다.
        // 캔버스의 자식으로 있어야만 화면에 보인다.
        slider.transform.SetParent(canvasTransform);
        // 캔버스때문에 크기가 이상해지는 것을 방지한다.(1,1,1)로 설정
        slider.transform.localScale = Vector3.one;

        // 슬라이더가 쫓아다니 대상을 몬스터로 설정
        slider.GetComponent<MonsterSliderPosition>().Setup(monster.transform);
        // 슬라이더의 몬스터의 HP를 표시하도록 설정한다.
        slider.GetComponent<MonsterSlider>().Setup(monster.GetComponent<MonsterHP>());
    }
}
