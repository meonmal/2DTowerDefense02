using TMPro;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerHPText;
    [SerializeField]
    private TextMeshProUGUI playerGoldText;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;

    private void Update()
    {
        playerHPText.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        playerGoldText.text = playerGold.CurrentGold.ToString();
    }
}
