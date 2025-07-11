// Classe "wrapper" ou "container" para a lista.
// A Unity lida melhor com a serialização de objetos do que de listas diretamente.
using System.Collections.Generic;

[System.Serializable]
public class RankingData
{
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}