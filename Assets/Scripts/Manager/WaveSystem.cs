using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    /// <summary>
    /// ���̺��� ��� ����
    /// </summary>
    [SerializeField]
    private Wave[] wave;
    /// <summary>
    /// ���� ������
    /// </summary>
    [SerializeField]
    private MonsterSpawner monsterSpawner;
    /// <summary>
    /// ���� ���̺� �ε���
    /// </summary>
    private int currentWaveIndex = -1;

    /// <summary>
    /// �б� ���� ������Ƽ
    /// ������ 0�̱⿡ +1�� ���ش�.
    /// ���� ���̺�� ���� ���̺� �ε��� + 1�̴�.
    /// </summary>
    public int CurrentWave => currentWaveIndex + 1;
    /// <summary>
    /// �б� ���� ������Ƽ
    /// ���̺갡 ����� ��Ÿ����.
    /// </summary>
    public int MaxWave => wave.Length;

    /// <summary>
    /// ���̺긦 �����ϴ� �Լ�
    /// </summary>
    public void StartWave()
    {
        // ���� �ʿ� ���Ͱ� ���� ���� �����ؾ� �� ���̺갡 ���� ������ ����
        if(monsterSpawner.MonsterList.Count == 0 && currentWaveIndex < wave.Length -1)
        {
            // ���� ���̺� �ε����� +1���ش�.
            currentWaveIndex++;
            // ���� �������� StartWave() �Լ��� ȣ�����ش�.
            // ���ؼ� ���� ���̺� ������ ����.
            monsterSpawner.StartWave(wave[currentWaveIndex]);
        }
    }
}

/// <summary>
/// [System.Serializable]�� ����ü�� ����ȭ�ϴ� ���̴�.
/// �̷��� �ϸ� Ŭ���� ������ ���� �������� �ν�����â���� ������ �� �ִ�.
/// </summary>
[System.Serializable]
public struct Wave
{
    /// <summary>
    /// ��ȯ �ֱ�
    /// </summary>
    public float spawnTime;
    /// <summary>
    /// �̹� ���̺꿡�� ���� ������ �ִ��
    /// </summary>
    public int maxMonsterSpawnCount;
    /// <summary>
    /// �̹� ���̺꿡�� ���� ������ ����
    /// </summary>
    public GameObject[] monsters;
}
