using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para usar o SceneManager

public class GameOverSceneController : MonoBehaviour
{
    // O método Awake é chamado assim que a cena carrega, antes de qualquer Start.
    // É o lugar perfeito para resetar o estado do jogo.
    void Awake()
    {
        // Reseta a escala de tempo para 1 (velocidade normal),
        // garantindo que as animações desta cena funcionem.
        Time.timeScale = 1f;
    }

    // Você pode adicionar funções aqui para usar em botões.
    // Por exemplo, um botão de "Reiniciar":
    public void ReiniciarJogo()
    {
        // Certifique-se de que "TESTEJOGO" é o nome exato da sua cena principal.
        SceneManager.LoadScene("TESTEJOGO");
    }

    // Exemplo para um botão de "Voltar ao Menu":
    public void VoltarParaMenu()
    {
        // Certifique-se de que "Menu" é o nome exato da sua cena de menu.
        SceneManager.LoadScene("Menu");
    }
}