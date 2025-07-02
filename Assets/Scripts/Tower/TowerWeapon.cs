using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Ÿ���� ����
/// </summary>
public enum TowerType { Cannon = 0, Laser, Slow, Buff, }
/// <summary>
/// SearchTarget = ���� ã�� ����
/// AttackTarget = ���� ����
/// SlowTower�� ������ �� �ϱ⿡ �Ʒ��� ���°� �ʿ� ����. �׳� ��� �ߵ���.
/// </summary>
public enum TowerState { SearchTarget = 0, AttackCannon, AttackLaser, }
public class TowerWeapon : MonoBehaviour
{

    [Header("Commons")]
    /// <summary>
    /// Ÿ�� ������ �������� ���� ����
    /// </summary>
    [SerializeField]
    private TowerTemplate towerTemplate;
    /// <summary>
    /// �߻�ü ��ȯ ��ġ
    /// </summary>
    [SerializeField]
    private Transform spawnPosition;
    /// <summary>
    /// Ÿ���� ����
    /// </summary>
    [SerializeField]
    private TowerType towerType;


    [Header("Cannon")]
    /// <summary>
    /// �߻�ü ������
    /// </summary>
    [SerializeField]
    private GameObject bullet;

    [Header("Laser")]
    /// <summary>
    /// �������� ���Ǵ� ��
    /// </summary>
    [SerializeField]
    private LineRenderer lineRenderer;
    /// <summary>
    /// Ÿ�� ȿ��
    /// </summary>
    [SerializeField]
    private Transform hitEffect;
    /// <summary>
    /// ������ �ε����� ���̾� ����
    /// </summary>
    [SerializeField]
    private LayerMask targetLayer;

    /// <summary>
    /// Ÿ���� ����
    /// </summary>
    private int level;
    /// <summary>
    /// Ÿ���� ���� ����
    /// </summary>
    private TowerState towerState = TowerState.SearchTarget;
    /// <summary>
    /// ���� ���
    /// </summary>
    private Transform attackTarget = null;
    /// <summary>
    /// Ÿ�� ������
    /// </summary>
    private TowerSpawner towerSpawner;
    /// <summary>
    /// ���� ����� ã�� ���� ����(����Ʈ�� ���� ã��)
    /// </summary>
    private MonsterSpawner monsterSpawner;
    /// <summary>
    /// �÷��̾��� ��� ����
    /// </summary>
    private PlayerGold playerGold;
    /// <summary>
    /// ���� Ÿ���� ��ġ�Ǿ� �ִ� Ÿ��
    /// </summary>
    private Tile ownerTile;

    /// <summary>
    /// ������ ���� �߰��� ���ݷ�
    /// </summary>
    private float addedDamage;
    /// <summary>
    /// ������ �޴��� ���� ����(0 : ����x, 1~3 : �޴� ���� ����)
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
    /// ó���� ���¸� �����ϴ� �Լ�
    /// </summary>
    /// <param name="monsterSpawner"></param>
    public void Setup(TowerSpawner towerSpawner, MonsterSpawner monsterSpawner, PlayerGold playerGold, Tile ownerTile)
    {
        this.towerSpawner = towerSpawner;
        this.monsterSpawner = monsterSpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        // ������ ������ ĳ��, �������� �� ����
        if(towerType == TowerType.Cannon || towerType == TowerType.Laser)
        {
            // ������ ���¸� �� Ž�� ���·� �Ѵ�.
            ChangeState(TowerState.SearchTarget);
        }
    }

    /// <summary>
    /// Ÿ���� ���¸� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="towerState">Ÿ���� ����</param>
    public void ChangeState(TowerState newState)
    {
        // ������ ���¸� ���߰�
        StopCoroutine(towerState.ToString());
        // ���¸� �����ؼ�
        towerState = newState;
        // ����� ���¸� ����Ѵ�.
        StartCoroutine(towerState.ToString());
    }

    /// <summary>
    /// �� ������ ���� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Update()
    {
        // ���� ���� ����� ���ٸ� ����
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }

    /// <summary>
    /// Ÿ���� ���� �ٶ󺸵��� �ϴ� �Լ�
    /// </summary>
    private void RotateToTarget()
    {
        // Ÿ���� ��ġ�� ������ ��ġ�� x, y ��ǥ�� ����ؼ� ������ ������ش�.
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        // ������ ���� x��� y���� ��ǥ�� ����ؼ� z���� ȸ������ ���Ѵ�.
        // Atan2 �Լ��� y��ǥ�� x��ǥ�� �̿��ؼ� ������ ���Ѵ�.
        // �ٸ� �� �� ������ ������ ������ ���� �����̱⿡
        // �̰� ���� ������� Mathf.Rad2Deg�� �����ش�.
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        // ������ ����� �͵��� �̿��� z���� ȸ�������ش�.
        transform.rotation = Quaternion.Euler(0, 0, degree);

        // ���� ���� ����� ������ ����
        if (attackTarget != null)
        {
            Vector3 dir = attackTarget.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // Y�� �ø� ����: �Ʒ����� ���� ���� ����
            if (angle > 90 || angle < -90)
            {
                GetComponent<SpriteRenderer>().flipY = true; // ������ �� ����
            }
            else
            {
                GetComponent<SpriteRenderer>().flipY = false;
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    /// <summary>
    /// ���͸� ã�Ƴ��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator SearchTarget()
    {
        // �ݺ����� ���� ���� �����Ѵ�.
        while (true)
        {
            //// ���� �����̿� �ִ� ���� ã�� ���� ó�� �Ÿ��� ���Ѵ�� ����
            //float closestDistSqr = Mathf.Infinity;

            //// �ݺ���. ���� �ʿ� �ִ� ��� ���͸� �˻��Ѵ�.
            //for(int i = 0; i< monsterSpawner.MonsterList.Count; i++)
            //{
            //    // ���� �ʿ� �ִ� ���� �ڽ��� �Ÿ��� ����Ѵ�.
            //    float distance = Vector3.Distance(monsterSpawner.MonsterList[i].transform.position, transform.position);
            //    // ���� ������ ����� �Ÿ��� ���� ���� �ȿ� �ְ� ���� �˻��� ������ ������ ����
            //    if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            //    {
            //        // ���� �ű⸦ closestDistSqr�� ����
            //        closestDistSqr = distance;
            //        // ���ǿ� �´� ���� ���� ������� ����
            //        attackTarget = monsterSpawner.MonsterList[i].transform;
            //    }
            //}

            // ���� Ÿ������ ���� ������ �ִ� ���͸� Ž���Ѵ�.
            attackTarget = FindClosestAttackTarget();

            // ���� ���� ����� ������ ����
            if (attackTarget != null)
            {
                // ���¸� ���� ���·� ����
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
    /// ���� �����ϴ� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCannon()
    {
        // �ݺ����� ���� ���� ����
        while (true)
        {
            //// ���� ���� ����� ���ٸ� ����
            //if(attackTarget == null)
            //{
            //    // ���¸� �� Ž�� ���·� �ٲٰ�
            //    ChangeState(TowerState.SearchTarget);
            //    // �ݺ����� �������´�.
            //    break;
            //}

            //// ���Ϳ� Ÿ���� �Ÿ��� ����Ѵ�.
            //float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
            //// ���� �� �Ÿ��� ���� ���� �ۿ� �ִٸ� ����
            //if (distance > towerTemplate.weapon[level].range)
            //{
            //    // ���� ����� ���� ������ ����
            //    attackTarget = null;
            //    // ���¸� �� Ž�� ���·� �ٲ۴�.
            //    ChangeState(TowerState.SearchTarget);
            //    // �ݺ����� ���� ���´�.
            //    break;
            //}

            // ���� ����� ���ٸ�
            if(IsPossibleToAttackTarget() == false)
            {
                // �� Ž�� ���·� ��ȯ
                ChangeState(TowerState.SearchTarget);
                break;
            }

            // ���� ��Ÿ�Ӹ�ŭ ����ߴٰ� ����
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            SpawnBullet();
        }
    }

    /// <summary>
    /// ������ ��ž�� �����ϰ� �ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackLaser()
    {
        LaserOn();

        // �ݺ����� ���� ���� ����
        while (true)
        {
            // ���� ������ �� �ִ� ����� ���ٸ� ����
            if(IsPossibleToAttackTarget() == false)
            {
                // �������� �������� ����.
                LaserOff();
                // Ÿ���� ���¸� �� Ž�� ���·� �ٲ۴�.
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
    /// ���� Ÿ������ ���� ������ �ִ� ���� ����� ã�� �Լ�
    /// </summary>
    /// <returns></returns>
    private Transform FindClosestAttackTarget()
    {
        // ���� �����̿� �ִ� ���� ã�� ���� ó�� �Ÿ��� �ִ��� ũ�� ����
        float closestDistSqr = Mathf.Infinity;

        // ���� �����ʿ� �ִ� ��� ���� �˻��ϰ�
        for(int i =0; i<monsterSpawner.MonsterList.Count; i++)
        {
            // ���� ��ž�� ���� �Ÿ��� ����Ѵ�.
            float distance = Vector3.Distance(monsterSpawner.MonsterList[i].transform.position, transform.position);

            // ���� ������ ����� �Ÿ��� ���� ���� �ȿ� �ְ� �ٸ� ���麸�� ���� ������ ����
            if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                // ���� ����� ���� �����ϰ�
                closestDistSqr = distance;
                // �� ���� ���� ������� ����
                attackTarget = monsterSpawner.MonsterList[i].transform;
            }
        }

        return attackTarget;
    }

    /// <summary>
    /// ���� ����� �ִ��� �˻��ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    private bool IsPossibleToAttackTarget()
    {
        // ���� ����� ������
        if(attackTarget == null)
        {
            // ������� false�� �Լ� ����
            return false;
        }

        // ���� ���� ��ž�� �Ÿ��� ����Ѵ�.
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        // ����� �Ÿ��� Ÿ���� ���� ���� �ۿ� �ִٸ� ����
        if(distance > towerTemplate.weapon[level].range)
        {
            // ���� ����� ���� ������ �����ϰ�
            attackTarget = null;
            // ������� false�� �Լ� ����
            return false;
        }

        // ���� ���ǿ� �ɸ��� ������ ���� ���ɻ���.
        return true;
    }

    /// <summary>
    /// �߻�ü�� ��ȯ�ϴ� �Լ�
    /// </summary>
    private void SpawnBullet()
    {
        // �߻�ü ����
        // �߻�ü��, ���������ǿ� ȸ�� ���� ��ȯ
        GameObject clone = Instantiate(bullet, spawnPosition.position, Quaternion.identity);
        // ������ �߻�ü���� ���ݴ���� ������ ����
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
    /// Ÿ���� ������ �ø��� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool Upgrade()
    {
        // ���� ���� �÷��̾ ���� �� ���� Ÿ�� ���׷��̵� ����� �� ���ٸ� ����
        if(playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            // ���� ��
            return false;
        }

        // Ÿ���� ������ �ø���
        level++;
        // ���� ������ Ÿ���� ������ ��븸ŭ ����
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
