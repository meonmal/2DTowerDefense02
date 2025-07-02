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
        /// Ÿ���� ������
        /// </summary>
        public float damage;
        /// <summary>
        /// Ÿ���� ���� �ۼ�Ʈ(0.2 = 20%)
        /// </summary>
        public float slow;
        /// <summary>
        /// ���ݷ� ������(0.2 = +20%)
        /// </summary>
        public float buff;
        /// <summary>
        /// Ÿ���� ���� ��Ÿ��
        /// </summary>
        public float rate;
        /// <summary>
        /// Ÿ���� ���� ����
        /// </summary>
        public float range;
        /// <summary>
        /// Ÿ���� ������ ���
        /// </summary>
        public int cost;
        /// <summary>
        /// Ÿ���� �Ǹ��ϸ� ȹ���ϴ� ��
        /// </summary>
        public int sell;
    }

    
}
