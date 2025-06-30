using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerData : MonoBehaviour
{
    [SerializeField]
    private Image towerImage;
    [SerializeField]
    private TextMeshProUGUI towerDamage;
    [SerializeField]
    private TextMeshProUGUI towerRate;
    [SerializeField]
    private TextMeshProUGUI towerRange;
    [SerializeField]
    private TextMeshProUGUI towerLevel;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private SystemText systemText;

    private TowerWeapon currentTower;

    private void Awake()
    {
        PanelOff();
    }

    private void Update()
    {
        // esc ��ư ������ ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelOff();
        }
    }


    public void PanelOn(Transform tower)
    {
        currentTower = tower.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        TowerDataUpdate();
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    public void PanelOff()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    public void TowerDataUpdate()
    {
        towerImage.sprite = currentTower.TowerSprite;
        towerDamage.text = "Damage : " + currentTower.Damage;
        towerRate.text = "Rate : " + currentTower.Rate;
        towerRange.text = "Range : " + currentTower.Range;
        towerLevel.text = "Level : " + currentTower.Level;

        // ���׷��̵尡 �Ұ��������� ���׷��̵� ��ư�� ��Ȱ��ȭ
        upgradeButton.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void ClickButtonUpgrade()
    {
        bool isSucces = currentTower.Upgrade();

        if(isSucces == true)
        {
            TowerDataUpdate();
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            systemText.PrintText(SystemType.Money);
        }
    }

    public void ClickButtonSell()
    {
        currentTower.Sell();
        PanelOff();
    }
}
