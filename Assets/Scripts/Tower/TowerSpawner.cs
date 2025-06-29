using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    /// <summary>
    /// 타워 프리팹
    /// </summary>
    [SerializeField]
    private GameObject towerPrefab;
    /// <summary>
    /// 현재 맵에 있는 적의 리스트 정보를 얻기 위해 생성
    /// </summary>
    [SerializeField]
    private MonsterSpawner monsterSpawner;
    /// <summary>
    /// 타워를 건설하는데 드는 비용
    /// </summary>
    [SerializeField]
    private int towerBuildGold;
    [SerializeField]
    private PlayerGold playerGold;

    /// <summary>
    /// 타워를 생성하는 함수
    /// </summary>
    /// <param name="tileTransform">타일 정보</param>
    public void TowerSpawn(Transform tileTransform)
    {
        // 만약 현재 갖고 있는 돈보다 타워 건설비용이 더 많다면 실행
        if(playerGold.CurrentGold < towerBuildGold)
        {
            // 실행 ㄴ
            return;
        }

        // 지역변수인 tile에 Tile 컴포넌트를 붙여준다.
        Tile tile = tileTransform.GetComponent<Tile>();

        // 해당 타일에 만약 설치되어 있는 타워가 있으면 실행
        if ((tile.isBuildTower == true))
        {
            // 실행하지 마라.
            return;
        }

        // 해당 타일에 타워를 설치하기 전 isBuildTower를 true로 변경
        tile.isBuildTower = true;
        // 현재 갖고 있는 돈에서 타워 건설비용만큼 감소시킨다.
        playerGold.CurrentGold -= towerBuildGold;
        // 타일보다 z축 -1에 설치한다.
        Vector3 position = tileTransform.position + Vector3.back;
        // 본인이 정한 타일의 위치에 타워 소환
        // 타워 프리팹을, 본인이 정한 타워의 위치에 회전 없이 소환.
        GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        // 타워 무기에 monsterSpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(monsterSpawner);
    }
}
