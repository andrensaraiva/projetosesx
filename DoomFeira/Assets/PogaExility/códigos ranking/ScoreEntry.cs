// ScoreEntry.cs

[System.Serializable] // Permite que a Unity entenda e serialize esta classe.
public class ScoreEntry
{
    public string name; // O nome do jogador (3 letras).
    public int score;   // A pontua��o do jogador.

    // Construtor vazio. OBRIGAT�RIO para o Firebase/JSON conseguir criar o objeto.
    public ScoreEntry() { }

    // Construtor para facilitar a cria��o de novos registros no c�digo.
    public ScoreEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}