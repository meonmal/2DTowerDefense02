using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    /// <summary>
    /// 적 프리팹
    /// </summary>
    [SerializeField]
    private GameObject monsterPrefab;
    /// <summary>
    /// 이동 경로 정보
    /// </summary>
    [SerializeField]
    private Transform[] wayPoints;
    /// <summary>
    /// 소환 시간
    /// </summary>
    [SerializeField]
    private float spawnTime;
    /// <summary>
    /// 현재 생성되어 존재하는 모든 몬스터의 정보
    /// </summary>
    private List<Monster> monsterList;

    /// <summary>
    /// 읽기 전용 프로퍼티.
    /// 생성 및 삭제는 어짜피 이 스크립트에서 하기에 set은 필요 없다.
    /// </summary>
    public List<Monster> MonsterList => monsterList;

    /// <summary>
    /// 게임 씬이 로드 되고 바로 호출되는 함수
    /// </summary>
    private void Awake()
    {
        // 적 리스트를 메모리에 할당해준다.
        monsterList = new List<Monster>();

        StartCoroutine(SpawnMonster());
    }

    /// <summary>
    /// 몬스터를 소환하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnMonster()
    {
        // 반복문을 조건 없이 실행
        while (true)
        {
            // 적 오브젝트를 생성
            GameObject clone = Instantiate(monsterPrefab);
            // 적 오브젝트에게 컴포넌트 붙여주기
            Monster monster = clone.GetComponent<Monster>();

            // monster의 Setup함수 호출(매개변수로 wayPoints 정보를 넣어준다)
            monster.Setup(this, wayPoints);
            // 리스트에 방금 생성된 몬스터의 정보를 저장한다.
            monsterList.Add(monster);

            // spawnTime만큼 대기했다가 실행
            yield return new WaitForSeconds(spawnTime);
        }
    }

    /// <summary>
    /// 몬스터가 죽을 때 실행되는 함수
    /// </summary>
    /// <param name="monster"></param>
    public void DestroyMonster(Monster monster)
    {
        // 죽은 몬스터를 몬스터 리스트에서 삭제한다.
        monsterList.Remove(monster);

        // 몬스터 게임 오브젝트 삭제
        Destroy(monster.gameObject);
    }
}
