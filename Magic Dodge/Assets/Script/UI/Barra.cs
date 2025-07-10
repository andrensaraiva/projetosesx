// Barra.cs (VERSÃO COM CORREÇÃO DE RectTransform)
using UnityEngine;
using TMPro;

[System.Serializable]
public class PowerIconMapping
{
    public PowerType powerType;
    public GameObject iconPrefab;
}

public class Barra : MonoBehaviour
{
    [Header("Componentes da UI")]
    public TextMeshProUGUI waveNumberText;

    [Header("Configuração dos Ícones")]
    [Tooltip("O local na UI onde o ícone do poder deve aparecer.")]
    public Transform powerIconContainer;
    [Tooltip("Associe cada tipo de poder ao seu prefab de animação.")]
    public PowerIconMapping[] powerIcons;

    private GameObject currentPowerIconInstance;

    public void SetupNewWave(int waveNumber, float duration, PowerType powerType)
    {
        if (waveNumberText != null)
        {
            waveNumberText.text = "WAVE " + waveNumber;
        }

        if (currentPowerIconInstance != null)
        {
            Destroy(currentPowerIconInstance);
        }

        GameObject prefabToInstantiate = FindPrefabForPower(powerType);

        if (prefabToInstantiate != null && powerIconContainer != null)
        {
            // Instancia o prefab como filho do container
            currentPowerIconInstance = Instantiate(prefabToInstantiate, powerIconContainer);

            // --- INÍCIO DA CORREÇÃO CRÍTICA ---
            // Garante que o ícone instanciado tenha as propriedades corretas de UI.
            RectTransform iconRect = currentPowerIconInstance.GetComponent<RectTransform>();
            if (iconRect != null)
            {
                // Reseta a posição, escala e rotação para valores padrão dentro do container.
                iconRect.anchoredPosition = Vector2.zero;
                iconRect.localScale = Vector3.one;
                iconRect.localRotation = Quaternion.identity;

                // Força o ícone a preencher todo o espaço do container (opcional, mas geralmente desejado)
                iconRect.anchorMin = new Vector2(0, 0);
                iconRect.anchorMax = new Vector2(1, 1);
                iconRect.sizeDelta = Vector2.zero;
            }
            // --- FIM DA CORREÇÃO CRÍTICA ---
        }

        StopAllCoroutines();
        if (duration > 0)
        {
            StartCoroutine(WaveTimerCoroutine(duration));
        }
    }

    // Função auxiliar para deixar o código mais limpo
    private GameObject FindPrefabForPower(PowerType powerType)
    {
        foreach (var mapping in powerIcons)
        {
            if (mapping.powerType == powerType)
            {
                return mapping.iconPrefab;
            }
        }
        return null; // Retorna nulo se nenhum prefab for encontrado
    }

    private System.Collections.IEnumerator WaveTimerCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        WaveSpawner.instance?.OnWaveTimerEnd();
    }
}