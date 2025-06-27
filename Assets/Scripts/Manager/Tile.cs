using UnityEngine;

public class Tile : MonoBehaviour
{
    /// <summary>
    /// 타일에 타워가 설치 되어 있는지 확인하는 변수
    /// 이건 자동 구현 프로퍼티로, 외부에서 읽기, 쓰기가 모두 가능하다.
    /// private bool isBuildTower
    /// public bool IsBuildTower
    /// {
    ///     set
    ///     {
    ///         isBuildTower = value;
    ///     }
    ///     get
    ///     {
    ///         return isBuildTower;
    ///     }
    /// }
    /// 위와 같다.
    /// </summary>
    public bool isBuildTower { set; get; }

    private void Awake()
    {
        isBuildTower = false;
    }
}
