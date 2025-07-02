using UnityEngine;

public class Movement : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    [SerializeField]
    private float moveSpeed;
    /// <summary>
    /// 이동 방향
    /// </summary>
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private float baseMoveSpeed;

    /// <summary>
    /// 외부에서 수정 가능함.
    /// </summary>
    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
    }

    /// <summary>
    /// 매 프레임 마다 실행되는 함수
    /// </summary>
    private void Update()
    {
        // 현재 위치에서 매 초 마다 moveSpeed의 속도로 moveDirection의 방향으로 이동
        transform.position += moveSpeed * moveDirection * Time.deltaTime;
    }

    /// <summary>
    /// 이동 방향을 담당하는 함수
    /// </summary>
    /// <param name="direction">이동 방향</param>
    public void MoveDir(Vector3 direction)
    {
        moveDirection = direction;
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
}
