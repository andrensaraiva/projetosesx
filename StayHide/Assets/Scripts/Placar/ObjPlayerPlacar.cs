using UnityEngine;
using UnityEngine.Rendering;

public class ObjPlayerPlacar
{
    private int pontos = 0;

    private string nome;

    public static ObjPlayerPlacar instance;

    public void Awake()
    {
        instance = this;
    }

    public ObjPlayerPlacar(int pontos, string nome)
    {
        this.pontos = pontos;
        this.nome = nome;
    }

    public int getPontos()
    {
        return this.pontos;
    }

    public void setPontos(int pontos)
    {
        this.pontos = pontos;
    }

    public string getNome()
    {
        return this.nome;
    }

    public void setNome(string nome)
    {
        this.nome = nome;
    }
}
