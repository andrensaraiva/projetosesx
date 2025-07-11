using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DerrotaScript : MonoBehaviour
{
    public GameObject player;
    public GameObject painelFinal;

    private char[] letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(); // Alfabeto

    private int[] PosLetra = { 0, 0, 0 }; // �ndice da letra para cada posi��o da sigla

    public TextMeshProUGUI ScoreFinal;
    public TextMeshProUGUI[] Sigla; // Textos das letras da sigla (3 posi��es)

    public static DerrotaScript instance;

    public GameObject[] objSumir;

    private int score;

    public void Awake()
    {
        instance = this;
    }

    public void FimDeJogo(int score)
    {
        this.score = score;
        ScoreFinal.text = "Pontos Totais: " + score;
        painelFinal.SetActive(true);
        Time.timeScale = 0;

        for (int i = 0; i < objSumir.Length; i++)
        {
            objSumir[i].SetActive(false);
        }

        // Reinicia a sigla como "A A A"
        for (int i = 0; i < 3; i++)
        {
            PosLetra[i] = 0; // �ndice da letra A
            Sigla[i].text = letras[PosLetra[i]].ToString();
        }
    }

    // Aumenta o �ndice da letra da posi��o escolhida (0, 1 ou 2)
    public void subirSelec(int pos)
    {
        if (pos < 0 || pos >= PosLetra.Length) return;

        PosLetra[pos]++;
        if (PosLetra[pos] >= letras.Length)
            PosLetra[pos] = 0;

        Sigla[pos].text = letras[PosLetra[pos]].ToString();
    }

    // Diminui o �ndice da letra da posi��o escolhida (0, 1 ou 2)
    public void descerSelec(int pos)
    {
        if (pos < 0 || pos >= PosLetra.Length) return;

        PosLetra[pos]--;
        if (PosLetra[pos] < 0)
            PosLetra[pos] = letras.Length - 1;

        Sigla[pos].text = letras[PosLetra[pos]].ToString();
    }

    // Salva o nome e reinicia o jogo
    public void salvarEFinalizar()
    {
        string nome = Sigla[0].text + " " +Sigla[1].text + " " + Sigla[2].text;
        SalvarPlacar.instance.inserirPlayer(score, nome);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
