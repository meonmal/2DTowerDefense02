using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerObject;
    public Weapon[] weapon;
    public Sprite sprite;
    public GameObject followTower;
    
    [System.Serializable]
    public struct Weapon
    {
        /// <summary>
        /// 타워의 데미지
        /// </summary>
        public float damage;
        /// <summary>
        /// 타워의 공격 쿨타임
        /// </summary>
        public float rate;
        /// <summary>
        /// 타워의 공격 범위
        /// </summary>
        public float range;
        /// <summary>
        /// 타워의 레벨별 비용
        /// </summary>
        public int cost;
        /// <summary>
        /// 타워를 판매하면 획득하는 돈
        /// </summary>
        public int sell;
    }

    
}
