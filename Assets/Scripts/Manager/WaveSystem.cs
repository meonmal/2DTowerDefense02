using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    /// <summary>
    /// 웨이브의 모든 정보
    /// </summary>
    [SerializeField]
    private Wave[] wave;
    /// <summary>
    /// 몬스터 스포너
    /// </summary>
    [SerializeField]
    private MonsterSpawner monsterSpawner;
    /// <summary>
    /// 현재 웨이브 인덱스
    /// </summary>
    private int currentWaveIndex = -1;

    /// <summary>
    /// 읽기 전용 프로퍼티
    /// 시작이 0이기에 +1을 해준다.
    /// 현재 웨이브는 현재 웨이브 인덱스 + 1이다.
    /// </summary>
    public int CurrentWave => currentWaveIndex + 1;
    /// <summary>
    /// 읽기 전용 프로퍼티
    /// 웨이브가 몇개인지 나타낸다.
    /// </summary>
    public int MaxWave => wave.Length;

    /// <summary>
    /// 웨이브를 시작하는 함수
    /// </summary>
    public void StartWave()
    {
        // 현재 맵에 몬스터가 없고 아직 진행해야 할 웨이브가 남아 있으면 실행
        if(monsterSpawner.MonsterList.Count == 0 && currentWaveIndex < wave.Length -1)
        {
            // 현재 웨이브 인덱스를 +1해준다.
            currentWaveIndex++;
            // 몬스터 스포너의 StartWave() 함수를 호출해준다.
            // 더해서 현재 웨이브 정보를 제공.
            monsterSpawner.StartWave(wave[currentWaveIndex]);
        }
    }
}

/// <summary>
/// [System.Serializable]은 구조체를 직렬화하는 것이다.
/// 이렇게 하면 클래스 내부의 변수 정보들을 인스펙터창에서 수정할 수 있다.
/// </summary>
[System.Serializable]
public struct Wave
{
    /// <summary>
    /// 소환 주기
    /// </summary>
    public float spawnTime;
    /// <summary>
    /// 이번 웨이브에서 나올 몬스터의 최대수
    /// </summary>
    public int maxMonsterSpawnCount;
    /// <summary>
    /// 이번 웨이브에서 나올 몬스터의 종류
    /// </summary>
    public GameObject[] monsters;
}
