using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float health = 2f;
    public Transform headPosition;

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die() {
        Destroy(gameObject);
    }

    public void PerformAction(PlayerController player) {
        player.health--;
        FindObjectOfType<GameManager>().EndEnemyTurn();
    }
}