using UnityEngine;

public class MonsterSliderPosition : MonoBehaviour
{
    /// <summary>
    /// ���� HP���� ��ġ
    /// </summary>
    [SerializeField]
    private Vector3 distance = Vector3.down * 20;
    /// <summary>
    /// ���� HP�ٰ� �Ѿƴٴ� ���
    /// </summary>
    private Transform target;
    private RectTransform rectTransform;

    /// <summary>
    /// ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="target">�Ѿƴٴ� ���</param>
    public void Setup(Transform target)
    {
        this.target = target;
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ����� ��� Update�� ȣ��ǰ� ������ ȣ���� �ȴ�.
    /// �׷��⿡ �̵��ϴ� ���͸� ����ٴϰ� �ϱ⿡ �����ϴ�.
    /// </summary>
    private void LateUpdate()
    {
        // ���� ����ٴ� ����� ���ٸ� ����
        if(target == null)
        {
            // ���� ������Ʈ ����
            Destroy(gameObject);
            return;
        }

        // ������ ��ġ�� ������ǥ�� �������� ȭ�鿡���� ��ǥ���� ���Ѵ�.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position);
        // ������ ���� ��ǥ + distance��ŭ ������ ��ġ�� MonsterSlider�� ��ġ�� �����Ѵ�.
        rectTransform.position = screenPosition + distance;
    }
}
