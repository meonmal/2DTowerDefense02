using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    /// <summary>
    /// 플레이어의 최대 체력
    /// </summary>
    [SerializeField]
    private int maxHP;
    /// <summary>
    /// 플레이어의 현재 체력
    /// </summary>
    private int currentHP;

    /// <summary>
    /// 읽기 전용 프로퍼티
    /// </summary>
    public int MaxHP => maxHP;
    /// <summary>
    /// 읽기 전용 프로퍼티
    /// </summary>
    public int CurrentHP => currentHP;

    private void Awake()
    {
        // 현재 체력을 최대 체력과 같게 설정
        currentHP = maxHP;
    }

    /// <summary>
    /// 데미지를 받는 함수
    /// </summary>
    /// <param name="damage">플레이어가 받을 데미지</param>
    public void TakeDamage(int damage)
    {
        // 현재 체력에서 damage만큼 감소한다.
        currentHP -= damage;

        // 만약 현재 체력이 0과 같거나 이하면 실행
        if(currentHP <= 0)
        {

        }
    }
}
