using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    /// <summary>
    /// 타워 스포너
    /// </summary>
    [SerializeField]
    private TowerSpawner towerSpawner;
    /// <summary>
    /// 타워 정보 출력을 위함
    /// </summary>
    [SerializeField]
    private TowerData towerData;

    /// <summary>
    /// 메인 카메라
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// 광선의 시작 위치와 방향을 담을 변수
    /// </summary>
    private Ray ray;
    /// <summary>
    /// 광선에 맞은 오브젝트 정보를 담을 변수
    /// </summary>
    private RaycastHit hit;
    /// <summary>
    /// 마우스 픽킹으로 선택한 오브젝트를 임시 저장
    /// </summary>
    private Transform hitTransform = null;

    /// <summary>
    /// 현재 씬이 시작되고 바로 호출되는 함수
    /// </summary>
    private void Awake()
    {
        // "MainCamera" 태그를 가지고 있는 오브젝트를 탐색 후 Camera 컴포넌트를 전달
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();와 동일
        mainCamera = Camera.main;
    }

    /// <summary>
    /// 매 프레임 마다 호출되는 함수
    /// </summary>
    private void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

        // 마우스 왼쪽 버튼을 눌렀을 때 실행
        if (Input.GetMouseButtonDown(0))
        {
            // 자신이 누른 마우스의 위치에 광선을 생성한다.
            // 이 때 ray에 담겨지는 정보는 광선의 시작 위치와 진행 방향이다.
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 광선에 부딪힌 오브젝트의 정보를 hit에 저장
            // 2D게임인데도 Physics2D가 아닌 Physics를 쓴 이유는
            // 광선을 결국 z축으로 쏴야 하기 때문이다.
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;

                // 만약 광선을 맞은 오브젝트의 태그가 Tile이라면 실행
                if (hit.transform.CompareTag("Tile"))
                {
                    // towerSpawner의 TowerSpawn함수 실행
                    towerSpawner.TowerSpawn(hit.transform);
                }
                // 만약 클릭한 오브젝트의 태그가 Tower면 실행
                else if (hit.transform.CompareTag("Tower"))
                {
                    // 타워 정보 판넬을 보이게 한다.
                    towerData.PanelOn(hit.transform);
                }
            }
        }
        // 마우스 왼쪽클릭시 실행
        else if (Input.GetMouseButtonUp(0))
        {
            // 만약 누른 오브젝트가 없거나 누른 오브젝트의 태그가 Tower가 아닐 때 실행
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                // 타워 정보를 미출력한다.
                towerData.PanelOff();
            }

            hitTransform = null;
        }
    }
}
