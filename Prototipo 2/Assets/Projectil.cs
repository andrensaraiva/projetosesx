// Projectile.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Header("Configura��es do Proj�til")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 3f; // Tempo em segundos antes de se destruir se n�o atingir nada

    [Header("Identifica��o do Alvo")]
    [Tooltip("A tag do objeto que este proj�til deve atingir (ex: 'Enemy' ou 'Player').")]
    [SerializeField] private string targetTag;

    [Header("Efeitos")]
    [SerializeField] private GameObject hitEffectPrefab; // Part�cula de explos�o/impacto

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Garante que o colisor � um trigger para n�o empurrar os objetos
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Start()
    {
        // Destr�i o proj�til ap�s 'lifetime' segundos para evitar que ele voe para sempre
        Destroy(gameObject, lifetime);
    }

    // O "atirador" chamar� esta fun��o para lan�ar o proj�til
    public void Launch(Vector2 direction)
    {
        // Define a velocidade na dire��o fornecida
        rb.linearVelocity = direction.normalized * speed;

        // Gira o proj�til para que sua frente (eixo X, a seta vermelha) aponte na dire��o do movimento
        transform.right = direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // O proj�til colidiu com alguma coisa. Verificamos se � o alvo correto.
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

                // Destr�i o proj�til ap�s o impacto
                Destroy(gameObject);
            }
        }
    }
}