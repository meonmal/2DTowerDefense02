using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;

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
    /// Ÿ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="tileTransform">Ÿ�� ����</param>
    public void TowerSpawn(Transform tileTransform)
    {
        // ���� ���� ���� �ִ� ������ Ÿ�� �Ǽ������ �� ���ٸ� ����
        if(playerGold.CurrentGold < towerTemplate.weapon[0].cost)
        {
            systemText.PrintText(SystemType.Money);
            // ���� ��
            return;
        }

        // ���������� tile�� Tile ������Ʈ�� �ٿ��ش�.
        Tile tile = tileTransform.GetComponent<Tile>();

        // �ش� Ÿ�Ͽ� ���� ��ġ�Ǿ� �ִ� Ÿ���� ������ ����
        if ((tile.isBuildTower == true))
        {
            systemText.PrintText(SystemType.Build);
            // �������� ����.
            return;
        }

        // �ش� Ÿ�Ͽ� Ÿ���� ��ġ�ϱ� �� isBuildTower�� true�� ����
        tile.isBuildTower = true;
        // ���� ���� �ִ� ������ Ÿ�� �Ǽ���븸ŭ ���ҽ�Ų��.
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        // Ÿ�Ϻ��� z�� -1�� ��ġ�Ѵ�.
        Vector3 position = tileTransform.position + Vector3.back;
        // ������ ���� Ÿ���� ��ġ�� Ÿ�� ��ȯ
        // Ÿ�� ��������, ������ ���� Ÿ���� ��ġ�� ȸ�� ���� ��ȯ.
        GameObject clone = Instantiate(towerTemplate.towerObject, position, Quaternion.identity);
        // Ÿ�� ���⿡ monsterSpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(monsterSpawner, playerGold, tile);
    }
}
