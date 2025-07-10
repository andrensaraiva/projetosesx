// Enemy4.cs (CORRIGIDO)
using UnityEngine;
public class Enemy4 : MonoBehaviour, IControllableAI
{
    public float moveSpeed = 5f;
    public float followDuration = 0.5f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private Rigidbody2D rb;
    private Transform player;
    private enum AiState { FollowingPlayer, Shooting }
    private AiState currentState;
    private float followTimer;
    private float fireCountdown = 0f;
    private float originalMoveSpeed;
    private bool isFrozen = false;
    void Awake() { rb = GetComponent<Rigidbody2D>(); originalMoveSpeed = moveSpeed; }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) { enabled = false; return; }
        currentState = AiState.FollowingPlayer;
        followTimer = followDuration;
        if (firePoint == null) firePoint = transform;
    }
    void Update()
    {
        if (isFrozen || player == null) return;
        if (currentState == AiState.FollowingPlayer) HandleFollowingPlayerState();
        else if (currentState == AiState.Shooting) HandleShootingState();
    }
    void FixedUpdate()
    {
        if (isFrozen || player == null || currentState != AiState.FollowingPlayer)
        {
            if (currentState != AiState.Shooting) rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = (player.position - transform.position).normalized * moveSpeed;
    }
    private void HandleFollowingPlayerState()
    {
        followTimer -= Time.deltaTime;
        if (followTimer <= 0)
        {
            currentState = AiState.Shooting;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
    private void HandleShootingState()
    {
        Vector2 lookDir = player.position - firePoint.position;
        firePoint.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg);
        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0f)
        {
            if (projectilePrefab != null) Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            fireCountdown = 1f / fireRate;
        }
    }
    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
        if (isFrozen && currentState != AiState.Shooting) rb.bodyType = RigidbodyType2D.Static;
        else if (!isFrozen && currentState != AiState.Shooting) rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void SetSpeed(float newSpeed) { moveSpeed = newSpeed; }
    public void ResetSpeed() { moveSpeed = originalMoveSpeed; }
    public float GetOriginalSpeed() { return originalMoveSpeed; }
}