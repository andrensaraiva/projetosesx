// PlayerShooting.cs (VERSÃO FINAL)
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public static PlayerShooting instance;

    [Header("Configurações Gerais")]
    public Transform firePoint;
    public GameObject defaultBulletPrefab;
    public float defaultFireRate = 2f;

    private PlayerMovement playerMovement;

    private GameObject currentBulletPrefab;
    private float currentFireRate;
    private ShootingMode currentShootingMode;
    private float nextFireTime = 0f;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        currentBulletPrefab = defaultBulletPrefab;
        currentFireRate = defaultFireRate;
        currentShootingMode = ShootingMode.Right;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + (1f / currentFireRate);
            Shoot();
        }
    }

    public void SetShootingModeAndBullet(PowerType powerType, GameObject newBulletPrefab, float newFireRate)
    {
        currentBulletPrefab = newBulletPrefab != null ? newBulletPrefab : defaultBulletPrefab;
        currentFireRate = newFireRate > 0 ? newFireRate : defaultFireRate;

        switch (powerType)
        {
            case PowerType.Storm:
                currentShootingMode = ShootingMode.Left;
                break;
            case PowerType.FireBall:
                currentShootingMode = ShootingMode.FourWay;
                break;
            case PowerType.PurpleBall:
                currentShootingMode = ShootingMode.FollowMovement;
                break;
            default:
                currentShootingMode = ShootingMode.Right;
                break;
        }
    }
    public void ApplyWaveShootingSetup(ShootingMode newMode, GameObject newBulletPrefab, float newFireRate)
    {
        currentShootingMode = newMode;
        currentBulletPrefab = newBulletPrefab != null ? newBulletPrefab : defaultBulletPrefab;
        currentFireRate = newFireRate > 0 ? newFireRate : defaultFireRate;

        Debug.Log($"<color=cyan>SETUP DE TIRO ATUALIZADO! Modo: {currentShootingMode}, Bala: {currentBulletPrefab.name}, Cadência: {currentFireRate}</color>");
    }

    private void Shoot()
    {
        if (currentBulletPrefab == null || firePoint == null || playerMovement == null) return;

        switch (currentShootingMode)
        {
            case ShootingMode.Right: FireBulletInDirection(Vector2.right); break;
            case ShootingMode.Left: FireBulletInDirection(Vector2.left); break;
            case ShootingMode.FourWay:
                FireBulletInDirection(Vector2.right);
                FireBulletInDirection(Vector2.left);
                FireBulletInDirection(Vector2.up);
                FireBulletInDirection(Vector2.down);
                break;
            case ShootingMode.FollowMovement:
                FireBulletInDirection(playerMovement.lastMoveDirection);
                break;
        }
    }

    private void FireBulletInDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject bulletInstance = Instantiate(currentBulletPrefab, firePoint.position, rotation);

        if (bulletInstance.TryGetComponent<Bullet>(out var bulletScript))
        {
            bulletScript.SetDirection(direction);
        }
    }
}