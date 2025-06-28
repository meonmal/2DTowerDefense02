using UnityEngine;
using UnityEngine.UI;

public class MonsterSlider : MonoBehaviour
{
    private MonsterHP monsterHP;
    private Slider slider;

    public void Setup(MonsterHP monsterHP)
    {
        this.monsterHP = monsterHP;
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = monsterHP.CurrentHP / monsterHP.MaxHP;
    }
}
