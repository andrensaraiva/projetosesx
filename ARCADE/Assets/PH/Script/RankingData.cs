// Crie este script na sua pasta de Scripts
// RankingData.cs

using System.Collections.Generic;

[System.Serializable]
public class RankingData
{
    public List<ScoreEntry> entries;

    public RankingData()
    {
        entries = new List<ScoreEntry>();
    }
}