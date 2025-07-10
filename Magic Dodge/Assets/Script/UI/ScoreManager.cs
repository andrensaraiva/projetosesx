// ScoreManager.cs (MODIFICADO)
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int currentScore { get; private set; }
    public int highScore { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetScore();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        // Opcional: Atualizar um texto de score na tela de jogo aqui.
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void SaveHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            Debug.Log("Novo High Score salvo: " + highScore);
        }
    }
}