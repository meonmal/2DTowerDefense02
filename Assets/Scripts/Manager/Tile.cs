using UnityEngine;

public class Tile : MonoBehaviour
{
    /// <summary>
    /// Ÿ�Ͽ� Ÿ���� ��ġ �Ǿ� �ִ��� Ȯ���ϴ� ����
    /// �̰� �ڵ� ���� ������Ƽ��, �ܺο��� �б�, ���Ⱑ ��� �����ϴ�.
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
    /// ���� ����.
    /// </summary>
    public bool isBuildTower { set; get; }

    private void Awake()
    {
        isBuildTower = false;
    }
}
