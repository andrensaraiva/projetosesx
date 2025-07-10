using System.Collections;
using UnityEngine;

public class PeManager : MonoBehaviour
{
    [Header("Configurações do Spawner")]
    public GameObject pePrefab;      // Arraste o prefab do pé aqui.
    public Transform[] pontosDeSpawn; // Locais onde o pé pode aparecer.

    [Header("Controle de Tempo")]
    public float tempoMinimoEntrePisadas = 5f;
    public float tempoMaximoEntrePisadas = 10f;

    void Start()
    {
        StartCoroutine(RotinaDeSpawnDePes());
    }

    IEnumerator RotinaDeSpawnDePes()
    {
        // Espera um pouco no início do jogo antes da primeira pisada.
        yield return new WaitForSeconds(3f);

        // Loop infinito para continuar gerando pés.
        while (true)
        {
            // Espera um tempo aleatório para a próxima pisada.
            float tempoDeEspera = Random.Range(tempoMinimoEntrePisadas, tempoMaximoEntrePisadas);
            yield return new WaitForSeconds(tempoDeEspera);

            // Sorteia um dos pontos de spawn.
            Transform pontoSorteado = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];

            // Cria uma instância do pé no local sorteado.
            Instantiate(pePrefab, pontoSorteado.position, pontoSorteado.rotation);
        }
    }
}