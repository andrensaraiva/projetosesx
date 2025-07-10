using System.Collections.Generic;
using System.IO;
// ---- INÍCIO DO CÓDIGO CORRETO PARA RankingManager.cs ----

using UnityEngine;
using System.Linq; // Importante para usar OrderBy

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance; // Padrão Singleton

    private RankingData rankingData;
    private string savePath;

    void Awake()
    {
        // Configuração do Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Não destruir ao carregar nova cena
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Define o caminho do arquivo de save
        savePath = Path.Combine(Application.persistentDataPath, "ranking.json");

        LoadRanking();
    }

    private void LoadRanking()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            rankingData = JsonUtility.FromJson<RankingData>(json);
            Debug.Log("Ranking carregado com sucesso!");
        }
        else
        {
            rankingData = new RankingData();
            Debug.Log("Nenhum ranking encontrado. Criando um novo.");
        }
    }

    private void SaveRanking()
    {
        // Ordena a lista em ordem decrescente de pontuação antes de salvar
        if (rankingData != null && rankingData.entries != null)
        {
            rankingData.entries = rankingData.entries.OrderByDescending(e => e.score).ToList();
        }

        string json = JsonUtility.ToJson(rankingData, true); // 'true' para formatar o JSON
        File.WriteAllText(savePath, json);
        Debug.Log("Ranking salvo em: " + savePath);
    }

    public void AddScore(string name, int score)
    {
        ScoreEntry newEntry = new ScoreEntry { name = name, score = score };
        rankingData.entries.Add(newEntry);
        SaveRanking(); // Salva e ordena a lista
    }

    public List<ScoreEntry> GetRankingEntries()
    {
        // Garante que nunca retorna nulo, para evitar erros em outros scripts
        if (rankingData == null || rankingData.entries == null)
        {
            return new List<ScoreEntry>();
        }
        return rankingData.entries;
    }
}

// ---- FIM DO CÓDIGO CORRETO ----