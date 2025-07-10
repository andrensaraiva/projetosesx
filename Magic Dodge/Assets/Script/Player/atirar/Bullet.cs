// Bullet.cs
using UnityEngine;

// Garante que a bala tenha um colisor para detectar acertos.
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 50;
    public float lifetime = 3f; // Tempo para a bala se autodestruir

    private Vector2 direction;

    void Start()
    {
        // Garante que as balas do jogador tenham a tag correta para o inimigo detectar.
        if (gameObject.tag != "PlayerBullet")
        {
            gameObject.tag = "PlayerBullet";
        }
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move a bala na direção definida
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // O PlayerShooting usa esta função para dizer para onde a bala deve ir.
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    // Detecta colisão com outros objetos.
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"<color=cyan>Bala colidiu com: {other.name}, Tag: {other.tag}</color>"); // LINHA DE DEBUG

        if (other.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            Debug.Log($"<color=green>Inimigo encontrado! Causando {damage} de dano.</color>"); // LINHA DE DEBUG
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
