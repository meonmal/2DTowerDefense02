using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 타워의 종류
/// </summary>
public enum TowerType { Cannon = 0, Laser, Slow, Buff, }
/// <summary>
/// SearchTarget = 적을 찾는 상태
/// AttackTarget = 공격 상태
/// SlowTower는 공격을 안 하기에 아래의 상태가 필요 없다. 그냥 상시 발동임.
/// </summary>
public enum TowerState { SearchTarget = 0, AttackCannon, AttackLaser, }
public class TowerWeapon : MonoBehaviour
{

    [Header("Commons")]
    /// <summary>
    /// 타워 정보를 가져오기 위한 변수
    /// </summary>
    [SerializeField]
    private TowerTemplate towerTemplate;
    /// <summary>
    /// 발사체 소환 위치
    /// </summary>
    [SerializeField]
    private Transform spawnPosition;
    /// <summary>
    /// 타워의 종류
    /// </summary>
    [SerializeField]
    private TowerType towerType;


    [Header("Cannon")]
    /// <summary>
    /// 발사체 프리팹
    /// </summary>
    [SerializeField]
    private GameObject bullet;

    [Header("Laser")]
    /// <summary>
    /// 레이저로 사용되는 선
    /// </summary>
    [SerializeField]
    private LineRenderer lineRenderer;
    /// <summary>
    /// 타격 효과
    /// </summary>
    [SerializeField]
    private Transform hitEffect;
    /// <summary>
    /// 광선에 부딪히는 레이어 설정
    /// </summary>
    [SerializeField]
    private LayerMask targetLayer;

    /// <summary>
    /// 타워의 레벨
    /// </summary>
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
    /// 타워 스포너
    /// </summary>
    private TowerSpawner towerSpawner;
    /// <summary>
    /// 공격 대상을 찾기 위한 변수(리스트를 통해 찾음)
    /// </summary>
    private MonsterSpawner monsterSpawner;
    /// <summary>
    /// 플레이어의 골드 정보
    /// </summary>
    private PlayerGold playerGold;
    /// <summary>
    /// 현재 타워가 배치되어 있는 타일
    /// </summary>
    private Tile ownerTile;

    /// <summary>
    /// 버프에 의해 추가된 공격력
    /// </summary>
    private float addedDamage;
    /// <summary>
    /// 버프를 받는지 여부 설정(0 : 버프x, 1~3 : 받는 버프 레벨)
    /// </summary>
    private int buffLevel;

    public Sprite TowerSprite => towerTemplate.sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int UgradeCost => Level < MaxLevel ? towerTemplate.weapon[level + 1].cost : 0;
    public int SellCost => towerTemplate.weapon[level].sell;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public float Slow => towerTemplate.weapon[level].slow;
    public float Buff => towerTemplate.weapon[level].buff;
    public TowerType TowerType => towerType;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int BuffLevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }

    

    /// <summary>
    /// 처음의 상태를 세팅하는 함수
    /// </summary>
    /// <param name="monsterSpawner"></param>
    public void Setup(TowerSpawner towerSpawner, MonsterSpawner monsterSpawner, PlayerGold playerGold, Tile ownerTile)
    {
        this.towerSpawner = towerSpawner;
        this.monsterSpawner = monsterSpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        // 무기의 종류가 캐논, 레이저일 때 실행
        if(towerType == TowerType.Cannon || towerType == TowerType.Laser)
        {
            // 최초의 상태를 적 탐지 상태로 한다.
            ChangeState(TowerState.SearchTarget);
        }
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
                GetComponent<SpriteRenderer>().flipY = true; // 뒤집힌 거 방지
            }
            else
            {
                GetComponent<SpriteRenderer>().flipY = false;
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
            //// 제일 가까이에 있는 적을 찾기 위해 처음 거리를 무한대로 설정
            //float closestDistSqr = Mathf.Infinity;

            //// 반복문. 현재 맵에 있는 모든 몬스터를 검사한다.
            //for(int i = 0; i< monsterSpawner.MonsterList.Count; i++)
            //{
            //    // 현재 맵에 있는 적과 자신의 거리를 계산한다.
            //    float distance = Vector3.Distance(monsterSpawner.MonsterList[i].transform.position, transform.position);
            //    // 만약 위에서 계산한 거리가 공격 범위 안에 있고 현재 검사한 적보다 가까우면 실행
            //    if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            //    {
            //        // 현재 거기를 closestDistSqr에 저장
            //        closestDistSqr = distance;
            //        // 조건에 맞는 적을 공격 대상으로 설정
            //        attackTarget = monsterSpawner.MonsterList[i].transform;
            //    }
            //}

            // 현재 타워에서 가장 가까이 있는 몬스터를 탐색한다.
            attackTarget = FindClosestAttackTarget();

            // 만약 공격 대상이 있으면 실행
            if (attackTarget != null)
            {
                // 상태를 공격 상태로 변경
                if(towerType == TowerType.Cannon)
                {
                    ChangeState(TowerState.AttackCannon);
                }
                else if(towerType == TowerType.Laser)
                {
                    ChangeState(TowerState.AttackLaser);
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// 적을 공격하는 상태
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCannon()
    {
        // 반복문을 조건 없이 실행
        while (true)
        {
            //// 만약 공격 대상이 없다면 실행
            //if(attackTarget == null)
            //{
            //    // 상태를 적 탐색 상태로 바꾸고
            //    ChangeState(TowerState.SearchTarget);
            //    // 반복문을 빠져나온다.
            //    break;
            //}

            //// 몬스터와 타워의 거리를 계산한다.
            //float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
            //// 만약 그 거리가 공격 범위 밖에 있다면 실행
            //if (distance > towerTemplate.weapon[level].range)
            //{
            //    // 공격 대상은 없는 것으로 설정
            //    attackTarget = null;
            //    // 상태를 적 탐색 상태로 바꾼다.
            //    ChangeState(TowerState.SearchTarget);
            //    // 반복문을 빠져 나온다.
            //    break;
            //}

            // 공격 대상이 없다면
            if(IsPossibleToAttackTarget() == false)
            {
                // 적 탐지 상태로 전환
                ChangeState(TowerState.SearchTarget);
                break;
            }

            // 공격 쿨타임만큼 대기했다가 실행
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            SpawnBullet();
        }
    }

    /// <summary>
    /// 레이저 포탑이 공격하게 하는 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackLaser()
    {
        LaserOn();

        // 반복문을 조건 없이 실행
        while (true)
        {
            // 만약 공격할 수 있는 대상이 없다면 실행
            if(IsPossibleToAttackTarget() == false)
            {
                // 레이저를 생성하지 마라.
                LaserOff();
                // 타워의 상태를 적 탐지 상태로 바꾼다.
                ChangeState(TowerState.SearchTarget); 
                break;
            }

            SpawnLaser();

            yield return null;
        }
    }

    public void OnBuffAroundTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i=0; i<towers.Length; i++)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if(weapon.BuffLevel > Level)
            {
                continue;
            }

            if (Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                if(weapon.TowerType == TowerType.Cannon || weapon.TowerType == TowerType.Laser)
                {
                    weapon.AddedDamage = weapon.Damage * (towerTemplate.weapon[level].buff);
                    weapon.BuffLevel = Level;
                }
            }
        }
    }

    /// <summary>
    /// 현재 타워에서 가장 가까이 있는 공격 대상을 찾는 함수
    /// </summary>
    /// <returns></returns>
    private Transform FindClosestAttackTarget()
    {
        // 제일 가까이에 있는 적을 찾기 위해 처음 거리를 최대한 크게 설정
        float closestDistSqr = Mathf.Infinity;

        // 몬스터 스포너에 있는 모든 적을 검사하고
        for(int i =0; i<monsterSpawner.MonsterList.Count; i++)
        {
            // 현재 포탑과 적의 거리를 계산한다.
            float distance = Vector3.Distance(monsterSpawner.MonsterList[i].transform.position, transform.position);

            // 만약 위에서 계산한 거리가 공격 범위 안에 있고 다른 적들보다 가장 가까우면 실행
            if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                // 가장 가까운 적을 저장하고
                closestDistSqr = distance;
                // 그 적을 공격 대상으로 설정
                attackTarget = monsterSpawner.MonsterList[i].transform;
            }
        }

        return attackTarget;
    }

    /// <summary>
    /// 공격 대상이 있는지 검사하는 함수
    /// </summary>
    /// <returns></returns>
    private bool IsPossibleToAttackTarget()
    {
        // 공격 대상이 없으면
        if(attackTarget == null)
        {
            // 결과값은 false로 함수 종료
            return false;
        }

        // 공격 대상과 포탑의 거리를 계산한다.
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        // 계산한 거리가 타워의 공격 범위 밖에 있다면 실행
        if(distance > towerTemplate.weapon[level].range)
        {
            // 공격 대상은 없는 것으로 설정하고
            attackTarget = null;
            // 결과값은 false로 함수 종료
            return false;
        }

        // 위의 조건에 걸리지 않으면 공격 가능상태.
        return true;
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
        float damage = towerTemplate.weapon[level].damage + AddedDamage;
        clone.GetComponent<Bullet>().Setup(attackTarget, damage);
    }

    private void LaserOn()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }

    private void LaserOff()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPosition.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPosition.position, direction, towerTemplate.weapon[level].range, targetLayer);

        for(int i = 0; i< hit.Length; i++)
        {
            if (hit[i].transform == attackTarget)
            {
                lineRenderer.SetPosition(0, spawnPosition.position);
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                hitEffect.position = hit[i].point;
                float damage = towerTemplate.weapon[level].damage + AddedDamage;
                attackTarget.GetComponent<MonsterHP>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 타워의 레벨을 올리는 함수
    /// </summary>
    /// <returns></returns>
    public bool Upgrade()
    {
        // 만약 현재 플레이어가 가진 돈 보다 타워 업그레이드 비용이 더 높다면 실행
        if(playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            // 실행 ㄴ
            return false;
        }

        // 타워의 레벨을 올리고
        level++;
        // 현재 돈에서 타워의 레벨당 비용만큼 차감
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;


        if (towerType == TowerType.Laser)
        {
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        if(towerType == TowerType.Slow)
        {
            CircleCollider2D circleColl = GetComponentInChildren<CircleCollider2D>();
            circleColl.radius = towerTemplate.weapon[level].range;
        }

        towerSpawner.OnBuffAllBuffTowers();

        return true;
    }

    public void Sell()
    {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        ownerTile.isBuildTower = false;
        Destroy(gameObject);
    }
}
