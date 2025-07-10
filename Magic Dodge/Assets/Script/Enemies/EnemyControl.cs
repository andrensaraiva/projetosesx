using UnityEngine;

public class Enemy : MonoBehaviour
{
    // O inimigo terá uma referência ao manager que o criou.
    private WaveManager waveManager;

    // Suponha que seu inimigo tenha vida.
    public float health = 10f;

    /// <summary>
    /// Esta função será chamada pelo WaveManager no momento em que o inimigo é criado.
    /// </summary>
    public void SetWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }

    // Exemplo de uma função para tomar dano
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // ANTES de se destruir, notifica o WaveManager!
        // A verificação 'waveManager != null' é uma boa prática.
        if (waveManager != null)
        {
            waveManager.EnemyDefeated();
        }

        // Adicione aqui efeitos de morte, som, etc.
        Destroy(gameObject);
    }

    // Se seu projétil do jogador tiver um script...
    // Ele pode chamar esta função ao colidir com o inimigo.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // Supondo que a bala dê 10 de dano.
            TakeDamage(10);
            Destroy(collision.gameObject); // Destruir a bala após o impacto
        }
    }
}