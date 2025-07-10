using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Usaremos um Singleton para que qualquer script possa chamar o tremor facilmente.
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Salva a posi��o original da c�mera.
        // Usamos localPosition para o caso de a c�mera ser filha de outro objeto.
        originalPosition = transform.localPosition;
    }

    // M�todo p�blico que outros scripts chamar�o para iniciar o tremor
    public void StartShake(float duration, float magnitude)
    {
        // Se j� estiver tremendo, para a corrotina antiga antes de come�ar uma nova
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        // Enquanto a dura��o do tremor n�o tiver acabado
        while (elapsed < duration)
        {
            // Gera um deslocamento aleat�rio
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Aplica o deslocamento � posi��o original da c�mera
            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            // Incrementa o tempo decorrido
            elapsed += Time.deltaTime;

            // Espera at� o pr�ximo frame
            yield return null;
        }

        // Ao final, garante que a c�mera volte exatamente para a posi��o original
        transform.localPosition = originalPosition;
        shakeCoroutine = null; // Limpa a refer�ncia da corrotina
    }
}