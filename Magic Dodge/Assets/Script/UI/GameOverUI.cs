// GameOverUI.cs (MODIFICADO)
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;

    // Use OnEnable para garantir que a UI se atualize sempre que a cena for carregada
    void OnEnable()
    {
        if (ScoreManager.instance != null)
        {
            finalScoreText.text = "SCORE: " + ScoreManager.instance.currentScore;
            highScoreText.text = "HIGH SCORE: " + ScoreManager.instance.highScore;
        }
        else
        {
            finalScoreText.text = "SCORE: ???";
            highScoreText.text = "HIGH SCORE: ???";
            Debug.LogError("ScoreManager não encontrado na cena de GameOver!");
        }
    }

    public void GoToMenu()
    {
        // IMPORTANTE: Mude "MenuScene" para o nome real da sua cena de Menu.
        SceneManager.LoadScene("MenuScene");
    }

    public void RestartGame()
    {
        // IMPORTANTE: Mude "GameScene" para o nome real da sua cena de Jogo.
        SceneManager.LoadScene("GameScene");
    }
}