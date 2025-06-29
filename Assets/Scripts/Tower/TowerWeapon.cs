using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// SearchTarget = 적을 찾는 상태
/// AttackTarget = 공격 상태
/// </summary>
public enum TowerState { SearchTarget = 0, AttackTarget, }
public class TowerWeapon : MonoBehaviour
{
    /// <summary>
    /// 발사체 프리팹
    /// </summary>
    [SerializeField]
    private GameObject bullet;
    /// <summary>
    /// 발사체 소환 위치
    /// </summary>
    [SerializeField]
    private Transform spawnPosition;
    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    [SerializeField]
    private float attackRate;
    /// <summary>
    /// 공격 범위
    /// </summary>
    [SerializeField]
    private float attackRange;
    /// <summary>
    /// 타워의 공격력
    /// </summary>
    [SerializeField]
    private float attackDamage;
    /// <summary>
    /// 타워의 레벨
    /// </summary>
    [SerializeField]
    private int level;
    /// <summary>
    /// 타워의 현재 상태
    /// </summary>
    private TowerState towerState = TowerState.SearchTarget;
    /// <summary>
    /// 공격 대상
    /// </summary>
    private Transform attackTarget = null;
    /// <summary>
    /// 공격 대상을 찾기 위한 변수(리스트를 통해 찾음)
    /// </summary>
    private MonsterSpawner monsterSpawner;

    public float Damage => attackDamage;
    public float Rate => attackRate;
    public float Range => attackRange;
    public int Level => level;

    /// <summary>
    /// 처음의 상태를 세팅하는 함수
    /// </summary>
    /// <param name="monsterSpawner"></param>
    public void Setup(MonsterSpawner monsterSpawner)
    {
        this.monsterSpawner = monsterSpawner;

        // 처음의 상태를 탐색 상태로 한다.
        ChangeState(TowerState.SearchTarget);
    }

    /// <summary>
    /// 타워의 상태를 변환하는 함수
    /// </summary>
    /// <param name="towerState">타워의 상태</param>
    public void ChangeState(TowerState newState)
    {
        // 이전의 상태를 멈추고
        StopCoroutine(towerState.ToString());
        // 상태를 변경해서
        towerState = newState;
        // 변경된 상태를 재생한다.
        StartCoroutine(towerState.ToString());
    }

    /// <summary>
    /// 매 프레임 마다 호출되는 함수
    /// </summary>
    private void Update()
    {
        // 만약 공격 대상이 없다면 실행
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }

    /// <summary>
    /// 타워가 적을 바라보도록 하는 함수
    /// </summary>
    private void RotateToTarget()
    {
        // 타워의 위치와 몬스터의 위치를 x, y 좌표로 계산해서 방향을 계산해준다.
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        // 위에서 구한 x축과 y축의 좌표를 계산해서 z축의 회전값을 구한다.
        // Atan2 함수는 y좌표와 x좌표를 이용해서 방향을 구한다.
        // 다만 이 때 구해진 방향의 단위는 라디안 단위이기에
        // 이걸 도로 계산해줄 Mathf.Rad2Deg를 곱해준다.
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        // 위에서 계산한 것들을 이용해 z축을 회전시켜준다.
        transform.rotation = Quaternion.Euler(0, 0, degree);

        // 만약 공격 대상이 있으면 실행
        if (attackTarget != null)
        {
            Vector3 dir = attackTarget.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // Y축 플립 방지: 아래쪽일 때는 각도 보정
            if (angle > 90 || angle < -90)
            {
                transform.localScale = new Vector3(1, -1, 1); // 뒤집힌 거 방지
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    /// <summary>
    /// 몬스터를 찾아내는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator SearchTarget()
    {
        // 반복문을 조건 없이 실행한다.
        while (true)
        {
            // 제일 가까이에 있는 적을 찾기 위해 처음 거리를 무한대로 설정
            float closestDistSqr = Mathf.Infinity;

            // 반복문. 현재 맵에 있는 모든 몬스터를 검사한다.
            for(int i = 0; i< monsterSpawner.MonsterList.Count; i++)
            {
                // 현재 맵에 있는 적과 자신의 거리를 계산한다.
                float distance = Vector3.Distance(monsterSpawner.MonsterList[i].transform.position, transform.position);
                // 만약 위에서 계산한 거리가 공격 범위 안에 있고 현재 검사한 적보다 가까우면 실행
                if(distance <= attackRange && distance <= closestDistSqr)
                {
                    // 현재 거기를 closestDistSqr에 저장
                    closestDistSqr = distance;
                    // 조건에 맞는 적을 공격 대상으로 설정
                    attackTarget = monsterSpawner.MonsterList[i].transform;
                }
            }

            // 만약 공격 대상이 있으면 실행
            if (attackTarget != null)
            {
                // 상태를 공격 상태로 변경
                ChangeState(TowerState.AttackTarget);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 적을 공격하는 상태
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackTarget()
    {
        // 반복문을 조건 없이 실행
        while (true)
        {
            // 만약 공격 대상이 없다면 실행
            if(attackTarget == null)
            {
                // 상태를 적 탐색 상태로 바꾸고
                ChangeState(TowerState.SearchTarget);
                // 반복문을 빠져나온다.
                break;
            }

            // 몬스터와 타워의 거리를 계산한다.
            float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
            // 만약 그 거리가 공격 범위 밖에 있다면 실행
            if (distance > attackRange)
            {
                // 공격 대상은 없는 것으로 설정
                attackTarget = null;
                // 상태를 적 탐색 상태로 바꾼다.
                ChangeState(TowerState.SearchTarget);
                // 반복문을 빠져 나온다.
                break;
            }

            // 공격 쿨타임만큼 대기했다가 실행
            yield return new WaitForSeconds(attackRate);

            SpawnBullet();
        }
    }

    /// <summary>
    /// 발사체를 소환하는 함수
    /// </summary>
    private void SpawnBullet()
    {
        // 발사체 생성
        // 발사체를, 스폰포지션에 회전 없이 소환
        GameObject clone = Instantiate(bullet, spawnPosition.position, Quaternion.identity);
        // 생성된 발사체에게 공격대상의 정보를 제공
        clone.GetComponent<Bullet>().Setup(attackTarget, attackDamage);
    }
}
