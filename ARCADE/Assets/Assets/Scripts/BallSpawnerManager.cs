using System.Collections;
using UnityEngine;

public class BallSpawnerManager_Balanced : MonoBehaviour
{
    [Header("Configuração Básica")]
    [Tooltip("Coloque aqui todos os prefabs de bolas. A ordem não importa mais.")]
    public GameObject[] ballPrefabs;

    [Tooltip("Pontos onde as bolas podem nascer.")]
    public Transform[] spawnPoints;

    [Header("Controle de Dificuldade")]
    [Tooltip("Tempo em segundos para atingir a dificuldade máxima. Valores maiores = curva mais lenta.")]
    public float timeToReachMaxDifficulty = 180f; // Sugestão: 3 minutos

    [Tooltip("Intervalo de spawn no início do jogo.")]
    public float initialSpawnInterval = 3.5f; // Começa um pouco mais lento

    [Tooltip("Intervalo de spawn na dificuldade máxima (o menor possível).")]
    public float minSpawnInterval = 1.2f; // Mais tempo para o jogador respirar

    [Header("Bolas Extras")]
    [Range(0, 1)]
    [Tooltip("Na dificuldade máxima, qual a chance (0 a 1) de uma SEGUNDA bola aparecer junto com a primeira?")]
    public float chanceDeSpawnDuploNoMaximo = 0.3f; // 30% de chance no pico da dificuldade

    private float startTime;

    void Start()
    {
        // Validação para garantir que o spawner pode funcionar.
        if (ballPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("BallSpawnerManager: Prefabs de bola ou pontos de spawn não foram configurados! Desativando spawner.");
            this.enabled = false;
            return;
        }

        startTime = Time.time;
        StartCoroutine(SpawnBallsRoutine());
    }

    IEnumerator SpawnBallsRoutine()
    {
        while (true)
        {
            // --- CÁLCULO DA DIFICULDADE ATUAL ---
            float timeElapsed = Time.time - startTime;
            float difficultyCurve = Mathf.Clamp01(timeElapsed / timeToReachMaxDifficulty);

            // 1. A dificuldade principal vem da diminuição do tempo de espera.
            float currentSpawnInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, difficultyCurve);
            yield return new WaitForSeconds(currentSpawnInterval);


            // --- SPAWN DA BOLA PRINCIPAL (SEMPRE ACONTECE) ---
            SpawnSingleBall();


            // --- CHANCE DE SPAWNAR UMA BOLA EXTRA ---

            // 2. Calculamos a chance atual de um spawn duplo, que cresce com o tempo.
            float chanceDuploAtual = Mathf.Lerp(0, chanceDeSpawnDuploNoMaximo, difficultyCurve);

            // Se o sorteio (um número aleatório entre 0.0 e 1.0) for menor que a chance atual...
            if (Random.value < chanceDuploAtual)
            {
                // ...spawnamos uma segunda bola!
                // Usamos um pequeno delay para não aparecerem exatamente no mesmo frame.
                yield return new WaitForSeconds(0.1f);
                SpawnSingleBall();
            }
        }
    }

    /// <summary>
    /// Sorteia uma bola e um local e a instancia no jogo.
    /// </summary>
    void SpawnSingleBall()
    {
        // 3. Sorteia QUALQUER bola da lista, desde o início.
        GameObject randomBallPrefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];

        // Sorteia qualquer ponto de spawn.
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instancia a bola.
        GameObject newBall = Instantiate(randomBallPrefab, randomSpawnPoint.position, Quaternion.identity);

        // Define a direção inicial.
        Vector2 direcaoImpulso = (randomSpawnPoint.position.x < 0) ? Vector2.right : Vector2.left;

        BolaController controller = newBall.GetComponent<BolaController>();
        if (controller != null)
        {
            controller.InicializarParaSpawner(direcaoImpulso);
        }
    }
}