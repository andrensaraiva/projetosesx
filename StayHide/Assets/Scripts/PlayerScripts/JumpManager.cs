using System.Collections;
using UnityEngine;

public class JumpManager : MonoBehaviour
{
    public static JumpManager instance;

    [Header("Referências")]
    public GameObject falsoPlayer; // O objeto visual que vai "pular"
    public Transform sombra;       // A sombra que vai encolher

    [Header("Configurações do Pulo")]
    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.5f;

    [Range(0.1f, 1f)]
    public float shadowMinScale = 0.4f; // Escala mínima da sombra (40%)

    // --- Estado ---
    public bool onGround = true;
    private Vector3 originalFalsoPlayerPos;
    private Vector3 originalSombraScale;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Guarda as posições/escalas originais para resetar corretamente
        if (falsoPlayer != null) originalFalsoPlayerPos = falsoPlayer.transform.localPosition;
        if (sombra != null) originalSombraScale = sombra.transform.localScale;
    }

    // Função pública para iniciar o pulo
    public void pular()
    {
        // Só pula se estiver no chão
        if (onGround)
        {
            StartCoroutine(JumpCoroutine());
        }
    }

    // Coroutine otimizada do pulo
    IEnumerator JumpCoroutine()
    {
        onGround = false;
        float timer = 0f;

        while (timer <= jumpDuration)
        {
            timer += Time.deltaTime;
            float percent = Mathf.Clamp01(timer / jumpDuration);

            // 1. CURVA DO PULO (ARCO SUAVE)
            // Mathf.Sin(percent * Mathf.PI) cria uma curva suave de 0 -> 1 -> 0
            float curve = Mathf.Sin(percent * Mathf.PI);
            falsoPlayer.transform.localPosition = originalFalsoPlayerPos + new Vector3(0, curve * jumpHeight, 0);

            // 2. ESCALA DA SOMBRA
            // A sombra encolhe até o mínimo no pico do pulo e depois volta ao normal
            float shadowScale = Mathf.Lerp(originalSombraScale.x, originalSombraScale.x * shadowMinScale, curve);
            sombra.localScale = new Vector3(shadowScale, shadowScale, originalSombraScale.z);

            yield return null;
        }

        // Garante que tudo volte ao estado original no final
        falsoPlayer.transform.localPosition = originalFalsoPlayerPos;
        sombra.localScale = originalSombraScale;
        onGround = true;
    }

    public bool getOnGround()
    {
        return onGround;
    }
}