using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    private void Awake()
    {
        OffAttackRange();
    }

    /// <summary>
    /// ���� ������ ���̰� �ϴ� �Լ�
    /// </summary>
    /// <param name="position">���� ������ �־�� �� ��ġ</param>
    /// <param name="range">���� ����</param>
    public void OnAttackRange(Vector3 position, float range)
    {
        // ���� ������Ʈ�� ���̰� �Ѵ�.
        gameObject.SetActive(true);

        // ���� ������ ũ��
        // AttackRange�� ���� ���⿡ ���� �����̱⿡ * 2.0�� ���ش�.
        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        // ���� ���� ��ġ
        transform.position = position;
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}
