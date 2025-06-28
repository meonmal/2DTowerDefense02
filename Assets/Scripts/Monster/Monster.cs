using System.Collections;
using UnityEngine;

public enum MonsterDieType { Kill = 0, Arrive}
public class Monster : MonoBehaviour
{
    /// <summary>
    /// wayPoint 갯수
    /// </summary>
    private int wayPointsCount;
    /// <summary>
    /// wayPoint 정보
    /// </summary>
    private Transform[] wayPoints;
    /// <summary>
    /// 현재 목표 지점 인덱스
    /// </summary>
    private int currentIndex;
    /// <summary>
    /// 오브젝트 이동 제어
    /// </summary>
    private Movement movement;
    /// <summary>
    /// 몬스터 스포너
    /// 몬스터의 삭제를 몬스터 본인이 하지 않고 몬스터 스포너에 알려서 삭제한다.
    /// 몬스터 리스트가 몬스터 스포너에게 있기 때문이다.
    /// </summary>
    private MonsterSpawner monsterSpawner;
    /// <summary>
    /// 몬스터가 죽으면 획득하는 골드
    /// </summary>
    [SerializeField]
    private int gold = 50;

    /// <summary>
    /// 이동하기 전 세팅을 하는 함수
    /// </summary>
    /// <param name="wayPoints">이동해야할 지점</param>
    public void Setup(MonsterSpawner monsterSpawner, Transform[] wayPoints)
    {
        // movement에 컴포넌트 가져오기
        movement = GetComponent<Movement>();
        this.monsterSpawner = monsterSpawner;

        // 적 이동 경로 wayPoints 정보 설정
        // wayPointsCount는 wayPoints의 갯수다.
        wayPointsCount = wayPoints.Length;
        // wayPoints에 메모리를 할당하는데 그 크기는 wayPointsCount이다.
        this.wayPoints = new Transform[wayPointsCount];
        // wayPoints의 값으로 같은 이름의 매개변수인 wayPoints를 씀
        this.wayPoints = wayPoints;

        // 적의 첫 위치는 첫번째 wayPoints의 위치로 설정
        transform.position = wayPoints[0].position;

        StartCoroutine(HasReachedGoal());
    }

    /// <summary>
    /// 현재 목표 지점까지 이동을 다 했는지 확인하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator HasReachedGoal()
    {
        NextMove();

        // 반복문을 조건 없이 실행
        while (true)
        {
            // 만약 현재 위치가 현재 목표 지점까지의 거리가 0.02이하일 때 실행
            // * movement.MoveSpeed를 하는 이유는 이동속도가 너무 빠르면 한 프레임에
            // 0.02보다 더 크게 움직일 수 있기 때문임.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement.MoveSpeed)
            {
                NextMove();
            }

            yield return null;
        }
    }

    /// <summary>
    /// 다음 이동 방향을 설정하는 함수
    /// </summary>
    private void NextMove()
    {
        // 만약 아직 이동해야 할 wayPoint가 남아 있으면 실행
        if(currentIndex < wayPointsCount -1)
        {
            // 오브젝트의 위치를 현재 목표 위치로 설정
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            // 이동해야 할 방향을 다음 목표 지점의 방향으로 설정
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement.MoveDir(direction);
        }
        // 만약 다음으로 이동해야 할 wayPoint가 없다면 실행
        else
        {
            // 적이 골 지점에 도착해서 죽은 것이라면 획득 가능한 골드를 0으로 만든다.
            gold = 0;
            // 게임 오브젝트 삭제
            // Destroy(gameObject);
            OnDie(MonsterDieType.Arrive);
        }
    }

    /// <summary>
    /// 몬스터가 죽을 때 실행되는 함수
    /// </summary>
    /// <param name="type">몬스터가 죽는 방법</param>
    public void OnDie(MonsterDieType type)
    {
        monsterSpawner.DestroyMonster(type, this, gold);
    }
}
