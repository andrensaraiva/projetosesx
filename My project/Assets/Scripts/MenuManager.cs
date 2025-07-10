using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Coloque o nome da sua cena de jogo aqui no Inspector
    public string nomeDaCenaDoJogo = "GameScene";
    public GameObject creditsPanel; // O campo mais importante para n�s agora

    // Esta fun��o ser� chamada pelo bot�o
    public void IniciarJogo()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }
    public void ShowCreditsPanel()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
    }

    public void HideCreditsPanel()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }
}