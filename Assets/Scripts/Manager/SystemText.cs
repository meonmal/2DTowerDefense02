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
                systemText.text = "돈이 부족합니다...!!";
                break;
            case SystemType.Build:
                systemText.text = "이미 타워가 설치 되었습니다...!!";
                break;
        }

        systemTextSetting.FadeOut();
    }
}
