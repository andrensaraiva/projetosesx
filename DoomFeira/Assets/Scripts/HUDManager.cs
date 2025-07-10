using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Necess�rio para usar Listas

// A classe de estado de anima��o que definimos acima
[System.Serializable]
public class FaceAnimationState
{
    public string stateName;
    public Sprite[] frames;
    public float framesPerSecond = 8f;
    public bool loop = true;
}

public class HUDManager : MonoBehaviour
{
    [Header("Stats")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI ammoText;

    [Header("Face")]
    public UISpriteAnimator faceAnimator; // <<-- MUDAN�A: Refer�ncia para o nosso novo animador
    public List<FaceAnimationState> faceAnimations; // <<-- MUDAN�A: Lista para todas as anima��es do rosto

    private string currentFaceState = ""; // Guarda o estado atual para evitar reiniciar a mesma anima��o

    // Fun��o para atualizar vida e armadura
    public void UpdateStatus(int health, int armor)
    {
        if (healthText != null) healthText.text = $"{health}%";
        if (armorText != null) armorText.text = $"{armor}%";

        // --- L�GICA DO ROSTO MODIFICADA ---
        string newStateName = GetStateNameForHealth(health);

        // Se o estado de sa�de mudou (ex: de Saudavel para Machucado), toca a nova anima��o
        if (newStateName != currentFaceState)
        {
            currentFaceState = newStateName;

            // Encontra a anima��o correspondente na nossa lista pelo nome
            FaceAnimationState stateToPlay = faceAnimations.Find(anim => anim.stateName == newStateName);

            if (stateToPlay != null && faceAnimator != null)
            {
                // Diz ao animador para tocar os frames desta anima��o com as configura��es dela
                faceAnimator.PlayAnimation(stateToPlay.frames, stateToPlay.framesPerSecond, stateToPlay.loop);
            }
        }
    }

    // Fun��o auxiliar que decide o nome do estado baseado na porcentagem de vida
    private string GetStateNameForHealth(int health)
    {
        if (health > 80) return "Saudavel";
        if (health > 60) return "LevementeMachucado";
        if (health > 40) return "Machucado";
        if (health > 20) return "MuitoMachucado";
        return "Morrendo";
    }

    // ... suas outras fun��es (UpdateAmmo, etc.) continuam aqui ...
    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }
}