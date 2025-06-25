using System.Collections;
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
    /// 게임 씬이 로드 되고 바로 호출되는 함수
    /// </summary>
    private void Awake()
    {
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
            monster.Setup(wayPoints);

            // spawnTime만큼 대기했다가 실행
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
