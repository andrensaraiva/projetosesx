// RangedEnemy.cs
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Atributos Ranged")]
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float fireRate = 1f; // Tiros por segundo
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // Ponto de onde o projétil sai

    private float fireCooldownTimer;

    protected override void Update()
    {
        // Chama a lógica base (virar para o player, etc)
        base.Update();

        // Reduz o cooldown do tiro
        if (fireCooldownTimer > 0)
        {
            fireCooldownTimer -= Time.deltaTime;
        }
    }

    protected override Vector2 HandleMovement()
    {
        if (playerTransform == null || isDead) return Vector2.zero;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            // Perto o suficiente: para de mover e tenta atirar
            TryAttack();
            return Vector2.zero; // Retorna um vetor zero para indicar que deve parar
        }
        else
        {
            // Longe demais: move-se em direção ao jogador
            return (playerTransform.position - transform.position).normalized;
        }
    }


    private void TryAttack()
    {
        if (fireCooldownTimer <= 0f && projectilePrefab != null)
        {
            Fire();
            fireCooldownTimer = 1f / fireRate; // Reseta o cooldown
        }
    }

    private void Fire()
    {
        if (projectilePrefab == null || firePoint == null || playerTransform == null) return;

        Vector3 spawnPosition = firePoint.position;

        // Calcula a direção exata para o jogador
        Vector2 direction = playerTransform.position - spawnPosition;

        GameObject projectileGO = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Projectile projectileScript = projectileGO.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            // Lança o projétil na direção do jogador
            projectileScript.Launch(direction);
        }
    }

    // Desenha o range de ataque no editor para facilitar o debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}