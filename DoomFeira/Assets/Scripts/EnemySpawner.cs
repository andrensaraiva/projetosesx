using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configura��o de Inimigos")]
    public GameObject[] enemyPrefabs;

    // --- MUDAN�A: De raio para dimens�es de uma caixa ---
    [Header("�rea de Spawn (Paralelep�pedo)")]
    public Vector3 spawnAreaSize = new Vector3(40, 1, 40); // Largura(X), Altura(Y), Comprimento(Z)

    [Header("Zona de Seguran�a do Jogador")]
    public Transform playerTransform;
    public float playerSafeZoneRadius = 7f;

    [Header("Dificuldade Progressiva")]
    public float initialSpawnInterval = 4.0f;
    public float minimumSpawnInterval = 0.5f;
    public float intervalDecreaseRate = 0.02f;
    public int initialSpawnCount = 1;
    public int maxSpawnCount = 10;
    public float timeToIncreaseCount = 30.0f;

    private float currentSpawnInterval;
    private int currentSpawnCount;

    // --- ADI��O: A Fun��o OnDrawGizmos ---
    // Esta fun��o � chamada pela Unity apenas no Editor, e � onde desenhamos os Gizmos.
    void OnDrawGizmosSelected()
    {
        // Define a cor do Gizmo para a �rea de spawn
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Verde semitransparente
        // Desenha um cubo (paralelep�pedo) com o centro na posi��o do spawner e com o tamanho definido
        Gizmos.DrawCube(transform.position, spawnAreaSize);

        // Desenha a zona de seguran�a do jogador se ele estiver definido
        if (playerTransform != null)
        {
            // Define a cor do Gizmo para a zona de seguran�a
            Gizmos.color = new Color(1, 0, 0, 0.3f); // Vermelho semitransparente
            // Desenha uma esfera na posi��o do jogador com o raio da zona de seguran�a
            Gizmos.DrawSphere(playerTransform.position, playerSafeZoneRadius);
        }
    }


    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        currentSpawnCount = initialSpawnCount;

        if (playerTransform == null)
        {
            Debug.LogError("Refer�ncia do Jogador (PlayerTransform) n�o foi definida no EnemySpawner!", this.gameObject);
        }

        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(IncreaseDifficultyRoutine());
    }

    void Update()
    {
        if (currentSpawnInterval > minimumSpawnInterval)
        {
            currentSpawnInterval -= intervalDecreaseRate * Time.deltaTime;
        }
    }

    // ... As suas coroutines SpawnEnemiesRoutine e IncreaseDifficultyRoutine permanecem exatamente as mesmas ...

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);
            for (int i = 0; i < currentSpawnCount; i++)
            {
                SpawnSingleEnemy();
            }
        }
    }

    IEnumerator IncreaseDifficultyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToIncreaseCount);
            if (currentSpawnCount < maxSpawnCount)
            {
                currentSpawnCount++;
            }
        }
    }


    // --- FUN��O MODIFICADA PARA SPAWN EM CAIXA ---
    void SpawnSingleEnemy()
    {
        if (enemyPrefabs.Length == 0 || playerTransform == null) return;

        Vector3 spawnPosition;
        int attempts = 0;

        do
        {
            // Calcula um ponto aleat�rio dentro do paralelep�pedo
            float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
            float randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

            // A posi��o do spawn � o centro do spawner + o desvio aleat�rio
            spawnPosition = transform.position + new Vector3(randomX, 0, randomZ);

            attempts++;

            if (attempts > 20)
            {
                Debug.LogWarning("N�o foi poss�vel encontrar um local de spawn seguro. Pulando este spawn.");
                return;
            }
        }
        while (Vector3.Distance(spawnPosition, playerTransform.position) < playerSafeZoneRadius);

        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
    }
}