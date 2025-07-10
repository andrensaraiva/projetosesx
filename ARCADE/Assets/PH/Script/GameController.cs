using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para mudar de cena

public class GameController : MonoBehaviour
{
    // Esta função precisa ser 'public' para que o botão consiga "vê-la"
    public void EndGame()
    {
        // 1. Simula uma pontuação aleatória e a guarda na variável estática
        // Lembre-se que seu script se chama 'GameOverUI', então usamos ele aqui.
        GameOverUI.GameSession.score = Random.Range(100, 10000);

        // 2. Carrega a cena de Game Over
        SceneManager.LoadScene("GameOver");
    }

    // Você pode adicionar outras lógicas do seu jogo aqui depois
    // como contar pontos, controlar o jogador, etc.
}