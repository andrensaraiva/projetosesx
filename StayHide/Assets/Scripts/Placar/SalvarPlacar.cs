using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SalvarPlacar : MonoBehaviour
{
    public List<ObjPlayerPlacar> listaPlayers = new List<ObjPlayerPlacar>();

    public TextMeshProUGUI texto;

    public static SalvarPlacar instance;
    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        carregarPlayers();
    }

    private void carregarPlayers()
    {
        int totalPlayers = PlayerPrefs.GetInt("TotalPlayers");

        for (int i = 0; i < totalPlayers; i++)
        {
            int pts = PlayerPrefs.GetInt("PontosPlayer" + i);
            string nome = PlayerPrefs.GetString("NomePlayer" + i);
            listaPlayers.Add(new ObjPlayerPlacar(pts, nome));
        }
    }

    public int listarPlayers()
    {
        carregarPlayers();

        texto.text = "";

        int limite = 0;

        int numeroDeNoias = 0;
        foreach (var player in listaPlayers)
        {
            numeroDeNoias++;
            if (limite < 10)
            {
                texto.text += player.getNome() + " - Score: " + player.getPontos() + "\n";
                limite++;
            }
            else
            {
                break;
            }
            
        }

        return numeroDeNoias;
    }

    public void inserirPlayer(int pontos, string nome)
    {
        bool inserido = false;

        for (int i = 0; i < listaPlayers.Count; i++)
        {
            if (pontos > listaPlayers[i].getPontos())
            {
                listaPlayers.Insert(i, new ObjPlayerPlacar(pontos, nome));
                inserido = true;
                break;
            }
        }

        if (!inserido)
        {
            listaPlayers.Add(new ObjPlayerPlacar(pontos, nome));
        }
        salvarPlayers();
    }

    public void salvarPlayers()
    {
        PlayerPrefs.SetInt("TotalPlayers", listaPlayers.Count);

        for (int i = 0; i < listaPlayers.Count; i++)
        {
            PlayerPrefs.SetInt("PontosPlayer" + i, listaPlayers[i].getPontos());
            PlayerPrefs.SetString("NomePlayer" + i, listaPlayers[i].getNome());
        }

        PlayerPrefs.Save();
    }
}
