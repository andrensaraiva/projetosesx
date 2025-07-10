// EnemyHealth.cs (VERSÃO DE DEPURAÇÃO MÁXIMA)
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : MonoBehaviour
{
    [Header("Configuração de Vida e Pontos")]
    public int maxHealth = 100;
    public int pointsValue = 10;

    [Header("Efeitos")]
    public GameObject deathEffectPrefab;
    public AudioClip deathSound;

    private int currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (!gameObject.CompareTag("Enemy"))
        {
            gameObject.tag = "Enemy";
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // --- INÍCIO DA SEQUÊNCIA DE MORTE COM LOGS ---

        Debug.Log($"<color=#FF5733>PASSO 1/7: {gameObject.name} entrou na função Die().</color>");

        // Desativa a interação e o movimento
        GetComponent<Collider2D>().enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Desativa outros scripts para parar a IA
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) script.enabled = false;
        }

        Debug.Log("<color=#FF8C33>PASSO 2/7: Componentes de movimento e IA desativados.</color>");

        // Dá os pontos
        ScoreManager2.instance?.AddScore(pointsValue, transform.position);
        Debug.Log("<color=#FFC333>PASSO 3/7: Pontos adicionados.</color>");

        // Instancia o efeito de morte
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Debug.Log("<color=#F0FF33>PASSO 4/7: Prefab de efeito de morte instanciado.</color>");
        }
        else
        {
            Debug.LogWarning("PASSO 4/7: Prefab de efeito de morte é NULO.");
        }

        // Toca o som de morte
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Debug.Log("<color=#B5FF33>PASSO 5/7: Som de morte tocado.</color>");
        }

        // Esconde o inimigo
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("<color=#33FF57>PASSO 6/7: Sprite do inimigo escondido.</color>");
        }

        // Agenda a destruição final
        Destroy(gameObject, 2f);
        Debug.Log("<color=#33FFB5>PASSO 7/7: Destruição do GameObject agendada em 2 segundos. O objeto DEVE permanecer ativo até lá.</color>");
    }
}