using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Padrão Singleton para ser facilmente acessado de qualquer lugar.
    public static ScreenShake Instance;

    private Vector3 posicaoOriginal;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Guarda a posição original da câmera no início.
        posicaoOriginal = transform.localPosition;
    }

    // Método público que outros scripts chamarão para iniciar o tremor.
    public void IniciarTremor(float duracao, float magnitude)
    {
        // Se já estiver tremendo, para a rotina antiga antes de começar uma nova.
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Tremer(duracao, magnitude));
    }

    private IEnumerator Tremer(float duracao, float magnitude)
    {
        float tempoPassado = 0f;

        while (tempoPassado < duracao)
        {
            // Gera um deslocamento aleatório dentro de um círculo.
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Aplica o deslocamento à posição original da câmera.
            transform.localPosition = posicaoOriginal + new Vector3(x, y, 0);

            tempoPassado += Time.deltaTime;

            yield return null; // Espera o próximo frame.
        }

        // Ao final do tremor, garante que a câmera volte exatamente para a posição original.
        transform.localPosition = posicaoOriginal;
        shakeCoroutine = null;
    }
}