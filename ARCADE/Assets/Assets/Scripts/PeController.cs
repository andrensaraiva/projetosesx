using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PeController : MonoBehaviour
{
    [Header("Configurações da Pisada")]
    public float alturaDoAviso = 2f;
    public float alturaFinal = -4f;
    public float tempoDeAviso = 2f;

    [Header("Velocidades")]
    public float tempoParaDescerAteAviso = 0.5f; // Tempo para a primeira descida
    public float tempoParaPisadaFinal = 0.15f; // Tempo para a pisada (bem rápido)
    public float tempoParaSubir = 1f;      // Tempo para recuar

    [Header("Efeitos")]
    public float duracaoDoTremor = 0.25f;
    public float magnitudeDoTremor = 0.1f;
    public AudioClip somDaPisada;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(CicloDePisada());
    }

    IEnumerator CicloDePisada()
    {
        Vector3 posicaoInicial = transform.position;
        Vector3 posicaoDeAviso = new Vector3(posicaoInicial.x, alturaDoAviso, 0);
        Vector3 posicaoFinal = new Vector3(posicaoInicial.x, alturaFinal, 0);

        // --- FASE 1: DESCIDA PARA O AVISO ---
        yield return MoverPara(posicaoInicial, posicaoDeAviso, tempoParaDescerAteAviso);

        // --- FASE 2: ESPERA ---
        yield return new WaitForSeconds(tempoDeAviso);

        // --- FASE 3: PISADA FINAL ---
        yield return MoverPara(posicaoDeAviso, posicaoFinal, tempoParaPisadaFinal);

        // --- FASE 4: IMPACTO ---
        if (somDaPisada != null)
        {
            audioSource.PlayOneShot(somDaPisada);
        }
        if (ScreenShake.Instance != null)
        {
            ScreenShake.Instance.IniciarTremor(duracaoDoTremor, magnitudeDoTremor);
        }
        yield return new WaitForSeconds(0.2f); // Pausa no chão

        // --- FASE 5: RECUO ---
        yield return MoverPara(posicaoFinal, posicaoInicial, tempoParaSubir);

        // --- FASE 6: AUTODESTRUIÇÃO ---
        Destroy(gameObject);
    }

    // --- NOVA FUNÇÃO DE MOVIMENTO (HELPER) ---
    // Esta corrotina move o objeto de um ponto A para um ponto B em um tempo específico.
    private IEnumerator MoverPara(Vector3 inicio, Vector3 fim, float duracao)
    {
        float tempoPassado = 0f;
        while (tempoPassado < duracao)
        {
            // Calcula o progresso (de 0.0 a 1.0)
            float t = tempoPassado / duracao;
            // Interpola suavemente a posição
            transform.position = Vector3.Lerp(inicio, fim, t);

            tempoPassado += Time.deltaTime;
            yield return null; // Espera o próximo frame
        }
        // Garante que o objeto chegue exatamente na posição final.
        transform.position = fim;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Desativa o colisor para não matar o pombo múltiplas vezes
            GetComponent<Collider2D>().enabled = false;
            GameManager.Instance.StartGameOver();
        }
    }
}