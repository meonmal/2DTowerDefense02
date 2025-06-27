using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    /// <summary>
    /// Ÿ�� ������
    /// </summary>
    [SerializeField]
    private TowerSpawner towerSpawner;

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
                // ���� ������ ���� ������Ʈ�� �±װ� Tile�̶�� ����
                if (hit.transform.CompareTag("Tile"))
                {
                    // towerSpawner�� TowerSpawn�Լ� ����
                    towerSpawner.TowerSpawn(hit.transform);
                }
            }
        }
    }
}
