using System.Collections;
using UnityEngine;

public class BallSpawnerManager : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;

    void Start()
    {
        StartCoroutine(SpawnBallsRoutine());
    }

    IEnumerator SpawnBallsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Aqui está a chave: o spawner pode sortear QUALQUER bola, grande ou pequena
            GameObject randomBallPrefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];

            GameObject newBall = Instantiate(randomBallPrefab, randomSpawnPoint.position, Quaternion.identity);

            Vector2 direcaoImpulso = (randomSpawnPoint.position.x < 0) ? Vector2.right : Vector2.left;

            BolaController controller = newBall.GetComponent<BolaController>();
            if (controller != null)
            {
                // Chamamos o método específico para bolas que vêm do spawner!
                controller.InicializarParaSpawner(direcaoImpulso);
            }
        }
    }
}