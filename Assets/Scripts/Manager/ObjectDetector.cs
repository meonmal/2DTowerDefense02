using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    /// <summary>
    /// Ÿ�� ������
    /// </summary>
    [SerializeField]
    private TowerSpawner towerSpawner;
    /// <summary>
    /// Ÿ�� ���� ����� ����
    /// </summary>
    [SerializeField]
    private TowerData towerData;

    /// <summary>
    /// ���� ī�޶�
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// ������ ���� ��ġ�� ������ ���� ����
    /// </summary>
    private Ray ray;
    /// <summary>
    /// ������ ���� ������Ʈ ������ ���� ����
    /// </summary>
    private RaycastHit hit;
    /// <summary>
    /// ���콺 ��ŷ���� ������ ������Ʈ�� �ӽ� ����
    /// </summary>
    private Transform hitTransform = null;

    /// <summary>
    /// ���� ���� ���۵ǰ� �ٷ� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Awake()
    {
        // "MainCamera" �±׸� ������ �ִ� ������Ʈ�� Ž�� �� Camera ������Ʈ�� ����
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();�� ����
        mainCamera = Camera.main;
    }

    /// <summary>
    /// �� ������ ���� ȣ��Ǵ� �Լ�
    /// </summary>
    private void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

        // ���콺 ���� ��ư�� ������ �� ����
        if (Input.GetMouseButtonDown(0))
        {
            // �ڽ��� ���� ���콺�� ��ġ�� ������ �����Ѵ�.
            // �� �� ray�� ������� ������ ������ ���� ��ġ�� ���� �����̴�.
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // ������ �ε��� ������Ʈ�� ������ hit�� ����
            // 2D�����ε��� Physics2D�� �ƴ� Physics�� �� ������
            // ������ �ᱹ z������ ���� �ϱ� �����̴�.
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;

                // ���� ������ ���� ������Ʈ�� �±װ� Tile�̶�� ����
                if (hit.transform.CompareTag("Tile"))
                {
                    // towerSpawner�� TowerSpawn�Լ� ����
                    towerSpawner.TowerSpawn(hit.transform);
                }
                // ���� Ŭ���� ������Ʈ�� �±װ� Tower�� ����
                else if (hit.transform.CompareTag("Tower"))
                {
                    // Ÿ�� ���� �ǳ��� ���̰� �Ѵ�.
                    towerData.PanelOn(hit.transform);
                }
            }
        }
        // ���콺 ����Ŭ���� ����
        else if (Input.GetMouseButtonUp(0))
        {
            // ���� ���� ������Ʈ�� ���ų� ���� ������Ʈ�� �±װ� Tower�� �ƴ� �� ����
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                // Ÿ�� ������ ������Ѵ�.
                towerData.PanelOff();
            }

            hitTransform = null;
        }
    }
}
