// ScoreManager2.cs (VERSÃO RENOMEADA)
using UnityEngine;

public class ScoreManager2 : MonoBehaviour // <--- MUDANÇA AQUI
{
    // Singleton para acesso global fácil
    public static ScoreManager2 instance; // <--- MUDANÇA AQUI

    public int currentScore { get; private set; }
    public int highScore { get; private set; }

    public System.Action<int> OnScoreChanged;
    public System.Action<int, Vector3> OnPointsGained;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void AddScore(int points, Vector3 worldPosition)
    {
        if (points <= 0) return;

        currentScore += points;
        Debug.Log($"Pontos adicionados: {points}. Score atual: {currentScore}");

        OnScoreChanged?.Invoke(currentScore);
        OnPointsGained?.Invoke(points, worldPosition);
    }

    public void AddScore(int points)
    {
        AddScore(points, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)));
    }

    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void SaveHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            Debug.Log($"Novo High Score salvo: {highScore}");
        }
    }
}