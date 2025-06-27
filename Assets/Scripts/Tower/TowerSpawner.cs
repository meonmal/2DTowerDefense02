using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    /// <summary>
    /// Ÿ�� ������
    /// </summary>
    [SerializeField]
    private GameObject towerPrefab;
    /// <summary>
    /// ���� �ʿ� �ִ� ���� ����Ʈ ������ ��� ���� ����
    /// </summary>
    [SerializeField]
    private MonsterSpawner monsterSpawner;

    /// <summary>
    /// Ÿ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="tileTransform">Ÿ�� ����</param>
    public void TowerSpawn(Transform tileTransform)
    {
        // ���������� tile�� Tile ������Ʈ�� �ٿ��ش�.
        Tile tile = tileTransform.GetComponent<Tile>();

        // �ش� Ÿ�Ͽ� ���� ��ġ�Ǿ� �ִ� Ÿ���� ������ ����
        if ((tile.isBuildTower == true))
        {
            // �������� ����.
            return;
        }

        // �ش� Ÿ�Ͽ� Ÿ���� ��ġ�ϱ� �� isBuildTower�� true�� ����
        tile.isBuildTower = true;

        // ������ ���� Ÿ���� ��ġ�� Ÿ�� ��ȯ
        // Ÿ�� ��������, ������ ���� Ÿ���� ��ġ�� ȸ�� ���� ��ȯ.
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        // Ÿ�� ���⿡ monsterSpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(monsterSpawner);
    }
}
