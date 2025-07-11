// Crie este script e anexe a um GameObject na cena MainMenu (pode ser o Canvas)
// UIMainMenu.cs

using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UIMainMenu : MonoBehaviour
{
    public Transform rankingContainer; // O pai onde as entradas ser�o criadas
    public GameObject rankingEntryPrefab; // O prefab que criamos

    // Fontes e tamanhos para o Top 3
    public TMP_FontAsset top1Font;
    public float top1FontSize = 40f;
    public TMP_FontAsset top2Font;
    public float top2FontSize = 35f;
    public TMP_FontAsset top3Font;
    public float top3FontSize = 30f;
    public TMP_FontAsset defaultFont;
    public float defaultFontSize = 25f;

    void Start()
    {
        DisplayRanking();
    }

    public void DisplayRanking()
    {
        // 1. Limpa o ranking antigo para evitar duplicatas
        foreach (Transform child in rankingContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Pega as entradas do RankingManager (que agora existe!)
        if (RankingManager.Instance == null)
        {
            Debug.LogError("RankingManager n�o encontrado na cena!");
            return;
        }
        List<ScoreEntry> entries = RankingManager.Instance.GetRankingEntries();

        // 3. Define quantas entradas mostrar (o n�mero de entradas salvas OU 3, o que for MENOR)
        int numEntriesToShow = Mathf.Min(entries.Count, 3);

        // 4. Cria um objeto de UI para cada entrada (LOOP SEGURO)
        for (int i = 0; i < numEntriesToShow; i++)
        {
            GameObject entryObject = Instantiate(rankingEntryPrefab, rankingContainer);
            ScoreEntry currentEntry = entries[i];

            TMP_Text nameText = entryObject.transform.Find("NameText").GetComponent<TMP_Text>();
            TMP_Text scoreText = entryObject.transform.Find("ScoreText").GetComponent<TMP_Text>();

            nameText.text = currentEntry.name;
            scoreText.text = currentEntry.score.ToString();

            // Aplica estilos especiais para o Top 3
            if (i == 0) // 1� Lugar
            {
                SetTextStyle(nameText, top1Font, top1FontSize, Color.yellow);
                SetTextStyle(scoreText, top1Font, top1FontSize, Color.yellow);
            }
            else if (i == 1) // 2� Lugar
            {
                SetTextStyle(nameText, top2Font, top2FontSize, new Color(0.75f, 0.75f, 0.75f)); // Prata
                SetTextStyle(scoreText, top2Font, top2FontSize, new Color(0.75f, 0.75f, 0.75f));
            }
            else if (i == 2) // 3� Lugar
            {
                SetTextStyle(nameText, top3Font, top3FontSize, new Color(0.8f, 0.5f, 0.2f)); // Bronze
                SetTextStyle(scoreText, top3Font, top3FontSize, new Color(0.8f, 0.5f, 0.2f));
            }
        }
    }

    // Fun��o auxiliar para definir o estilo do texto
    private void SetTextStyle(TMP_Text text, TMP_FontAsset font, float size, Color color)
    {
        if (font != null) text.font = font;
        text.fontSize = size;
        text.color = color;
    }
}