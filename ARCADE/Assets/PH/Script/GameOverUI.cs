// ---- INÍCIO DO CÓDIGO CORRETO ----

using UnityEngine;
using UnityEngine.UI; // ESSA LINHA É CRUCIAL para o tipo "Button" funcionar!
using TMPro;          // Essa linha é para TMP_Text e TMP_InputField
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // --- REFERÊNCIAS DA UI ---
    // O Unity vai criar um campo no Inspector para cada uma dessas variáveis públicas.
    public TMP_Text scoreText;
    public TMP_InputField nameInputField;
    public Button saveButton;

    private int finalScore;

    // Classe estática para passar a pontuação entre as cenas
    public static class GameSession
    {
        public static int score;
    }

    void Start()
    {
        // Pega a pontuação da sessão de jogo e mostra na tela
        finalScore = GameSession.score;
        scoreText.text = "SUA PONTUAÇÃO: " + finalScore.ToString();

        // IMPORTANTE: Garante que o botão não está nulo antes de adicionar o listener
        if (saveButton != null)
        {
            // Adiciona um listener para o botão chamar a função quando for clicado
            saveButton.onClick.AddListener(SaveScoreAndReturnToMenu);
        }
        else
        {
            Debug.LogError("O 'Save Button' não foi atribuído no Inspector!");
        }
    }

    public void SaveScoreAndReturnToMenu()
    {
        // Pega o nome digitado pelo jogador
        string playerName = nameInputField.text;

        // Se o nome estiver em branco, usa um nome padrão
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "JOGADOR";
        }

        // Adiciona a pontuação usando o RankingManager (verifique se você tem esse script no projeto)
        if (RankingManager.Instance != null)
        {
            RankingManager.Instance.AddScore(playerName.ToUpper(), finalScore);
        }

        // Carrega a cena do menu principal
        SceneManager.LoadScene("MENU");
    }
}

// ---- FIM DO CÓDIGO CORRETO ----