using UnityEngine;
using UnityEngine.UI; // Essencial para componentes de UI como Image
using System.Collections; // Essencial para Coroutines

// Esta linha garante que o script s� pode ser adicionado a um objeto que j� tenha um componente Image.
[RequireComponent(typeof(Image))]
public class UISpriteAnimator : MonoBehaviour
{
    private Image imageComponent;
    private Coroutine currentAnimationCoroutine; // Guarda a refer�ncia da anima��o que est� tocando

    void Awake()
    {
        // Pega o componente Image no mesmo objeto para podermos trocar os sprites
        imageComponent = GetComponent<Image>();
    }

    // Fun��o p�blica que o HUDManager vai chamar para iniciar uma nova anima��o
    public void PlayAnimation(Sprite[] frames, float fps, bool loop)
    {
        // Se j� houver uma anima��o tocando, pare-a para n�o criar conflitos
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
        }

        // Inicia a nova anima��o e guarda sua refer�ncia
        currentAnimationCoroutine = StartCoroutine(AnimateRoutine(frames, fps, loop));
    }

    // A rotina (Coroutine) que efetivamente faz a anima��o acontecer
    private IEnumerator AnimateRoutine(Sprite[] frames, float fps, bool loop)
    {
        // Verifica se h� algo para animar
        if (frames == null || frames.Length == 0 || fps <= 0)
        {
            // Se houver pelo menos um frame, mostra ele como uma imagem est�tica e para
            if (frames != null && frames.Length > 0) imageComponent.sprite = frames[0];
            yield break; // Encerra a rotina
        }

        int index = 0;
        float frameDuration = 1f / fps; // Calcula quanto tempo cada frame deve ficar na tela

        while (true) // Um loop que ser� controlado pela l�gica interna
        {
            // Mostra o frame atual na tela
            imageComponent.sprite = frames[index];

            // Avan�a o �ndice para o pr�ximo frame
            index++;

            // Verifica se a anima��o chegou ao fim
            if (index >= frames.Length)
            {
                if (loop)
                {
                    index = 0; // Se for para repetir, volta para o in�cio
                }
                else
                {
                    yield break; // Se n�o for para repetir, encerra a anima��o aqui
                }
            }

            // Espera a dura��o do frame antes de continuar o loop
            yield return new WaitForSeconds(frameDuration);
        }
    }
}