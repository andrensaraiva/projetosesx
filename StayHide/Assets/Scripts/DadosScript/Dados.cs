using TMPro;
using UnityEngine;

public class Dados : MonoBehaviour
{
    public static Dados instance;

    public TextMeshProUGUI scoreText;

    public int pontosTotais = 0;

    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        scoreText.text = "Score:" + pontosTotais;   
    }
    public void addPonto()
    {
        pontosTotais++;
        scoreText.text = "Score: " + pontosTotais;
    }

    public int getScore()
    {
        return pontosTotais;
    }
}
