using UnityEngine;
using TMPro; // NECESS�RIO para usar TextMeshPro

public class GameManager : MonoBehaviour
{
    // Vari�vel p�blica para arrastarmos o nosso texto da UI
    public TextMeshProUGUI scoreText;

    // Vari�vel privada para guardar a pontua��o
    private int currentScore;

    void Start()
    {
        // Inicia o jogo com 0 pontos
        currentScore = 0;

        // Atualiza o texto na tela pela primeira vez
        UpdateScoreUI();
    }

    // Fun��o p�blica que outros scripts (como o do inimigo) v�o chamar
    public void AddScore(int pointsToAdd)
    {
        currentScore += pointsToAdd;
        Debug.Log($"Pontua��o: {currentScore}"); // �til para testar no console

        // Atualiza o texto na tela com a nova pontua��o
        UpdateScoreUI();
    }

    public int GetFinalScore()
    {
        return currentScore;
    }

    // Fun��o privada que atualiza o elemento de texto
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Pontos: {currentScore}";
        }
    }
}