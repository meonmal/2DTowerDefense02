using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField]
    private int currentGold;

    public int CurrentGold
    {
        // 현재 골드를 0부터 시작해서 음수가 되지 않도록 함
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }
}
