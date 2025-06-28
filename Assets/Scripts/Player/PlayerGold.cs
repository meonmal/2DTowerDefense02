using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField]
    private int currentGold;

    public int CurrentGold
    {
        // ���� ��带 0���� �����ؼ� ������ ���� �ʵ��� ��
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }
}
