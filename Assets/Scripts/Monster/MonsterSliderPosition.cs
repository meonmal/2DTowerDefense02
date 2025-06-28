using UnityEngine;

public class MonsterSliderPosition : MonoBehaviour
{
    /// <summary>
    /// 몬스터 HP바의 위치
    /// </summary>
    [SerializeField]
    private Vector3 distance = Vector3.down * 20;
    /// <summary>
    /// 몬스터 HP바가 쫓아다닐 대상
    /// </summary>
    private Transform target;
    private RectTransform rectTransform;

    /// <summary>
    /// 먼저 세팅하는 함수
    /// </summary>
    /// <param name="target">쫓아다닐 대상</param>
    public void Setup(Transform target)
    {
        this.target = target;
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수지만 모든 Update가 호출되고 나서야 호출이 된다.
    /// 그렇기에 이동하는 몬스터를 따라다니게 하기에 적합하다.
    /// </summary>
    private void LateUpdate()
    {
        // 만약 따라다닐 대상이 없다면 실행
        if(target == null)
        {
            // 게임 오브젝트 삭제
            Destroy(gameObject);
            return;
        }

        // 몬스터의 위치를 월드좌표를 기준으로 화면에서의 좌표값을 구한다.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position);
        // 위에서 구한 좌표 + distance만큼 떨어진 위치를 MonsterSlider의 위치로 지정한다.
        rectTransform.position = screenPosition + distance;
    }
}
