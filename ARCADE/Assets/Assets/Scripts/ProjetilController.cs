using UnityEngine;

public class ProjetilController : MonoBehaviour
{
    // --- Variáveis Públicas ---
    public float velocidade = 20f;   // Velocidade com que o projétil sobe
    public float tempoDeVida = 3f;   // Tempo em segundos antes de se autodestruir

    void Start()
    {
        // Garante que o projétil será destruído depois de um tempo,
        // para não poluir a cena com objetos infinitos.
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // Move o projétil para cima constantemente
        transform.Translate(Vector2.up * velocidade * Time.deltaTime);
    }

    // A mágica acontece aqui!
    // Esta função é chamada automaticamente pela Unity quando este objeto colide com outro.
    // O script da bola (BolaController) já tem a lógica para receber o dano.
    // Nós só precisamos garantir que o projétil seja destruído no impacto.
    // Substituímos OnCollisionEnter2D por OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se o projétil entrar em qualquer trigger/collider (como o da bola), ele se destrói.
        // A gente pode até adicionar uma verificação pra não se destruir em outros triggers se precisar no futuro.
        if (other.CompareTag("Bola")) // Adicionando tag na bola para ser mais específico
        {
            Destroy(gameObject);
        }
    }
}
