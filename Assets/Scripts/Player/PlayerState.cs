using TMPro;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerHPText;
    [SerializeField]
    private TextMeshProUGUI playerGoldText;
    [SerializeField]
    private TextMeshProUGUI waveText;
    [SerializeField]
    private TextMeshProUGUI monsterCountText;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private WaveSystem waveSystem;
    [SerializeField]
    private MonsterSpawner monsterSpawner;

    private void Update()
    {
        playerHPText.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        playerGoldText.text = playerGold.CurrentGold.ToString();
        waveText.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        monsterCountText.text = monsterSpawner.CurrentMonsterCount + "/" + monsterSpawner.MaxMonsterCount;
    }
}
