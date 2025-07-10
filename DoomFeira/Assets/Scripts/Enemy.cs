using UnityEngine;
using UnityEngine.AI; // Adicione se n�o tiver

public class Enemy : MonoBehaviour
{
    // --- VARI�VEIS DO INIMIGO FRACO ---
    public float health = 100f;      // <<-- ADICIONE A VIDA
    public float damage = 20f;
    public int pointsValue = 10;

    private Transform playerTarget;
    private NavMeshAgent agent;
    private GameManager gameManager; // Adicione para otimizar a busca

    public SpriteAnimator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>(); // Encontra o game manager uma vez

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
    }

    void Update()
    {
        if (playerTarget != null && agent.enabled)
        {
            agent.SetDestination(playerTarget.position);

            // Se o inimigo est� se movendo, toca a anima��o "Walk"
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.Play("Walk");
            }
            // Se estiver parado, toca a anima��o "Idle"
            else
            {
                animator.Play("Idle");
            }
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage); // O dano dele ao encostar
            }
            Destroy(gameObject);
        }
    }

    // --- FUN��O FALTANTE ---
    // Adicione esta fun��o p�blica para que o proj�til possa cham�-la
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // A fun��o Die() permanece a mesma
    public void Die()
    {
        if (gameManager != null)
        {
            gameManager.AddScore(pointsValue);
        }
        else // Fallback caso n�o encontre no Start
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.AddScore(pointsValue);
        }
        animator.Play("Death");

        // Desativa a l�gica para que ele pare no lugar
        this.enabled = false; // Desativa este pr�prio script (o Update para)
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<Billboard>().enabled = false; // Para de encarar a c�mera

        // ... sua l�gica de score ...

        // Destr�i o objeto depois de um tempo para dar tempo de ver a anima��o de morte
        Destroy(gameObject, 2f);
    }
}   