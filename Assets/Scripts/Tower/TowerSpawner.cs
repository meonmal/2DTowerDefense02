using UnityEngine;
using System.Collections;

public class TowerSpawner : MonoBehaviour
{

    /// <summary>
    /// Ÿ���� ����(Ÿ���� �������̱⿡ �迭�� ����)
    /// </summary>
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    ///// <summary>
    ///// Ÿ�� ������
    ///// </summary>
    //[SerializeField]
    //private GameObject towerPrefab;
    /// <summary>
    /// ���� �ʿ� �ִ� ���� ����Ʈ ������ ��� ���� ����
    /// </summary>
    [SerializeField]
    private MonsterSpawner monsterSpawner;
    ///// <summary>
    ///// Ÿ���� �Ǽ��ϴµ� ��� ���
    ///// </summary>
    //[SerializeField]
    //private int towerBuildGold;
    [SerializeField]
    private PlayerGold playerGold;

    [SerializeField]
    private SystemText systemText;
    /// <summary>
    /// Ÿ�� �Ǽ� ��ư�� �������� Ȯ���ϴ� ����
    /// </summary>
    private bool isOnTowerButton = false;
    /// <summary>
    /// �ӽ� Ÿ���� �����ϱ� ���� ����
    /// </summary>
    private GameObject followTowerClone = null;
    /// <summary>
    /// Ÿ���� ����
    /// </summary>
    private int towerType;

    /// <summary>
    /// Ÿ�� �Ǽ� �غ�
    /// </summary>
    public void ReadyTowerSpawn(int type)
    {
        towerType = type;

        if(isOnTowerButton == true)
        {
            return;
        }

        // ���� ���� �ִ� ������ Ÿ���� �Ǽ� ����� �� ũ�ٸ� ����
        if(playerGold.CurrentGold < towerTemplate[towerType].weapon[0].cost)
        {
            // �ý��ۿ� �ִ� ������ �޼����� ���
            systemText.PrintText(SystemType.Money);
            return;
        }

        // Ÿ�� �Ǽ� ��ư�� �����ٰ� ����
        isOnTowerButton = true;
        followTowerClone = Instantiate(towerTemplate[towerType].followTower);
        StartCoroutine(TowerCancel());
    }

    /// <summary>
    /// Ÿ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="tileTransform">Ÿ�� ����</param>
    public void TowerSpawn(Transform tileTransform)
    {
        // ���� Ÿ�� �Ǽ� ��ư�� �� ���ȴٸ� ����
        if(isOnTowerButton == false)
        {
            // ��
            return;
        }

        //// ���� ���� ���� �ִ� ������ Ÿ�� �Ǽ������ �� ���ٸ� ����
        //if(playerGold.CurrentGold < towerTemplate.weapon[0].cost)
        //{
        //    systemText.PrintText(SystemType.Money);
        //    // ���� ��
        //    return;
        //}

        // ���������� tile�� Tile ������Ʈ�� �ٿ��ش�.
        Tile tile = tileTransform.GetComponent<Tile>();

        // �ش� Ÿ�Ͽ� ���� ��ġ�Ǿ� �ִ� Ÿ���� ������ ����
        if ((tile.isBuildTower == true))
        {
            systemText.PrintText(SystemType.Build);
            // �������� ����.
            return;
        }

        isOnTowerButton = false;
        // �ش� Ÿ�Ͽ� Ÿ���� ��ġ�ϱ� �� isBuildTower�� true�� ����
        tile.isBuildTower = true;
        // ���� ���� �ִ� ������ Ÿ�� �Ǽ���븸ŭ ���ҽ�Ų��.
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        // Ÿ�Ϻ��� z�� -1�� ��ġ�Ѵ�.
        Vector3 position = tileTransform.position + Vector3.back;
        // ������ ���� Ÿ���� ��ġ�� Ÿ�� ��ȯ
        // Ÿ�� ��������, ������ ���� Ÿ���� ��ġ�� ȸ�� ���� ��ȯ.
        GameObject clone = Instantiate(towerTemplate[towerType].towerObject, position, Quaternion.identity);
        // Ÿ�� ���⿡ monsterSpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(this, monsterSpawner, playerGold, tile);

        OnBuffAllBuffTowers();

        Destroy(followTowerClone);
        StopCoroutine(TowerCancel());
    }

    /// <summary>
    /// Ÿ�� �Ǽ��� ����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator TowerCancel()
    {
        while (true)
        {
            if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }

            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i=0; i<towers.Length; i++)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();
            
            if(weapon.TowerType == TowerType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
