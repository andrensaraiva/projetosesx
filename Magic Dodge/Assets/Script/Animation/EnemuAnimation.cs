using UnityEngine;
using DG.Tweening;

public class EnemuAnimation : MonoBehaviour
{
    [Header("Configuração do Tremor")]
    // A força do tremor. Quão longe ele pode se mover do ponto original.
    public float shakeStrength = 0.1f;

    // A duração de cada "tremida".
    public float shakeDuration = 1f;

    // Quão "irregular" é o tremor. Valores mais altos são mais irregulares.
    public int shakeVibrato = 10;

    void Start()
    {
        // Inicia a animação de tremor.
        StartShakeAnimation();
    }

    void StartShakeAnimation()
    {
        // transform.DOShakePosition() é a função para criar o tremor.
        // O primeiro parâmetro é a duração de cada ciclo de tremor.
        // O segundo é a força (distância máxima do centro).
        // O terceiro é a "vibração" (quantas vezes ele muda de direção).
        transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato)
            // .SetLoops(-1) faz com que, ao terminar um ciclo de tremor, ele comece outro imediatamente.
            .SetLoops(-1)
            // .SetEase(Ease.Linear) mantém a intensidade do tremor constante.
            .SetEase(Ease.Linear);
    }
}
