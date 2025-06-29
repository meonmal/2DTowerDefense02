using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    private void Awake()
    {
        OffAttackRange();
    }

    /// <summary>
    /// 공격 범위를 보이게 하는 함수
    /// </summary>
    /// <param name="position">공격 범위가 있어야 할 위치</param>
    /// <param name="range">공격 범위</param>
    public void OnAttackRange(Vector3 position, float range)
    {
        // 게임 오브젝트를 보이게 한다.
        gameObject.SetActive(true);

        // 공격 범위의 크기
        // AttackRange는 단일 방향에 대한 범위이기에 * 2.0을 해준다.
        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        // 공격 범위 위치
        transform.position = position;
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}
