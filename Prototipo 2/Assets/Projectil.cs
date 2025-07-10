// Projectile.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 3f; // Tempo em segundos antes de se destruir se não atingir nada

    [Header("Identificação do Alvo")]
    [Tooltip("A tag do objeto que este projétil deve atingir (ex: 'Enemy' ou 'Player').")]
    [SerializeField] private string targetTag;

    [Header("Efeitos")]
    [SerializeField] private GameObject hitEffectPrefab; // Partícula de explosão/impacto

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Garante que o colisor é um trigger para não empurrar os objetos
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Start()
    {
        // Destrói o projétil após 'lifetime' segundos para evitar que ele voe para sempre
        Destroy(gameObject, lifetime);
    }

    // O "atirador" chamará esta função para lançar o projétil
    public void Launch(Vector2 direction)
    {
        // Define a velocidade na direção fornecida
        rb.linearVelocity = direction.normalized * speed;

        // Gira o projétil para que sua frente (eixo X, a seta vermelha) aponte na direção do movimento
        transform.right = direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // O projétil colidiu com alguma coisa. Verificamos se é o alvo correto.
        if (other.CompareTag(targetTag))
        {
            // Tenta pegar o "contrato" IDamageable do objeto atingido
            IDamageable damageableObject = other.GetComponent<IDamageable>();

            // Se o objeto realmente pode tomar dano...
            if (damageableObject != null)
            {
                // ...aplica o dano!
                damageableObject.TakeDamage(damage);

                // Cria o efeito de impacto se houver um
                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                }

                // Destrói o projétil após o impacto
                Destroy(gameObject);
            }
        }
    }
}