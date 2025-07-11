using System.Collections;
using UnityEngine;

public class BallSpawnerManager_Balanced : MonoBehaviour
{
    [Header("Configura��o B�sica")]
    [Tooltip("Coloque aqui todos os prefabs de bolas. A ordem n�o importa mais.")]
    public GameObject[] ballPrefabs;

    [Tooltip("Pontos onde as bolas podem nascer.")]
    public Transform[] spawnPoints;

    [Header("Controle de Dificuldade")]
    [Tooltip("Tempo em segundos para atingir a dificuldade m�xima. Valores maiores = curva mais lenta.")]
    public float timeToReachMaxDifficulty = 180f; // Sugest�o: 3 minutos

    [Tooltip("Intervalo de spawn no in�cio do jogo.")]
    public float initialSpawnInterval = 3.5f; // Come�a um pouco mais lento

    [Tooltip("Intervalo de spawn na dificuldade m�xima (o menor poss�vel).")]
    public float minSpawnInterval = 1.2f; // Mais tempo para o jogador respirar

    [Header("Bolas Extras")]
    [Range(0, 1)]
    [Tooltip("Na dificuldade m�xima, qual a chance (0 a 1) de uma SEGUNDA bola aparecer junto com a primeira?")]
    public float chanceDeSpawnDuploNoMaximo = 0.3f; // 30% de chance no pico da dificuldade

    private float startTime;

    void Start()
    {
        // Valida��o para garantir que o spawner pode funcionar.
        if (ballPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("BallSpawnerManager: Prefabs de bola ou pontos de spawn n�o foram configurados! Desativando spawner.");
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
            // --- C�LCULO DA DIFICULDADE ATUAL ---
            float timeElapsed = Time.time - startTime;
            float difficultyCurve = Mathf.Clamp01(timeElapsed / timeToReachMaxDifficulty);

            // 1. A dificuldade principal vem da diminui��o do tempo de espera.
            float currentSpawnInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, difficultyCurve);
            yield return new WaitForSeconds(currentSpawnInterval);


            // --- SPAWN DA BOLA PRINCIPAL (SEMPRE ACONTECE) ---
            SpawnSingleBall();


            // --- CHANCE DE SPAWNAR UMA BOLA EXTRA ---

            // 2. Calculamos a chance atual de um spawn duplo, que cresce com o tempo.
            float chanceDuploAtual = Mathf.Lerp(0, chanceDeSpawnDuploNoMaximo, difficultyCurve);

            // Se o sorteio (um n�mero aleat�rio entre 0.0 e 1.0) for menor que a chance atual...
            if (Random.value < chanceDuploAtual)
            {
                // ...spawnamos uma segunda bola!
                // Usamos um pequeno delay para n�o aparecerem exatamente no mesmo frame.
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
        // 3. Sorteia QUALQUER bola da lista, desde o in�cio.
        GameObject randomBallPrefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];

        // Sorteia qualquer ponto de spawn.
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instancia a bola.
        GameObject newBall = Instantiate(randomBallPrefab, randomSpawnPoint.position, Quaternion.identity);

        // Define a dire��o inicial.
        Vector2 direcaoImpulso = (randomSpawnPoint.position.x < 0) ? Vector2.right : Vector2.left;

        BolaController controller = newBall.GetComponent<BolaController>();
        if (controller != null)
        {
            controller.InicializarParaSpawner(direcaoImpulso);
        }
    }
}