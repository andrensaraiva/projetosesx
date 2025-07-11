using System.Collections;
using System.Collections.Generic; // Precisamos disso para usar List<T>
using UnityEngine;

public class PeManager_Progressive : MonoBehaviour
{
    [Header("Configura��o B�sica")]
    [Tooltip("O prefab do objeto 'P�' que ser� instanciado.")]
    public GameObject pePrefab;
    [Tooltip("Array com todos os locais poss�veis onde o p� pode aparecer.")]
    public Transform[] pontosDeSpawn;

    [Header("Controle de Dificuldade")]
    [Tooltip("Tempo em segundos para atingir a dificuldade m�xima.")]
    public float timeToReachMaxDifficulty = 120f;

    [Header("Frequ�ncia das Pisadas")]
    [Tooltip("Tempo M�NIMO de espera no in�cio do jogo.")]
    public float initialMinWait = 8f;
    [Tooltip("Tempo M�XIMO de espera no in�cio do jogo.")]
    public float initialMaxWait = 12f;

    [Tooltip("Tempo M�NIMO de espera na dificuldade m�xima.")]
    public float finalMinWait = 1.5f;
    [Tooltip("Tempo M�XIMO de espera na dificuldade m�xima.")]
    public float finalMaxWait = 3f;

    [Header("Pisadas Duplas")]
    [Range(0, 1)]
    [Tooltip("Chance (de 0 a 1) de uma pisada dupla ocorrer na dificuldade m�xima. 0.75 = 75% de chance.")]
    public float chanceDeSpawnDuplo = 0.75f;

    private float startTime;

    void Start()
    {
        startTime = Time.time;

        // Verifica��o de seguran�a: pisadas duplas s� funcionam com 2 ou mais pontos.
        if (pontosDeSpawn.Length < 2)
        {
            Debug.LogWarning("PeManager: Menos de 2 pontos de spawn foram definidos. A funcionalidade de p� duplo ser� desativada.");
            chanceDeSpawnDuplo = 0; // Desativa a chance de spawn duplo se for imposs�vel.
        }

        StartCoroutine(RotinaDeSpawnDePes());
    }

    IEnumerator RotinaDeSpawnDePes()
    {
        // Espera um pouco no in�cio do jogo antes da primeira pisada.
        yield return new WaitForSeconds(5f);

        while (true)
        {
            // --- C�LCULO DA DIFICULDADE ATUAL ---
            float timeElapsed = Time.time - startTime;
            float difficultyCurve = Mathf.Clamp01(timeElapsed / timeToReachMaxDifficulty);

            // 1. Calcula o tempo de espera atual, diminuindo com o tempo.
            float currentMinWait = Mathf.Lerp(initialMinWait, finalMinWait, difficultyCurve);
            float currentMaxWait = Mathf.Lerp(initialMaxWait, finalMaxWait, difficultyCurve);
            float tempoDeEspera = Random.Range(currentMinWait, currentMaxWait);
            yield return new WaitForSeconds(tempoDeEspera);

            // --- L�GICA DE SPAWN (SIMPLES OU DUPLO) ---

            // 2. Calcula a chance atual de um spawn duplo acontecer.
            float chanceDuploAtual = Mathf.Lerp(0, chanceDeSpawnDuplo, difficultyCurve);

            // Se o sorteio for bem-sucedido E tivermos pontos suficientes...
            if (Random.value < chanceDuploAtual && pontosDeSpawn.Length >= 2)
            {
                SpawnarPesDuplos();
            }
            else
            {
                SpawnarPeUnico();
            }
        }
    }

    void SpawnarPeUnico()
    {
        // L�gica original: sorteia um ponto e cria o p�.
        Transform pontoSorteado = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];
        Instantiate(pePrefab, pontoSorteado.position, pontoSorteado.rotation);
    }

    void SpawnarPesDuplos()
    {
        // L�gica para garantir que os dois p�s n�o usem o mesmo spawn.

        // 1. Criamos uma lista com os �ndices de todos os pontos de spawn dispon�veis.
        List<int> indicesDisponiveis = new List<int>();
        for (int i = 0; i < pontosDeSpawn.Length; i++)
        {
            indicesDisponiveis.Add(i);
        }

        // 2. Sorteamos o primeiro �ndice da lista.
        int indiceSorteado1 = Random.Range(0, indicesDisponiveis.Count);
        int pontoIndex1 = indicesDisponiveis[indiceSorteado1];

        // **A CHAVE:** Removemos o �ndice j� usado da lista.
        indicesDisponiveis.RemoveAt(indiceSorteado1);

        // 3. Sorteamos o segundo �ndice da lista restante (que agora n�o cont�m mais o primeiro).
        int indiceSorteado2 = Random.Range(0, indicesDisponiveis.Count);
        int pontoIndex2 = indicesDisponiveis[indiceSorteado2];

        // 4. Instanciamos os dois p�s em seus locais distintos.
        Transform ponto1 = pontosDeSpawn[pontoIndex1];
        Instantiate(pePrefab, ponto1.position, ponto1.rotation);

        Transform ponto2 = pontosDeSpawn[pontoIndex2];
        Instantiate(pePrefab, ponto2.position, ponto2.rotation);
    }
}