// MenuUI.cs (NOVO SCRIPT)
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        if (ScoreManager.instance != null)
        {
            highScoreText.text = "HIGH SCORE: " + ScoreManager.instance.highScore;
        }
    }

    public void StartGame()
    {
        // Garanta que o nome da sua cena de jogo é "GameScene" ou mude aqui
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Jogo Fechado!"); // Para teste no editor
    }
}