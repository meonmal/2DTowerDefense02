using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    /// <summary>
    /// �÷��̾��� �ִ� ü��
    /// </summary>
    [SerializeField]
    private int maxHP;
    /// <summary>
    /// �÷��̾��� ���� ü��
    /// </summary>
    private int currentHP;

    /// <summary>
    /// �б� ���� ������Ƽ
    /// </summary>
    public int MaxHP => maxHP;
    /// <summary>
    /// �б� ���� ������Ƽ
    /// </summary>
    public int CurrentHP => currentHP;

    private void Awake()
    {
        // ���� ü���� �ִ� ü�°� ���� ����
        currentHP = maxHP;
    }

    /// <summary>
    /// �������� �޴� �Լ�
    /// </summary>
    /// <param name="damage">�÷��̾ ���� ������</param>
    public void TakeDamage(int damage)
    {
        // ���� ü�¿��� damage��ŭ �����Ѵ�.
        currentHP -= damage;

        // ���� ���� ü���� 0�� ���ų� ���ϸ� ����
        if(currentHP <= 0)
        {

        }
    }
}
