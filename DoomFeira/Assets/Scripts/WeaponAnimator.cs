using UnityEngine;
using System.Collections;

public class WeaponAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite idleSprite;
    private Sprite[] shootFrames;
    private float animationSpeed;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Fun��o para ser chamada pelo WeaponStats para configurar a anima��o
    public void SetAnimationData(Sprite idle, Sprite[] shoot, float fps)
    {
        idleSprite = idle;
        shootFrames = shoot;
        // A velocidade da anima��o � baseada na cad�ncia de tiro
        animationSpeed = fps;
    }

    // Inicia a anima��o de tiro
    public void PlayShootAnimation()
    {
        // Para a anima��o anterior se estiver tocando e inicia uma nova
        StopAllCoroutines();
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        // Se n�o houver frames de anima��o, n�o faz nada
        if (shootFrames == null || shootFrames.Length == 0) yield break;

        // Calcula quanto tempo cada frame deve ficar na tela
        float frameDuration = 1f / animationSpeed;

        // Passa por cada frame da anima��o
        foreach (Sprite frame in shootFrames)
        {
            spriteRenderer.sprite = frame;
            yield return new WaitForSeconds(frameDuration);
        }

        // Ao final, volta para o sprite de "parado"
        spriteRenderer.sprite = idleSprite;
    }
}