// Estrutura simples para guardar uma entrada de score.
// [System.Serializable] permite que a Unity converta isso para JSON.
[System.Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
}