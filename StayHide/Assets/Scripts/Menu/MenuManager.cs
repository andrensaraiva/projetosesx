using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject[] coroas;
    void Start()
    {
        int nums = SalvarPlacar.instance.listarPlayers();

        for (int i = 0; i < nums;i++)
        {
            if (i < 3)
            {
                coroas[i].SetActive(true);
            }
            else
            {
                break;
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Sair()
    {
        Debug.Log("Saiu");
        Application.Quit();
    }
}
