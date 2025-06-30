using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public enum SystemType { Money = 0, Build}
public class SystemText : MonoBehaviour
{
    private TextMeshProUGUI systemText;
    private SystemTextSetting systemTextSetting;

    private void Awake()
    {
        systemText = GetComponent<TextMeshProUGUI>();
        systemTextSetting = GetComponent<SystemTextSetting>();
    }

    public void PrintText(SystemType type)
    {
        switch (type)
        {
            case SystemType.Money:
                systemText.text = "���� �����մϴ�...!!";
                break;
            case SystemType.Build:
                systemText.text = "�̹� Ÿ���� ��ġ �Ǿ����ϴ�...!!";
                break;
        }

        systemTextSetting.FadeOut();
    }
}
