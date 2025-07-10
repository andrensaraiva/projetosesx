using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int ammo = 6;
    public int health = 3;
    public float headShotDamage = 2f;
    public float bodyShotDamage = 1f;

    private GameManager gm;

    void Start() {
        gm = FindObjectOfType<GameManager>();
    }

    void Update() {
        if (gm.CurrentState != TurnState.PlayerTurn) return;
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.collider.TryGetComponent<EnemyController>(out var enemy)) {
                    Shoot(enemy, hit.point);
                } else {
                    Reload();
                }
            }
        }
    }

    void Shoot(EnemyController enemy, Vector3 hitPoint) {
        if (ammo <= 0) {
            Reload();
            return;
        }

        ammo--;
        bool head = Vector3.Distance(hitPoint, enemy.headPosition.position) < 0.5f;
        float damage = head ? headShotDamage : bodyShotDamage;
        enemy.TakeDamage(damage);
        gm.EndPlayerTurn();
    }

    void Reload() {
        ammo = 6;
        gm.EndPlayerTurn();
    }
}