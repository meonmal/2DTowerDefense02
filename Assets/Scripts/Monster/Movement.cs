using UnityEngine;

public class Movement : MonoBehaviour
{
    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    [SerializeField]
    private float moveSpeed;
    /// <summary>
    /// �̵� ����
    /// </summary>
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private float baseMoveSpeed;

    /// <summary>
    /// �ܺο��� ���� ������.
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
    /// �� ������ ���� ����Ǵ� �Լ�
    /// </summary>
    private void Update()
    {
        // ���� ��ġ���� �� �� ���� moveSpeed�� �ӵ��� moveDirection�� �������� �̵�
        transform.position += moveSpeed * moveDirection * Time.deltaTime;
    }

    /// <summary>
    /// �̵� ������ ����ϴ� �Լ�
    /// </summary>
    /// <param name="direction">�̵� ����</param>
    public void MoveDir(Vector3 direction)
    {
        moveDirection = direction;
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
}
