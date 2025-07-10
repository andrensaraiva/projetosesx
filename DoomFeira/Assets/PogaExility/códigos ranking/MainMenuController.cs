// MainMenuController.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System.Text;
using System.Threading.Tasks;

public class MainMenuController : MonoBehaviour
{
    [Header("UI do Ranking")]
    public TextMeshProUGUI rankingText;

    [Header("Configura��o de Cenas")]
    public string gameSceneName = "SampleScene";

    private void OnEnable()
    {
        LoadRanking();
    }

    // --- O C�DIGO FOI CORRIGIDO AQUI ---
    private async void LoadRanking()
    {
        // 1. Espera o FirebaseManager existir.
        while (FirebaseManager.Instance == null)
        {
            await Task.Yield(); // Espera um frame
        }

        // 2. [A CORRE��O CR�TICA] Espera a TAREFA de inicializa��o ser criada.
        // Isso garante que o Start() do FirebaseManager j� rodou.
        while (FirebaseManager.Instance.InitializationTask == null)
        {
            await Task.Yield(); // Espera mais um frame
        }

        // 3. Agora que sabemos que a tarefa existe, podemos esper�-la terminar.
        await FirebaseManager.Instance.InitializationTask;

        // O resto do c�digo agora � seguro para ser executado.
        if (rankingText != null) rankingText.text = "CARREGANDO RANKING...";

        List<ScoreEntry> topScores = await FirebaseManager.Instance.GetTopScores();

        Debug.Log($"MainMenuController: Foram encontrados {topScores.Count} recordes no Firebase.");

        StringBuilder sb = new StringBuilder("--- HALL OF FAME ---\n\n");
        int count = Mathf.Min(topScores.Count, 10);

        if (count == 0)
        {
            sb.Append("NENHUM RECORDE AINDA!");
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                sb.AppendLine($"{(i + 1)}. {topScores[i].name}   {topScores[i].score}");
            }
        }

        if (rankingText != null)
        {
            rankingText.text = sb.ToString();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Carregando cena: {gameSceneName}");
            SceneManager.LoadScene(gameSceneName);
        }
    }
}