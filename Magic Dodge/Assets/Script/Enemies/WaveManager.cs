// WaveManager.cs (VERSÃO FINAL E ÚNICA)
using System.Collections;
using UnityEngine;

[System.Serializable]
public class WaveSetup
{
    public string waveName;
    public GameObject enemyPrefab;
    public GameObject bulletPrefabForThisWave;
    public ShootingMode shootingPatternForThisWave;
    public float fireRateForThisWave = 2f;
    public int enemyCount;
    public float timeBetweenSpawns = 1f;
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public WaveSetup[] waves;
    public Transform[] spawnPoints;

    private PlayerShooting playerShooting;
    private int currentWaveIndex = -1;
    private int enemiesRemaining;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        playerShooting = PlayerShooting.instance;
        if (playerShooting == null)
        {
            Debug.LogError("FATAL: WaveManager não encontrou o PlayerShooting!");
            return;
        }
        StartCoroutine(StartNextWaveAfterDelay(2f));
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            StartCoroutine(StartNextWaveAfterDelay(3f));
        }
    }

    private IEnumerator StartNextWaveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int nextWaveIndex = currentWaveIndex + 1;

        if (nextWaveIndex >= waves.Length)
        {
            Debug.Log("<color=yellow>JOGO VENCIDO!</color>");
            yield break;
        }

        currentWaveIndex = nextWaveIndex;
        WaveSetup currentWave = waves[currentWaveIndex];

        playerShooting.ApplyWaveShootingSetup(
            currentWave.shootingPatternForThisWave,
            currentWave.bulletPrefabForThisWave,
            currentWave.fireRateForThisWave
        );

        StartCoroutine(SpawnWave(currentWave));
    }

    private IEnumerator SpawnWave(WaveSetup wave)
    {
        enemiesRemaining = wave.enemyCount;
        for (int i = 0; i < wave.enemyCount; i++)
        {
            if (spawnPoints.Length == 0)
            {
                Debug.LogError("Nenhum ponto de spawn configurado!");
                yield break;
            }
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemyGO = Instantiate(wave.enemyPrefab, randomSpawnPoint.position, Quaternion.identity);

            WaveEnemyController enemyController = enemyGO.GetComponent<WaveEnemyController>();
            if (enemyController != null)
            {
                enemyController.Initialize(this);
            }
            else
            {
                Debug.LogError("Prefab do inimigo não tem o script 'WaveEnemyController.cs'!");
            }
            yield return new WaitForSeconds(wave.timeBetweenSpawns);
        }
    }
}