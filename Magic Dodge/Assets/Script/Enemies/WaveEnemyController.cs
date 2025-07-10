// WaveEnemyController.cs (VERSÃO LIMPA E CORRETA)
using UnityEngine;

public class WaveEnemyController : MonoBehaviour
{
    private WaveManager waveManager;
    public float health = 10f;

    public void Initialize(WaveManager manager)
    {
        waveManager = manager;
    }

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
        if (waveManager != null)
        {
            waveManager.EnemyDefeated();
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }
}