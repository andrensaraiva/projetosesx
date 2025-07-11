using System.Collections.Generic;
using System.IO;
// ---- INÍCIO DO CÓDIGO CORRETO PARA RankingManager.cs ----

using UnityEngine;
using System.Linq; // Importante para usar OrderBy

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    private const string RankingKey = "GameRanking"; // A "chave" para salvar/carregar nos PlayerPrefs
    private const int MaxRankingEntries = 10; // Limita o ranking aos 10 melhores

    void Awake()
    {
        // Configuração do Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garante que este objeto não seja destruído ao carregar novas cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adiciona uma nova pontuação ao ranking.
    /// </summary>
    public void AddScore(string playerName, int newScore)
    {
        RankingData rankingData = LoadRanking();

        // Adiciona a nova entrada
        rankingData.entries.Add(new ScoreEntry { name = playerName, score = newScore });

        // Ordena a lista: do maior score para o menor
        rankingData.entries = rankingData.entries.OrderByDescending(e => e.score).ToList();

        // Remove o excesso, mantendo apenas os melhores
        if (rankingData.entries.Count > MaxRankingEntries)
        {
            rankingData.entries.RemoveRange(MaxRankingEntries, rankingData.entries.Count - MaxRankingEntries);
        }

        SaveRanking(rankingData);
    }

    /// <summary>
    /// Retorna a lista de scores salvos.
    /// </summary>
    public List<ScoreEntry> GetRankingEntries()
    {
        return LoadRanking().entries;
    }

    private void SaveRanking(RankingData rankingData)
    {
        // Converte o objeto para uma string JSON
        string json = JsonUtility.ToJson(rankingData);
        // Salva a string nos PlayerPrefs
        PlayerPrefs.SetString(RankingKey, json);
        PlayerPrefs.Save(); // Força o salvamento imediato no disco
    }

    private RankingData LoadRanking()
    {
        // Carrega a string JSON dos PlayerPrefs
        string json = PlayerPrefs.GetString(RankingKey, "{}"); // Retorna "{}" se não houver nada salvo

        // Converte a string JSON de volta para um objeto
        RankingData rankingData = JsonUtility.FromJson<RankingData>(json);

        // Garante que a lista dentro do objeto nunca seja nula
        if (rankingData.entries == null)
        {
            rankingData.entries = new List<ScoreEntry>();
        }

        return rankingData;
    }

    // Ferramenta útil para testes! Clicando com o botão direito no componente no Inspector, você pode limpar os dados.
    [ContextMenu("Limpar Dados do Ranking")]
    public void ClearRankingData()
    {
        PlayerPrefs.DeleteKey(RankingKey);
        PlayerPrefs.Save();
        Debug.Log("Dados do ranking foram limpos!");
    }
}