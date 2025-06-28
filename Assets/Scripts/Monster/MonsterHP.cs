using System.Collections;
using UnityEngine;

public class MonsterHP : MonoBehaviour
{
    /// <summary>
    /// ������ �ִ� ü��
    /// </summary>
    [SerializeField]
    private float maxHP;
    /// <summary>
    /// ������ ���� ü��
    /// </summary>
    private float currentHP;
    /// <summary>
    /// ���Ͱ� ���� �������� Ȯ���ϴ� ����
    /// </summary>
    private bool isDie = false;
    private Monster monster;
    private Animator animator;

    /// <summary>
    /// �б� ���� ������Ƽ
    /// </summary>
    public float MaxHP => maxHP;
    /// <summary>
    /// �ܺ� ���� ������Ƽ
    /// </summary>
    public float CurrentHP => currentHP;

    private void Awake()
    {
        // ���� ü���� �ִ� ü�°� ���� ����
        currentHP = maxHP;
        monster = GetComponent<Monster>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// ���Ͱ� ������ ���� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="damage">���Ͱ� ���� ������</param>
    public void TakeDamage(float damage)
    {
        // ���� ���Ͱ� ���� ���¸� ����
        if(isDie == true)
        {
            // �Լ� ���
            return;
        }

        // ���� ü���� damage��ŭ ���ҽ�Ų��.
        currentHP -= damage;

        // ���� ������ ���� ü���� 0�� ���ų� ���϶�� ����
        if(currentHP <= 0)
        {
            // isDie�� true�� ������ ���Ͱ� ������ �״� ���� �����Ѵ�.
            isDie = true;
            // monster�� OnDie()�Լ��� ����
            monster.OnDie(MonsterDieType.Kill);
        }
    }
}
