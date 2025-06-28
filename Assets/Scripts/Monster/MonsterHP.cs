using System.Collections;
using UnityEngine;

public class MonsterHP : MonoBehaviour
{
    /// <summary>
    /// 몬스터의 최대 체력
    /// </summary>
    [SerializeField]
    private float maxHP;
    /// <summary>
    /// 몬스터의 현재 체력
    /// </summary>
    private float currentHP;
    /// <summary>
    /// 몬스터가 죽은 상태인지 확인하는 변수
    /// </summary>
    private bool isDie = false;
    private Monster monster;
    private Animator animator;

    /// <summary>
    /// 읽기 전용 프로퍼티
    /// </summary>
    public float MaxHP => maxHP;
    /// <summary>
    /// 외부 전용 프로퍼티
    /// </summary>
    public float CurrentHP => currentHP;

    private void Awake()
    {
        // 현재 체력을 최대 체력과 같게 설정
        currentHP = maxHP;
        monster = GetComponent<Monster>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 몬스터가 공격을 받을 때 실행되는 함수
    /// </summary>
    /// <param name="damage">몬스터가 받을 데미지</param>
    public void TakeDamage(float damage)
    {
        // 만약 몬스터가 죽은 상태면 실행
        if(isDie == true)
        {
            // 함수 취소
            return;
        }

        // 현재 체력을 damage만큼 감소시킨다.
        currentHP -= damage;

        // 만약 몬스터의 현재 체력이 0과 같거나 이하라면 실행
        if(currentHP <= 0)
        {
            // isDie를 true로 설정해 몬스터가 여러번 죽는 것을 방지한다.
            isDie = true;
            // monster의 OnDie()함수를 실행
            monster.OnDie(MonsterDieType.Kill);
        }
    }
}
