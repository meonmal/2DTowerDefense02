using UnityEngine;
using System.Collections;

public class TowerSpawner : MonoBehaviour
{

    /// <summary>
    /// 타워의 정보(타워가 여러개이기에 배열로 선언)
    /// </summary>
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    ///// <summary>
    ///// 타워 프리팹
    ///// </summary>
    //[SerializeField]
    //private GameObject towerPrefab;
    /// <summary>
    /// 현재 맵에 있는 적의 리스트 정보를 얻기 위해 생성
    /// </summary>
    [SerializeField]
    private MonsterSpawner monsterSpawner;
    ///// <summary>
    ///// 타워를 건설하는데 드는 비용
    ///// </summary>
    //[SerializeField]
    //private int towerBuildGold;
    [SerializeField]
    private PlayerGold playerGold;

    [SerializeField]
    private SystemText systemText;
    /// <summary>
    /// 타워 건설 버튼을 눌렀는지 확인하는 변수
    /// </summary>
    private bool isOnTowerButton = false;
    /// <summary>
    /// 임시 타워를 삭제하기 위한 변수
    /// </summary>
    private GameObject followTowerClone = null;
    /// <summary>
    /// 타워의 종류
    /// </summary>
    private int towerType;

    /// <summary>
    /// 타워 건설 준비
    /// </summary>
    public void ReadyTowerSpawn(int type)
    {
        towerType = type;

        if(isOnTowerButton == true)
        {
            return;
        }

        // 현재 갖고 있는 돈보다 타워의 건설 비용이 더 크다면 실행
        if(playerGold.CurrentGold < towerTemplate[towerType].weapon[0].cost)
        {
            // 시스템에 있는 돈부족 메세지를 출력
            systemText.PrintText(SystemType.Money);
            return;
        }

        // 타워 건설 버튼을 눌렀다고 설정
        isOnTowerButton = true;
        followTowerClone = Instantiate(towerTemplate[towerType].followTower);
        StartCoroutine(TowerCancel());
    }

    /// <summary>
    /// 타워를 생성하는 함수
    /// </summary>
    /// <param name="tileTransform">타일 정보</param>
    public void TowerSpawn(Transform tileTransform)
    {
        // 만약 타워 건설 버튼이 안 눌렸다면 실행
        if(isOnTowerButton == false)
        {
            // ㄴ
            return;
        }

        //// 만약 현재 갖고 있는 돈보다 타워 건설비용이 더 많다면 실행
        //if(playerGold.CurrentGold < towerTemplate.weapon[0].cost)
        //{
        //    systemText.PrintText(SystemType.Money);
        //    // 실행 ㄴ
        //    return;
        //}

        // 지역변수인 tile에 Tile 컴포넌트를 붙여준다.
        Tile tile = tileTransform.GetComponent<Tile>();

        // 해당 타일에 만약 설치되어 있는 타워가 있으면 실행
        if ((tile.isBuildTower == true))
        {
            systemText.PrintText(SystemType.Build);
            // 실행하지 마라.
            return;
        }

        isOnTowerButton = false;
        // 해당 타일에 타워를 설치하기 전 isBuildTower를 true로 변경
        tile.isBuildTower = true;
        // 현재 갖고 있는 돈에서 타워 건설비용만큼 감소시킨다.
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        // 타일보다 z축 -1에 설치한다.
        Vector3 position = tileTransform.position + Vector3.back;
        // 본인이 정한 타일의 위치에 타워 소환
        // 타워 프리팹을, 본인이 정한 타워의 위치에 회전 없이 소환.
        GameObject clone = Instantiate(towerTemplate[towerType].towerObject, position, Quaternion.identity);
        // 타워 무기에 monsterSpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(this, monsterSpawner, playerGold, tile);

        OnBuffAllBuffTowers();

        Destroy(followTowerClone);
        StopCoroutine(TowerCancel());
    }

    /// <summary>
    /// 타워 건설을 취소하는 코루틴 함수
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
