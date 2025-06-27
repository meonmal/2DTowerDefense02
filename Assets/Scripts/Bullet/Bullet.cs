using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// 이동을 위한 무브먼트 변수
    /// </summary>
    private Movement movement;
    /// <summary>
    /// 공격 대상
    /// </summary>
    private Transform target;

    /// <summary>
    /// 세팅을 하는 함수
    /// </summary>
    /// <param name="target">공격 대상</param>
    public void Setup(Transform target)
    {
        movement = GetComponent<Movement>();
        this.target = target;
    }

    private void Update()
    {
        // 만약 공격 대상이 존재하면 실행
        if(target != null)
        {
            // 발사체와 공격 대상의 거리를 계산 후
            Vector3 direction = (target.position - transform.position).normalized;
            // 이동 실행
            movement.MoveDir(direction);
        }
        // 만약 적이 없다면 실행
        else
        {
            // 오브젝트 삭제
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 대상과 닿았을 때 실행되는 함수
    /// </summary>
    /// <param name="collision">공격 대상</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 만약 대상의 태그가 Monster가 아니면 실행
        if (!collision.CompareTag("Monster"))
        {
            // 취소
            return;
        }

        // 만약 대상이 적이 아닐 때 실행
        if(collision.transform != target)
        {
            // 취소
            return;
        }

        // 적 사망 함수 호출
        collision.GetComponent<Monster>().OnDie();
        // 오브젝트 삭제
        Destroy(gameObject);
    }
}
