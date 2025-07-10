// ScoreUI.cs (MODIFICADO)
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("Componentes da UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    [Header("Pop-up de Pontos")]
    public GameObject pointsPopupPrefab;
    public Transform popupCanvas;

    void Start()
    {
        // Se inscreve nos eventos do ScoreManager2
        if (ScoreManager2.instance != null) // <--- MUDANÇA AQUI
        {
            ScoreManager2.instance.OnScoreChanged += UpdateScoreText; // <--- MUDANÇA AQUI
            ScoreManager2.instance.OnPointsGained += CreatePointsPopup; // <--- MUDANÇA AQUI
        }

        // Atualiza os textos iniciais
        UpdateScoreText(ScoreManager2.instance.currentScore); // <--- MUDANÇA AQUI
        if (highScoreText != null)
        {
            highScoreText.text = $"HI: {ScoreManager2.instance.highScore}"; // <--- MUDANÇA AQUI
        }
    }

    void OnDestroy()
    {
        // Se desinscreve para evitar erros
        if (ScoreManager2.instance != null) // <--- MUDANÇA AQUI
        {
            ScoreManager2.instance.OnScoreChanged -= UpdateScoreText; // <--- MUDANÇA AQUI
            ScoreManager2.instance.OnPointsGained -= CreatePointsPopup; // <--- MUDANÇA AQUI
        }
    }

    // O resto do script (UpdateScoreText, CreatePointsPopup) continua o mesmo...
    private void UpdateScoreText(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString("D6");
        }
    }

    private void CreatePointsPopup(int points, Vector3 worldPosition)
    {
        if (pointsPopupPrefab == null || popupCanvas == null)
        {
            Debug.LogError("Prefab do Pop-up ou Canvas não configurado no ScoreUI!", this);
            return;
        }

        // Instancia o pop-up no Canvas. É CRUCIAL que o segundo argumento seja o Transform do Canvas.
        GameObject popupInstance = Instantiate(pointsPopupPrefab, popupCanvas);

        // --- INÍCIO DA CORREÇÃO ---
        // Força a escala para (1, 1, 1) para garantir que seja visível.
        popupInstance.transform.localScale = Vector3.one;
        // --- FIM DA CORREÇÃO ---

        // Converte a posição do mundo (onde o inimigo morreu) para a posição na tela.
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        popupInstance.transform.position = screenPosition;

        // Define o texto do pop-up
        TextMeshProUGUI popupText = popupInstance.GetComponent<TextMeshProUGUI>();
        if (popupText != null)
        {
            popupText.text = $"+{points}";
        }
        else
        {
            Debug.LogWarning("O prefab do Pop-up não tem um componente TextMeshProUGUI!", popupInstance);
        }
    }

}