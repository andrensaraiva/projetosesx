// Enemy5.cs (CORRIGIDO)
using UnityEngine;
using System.Collections;
public class Enemy5 : MonoBehaviour, IControllableAI
{
    public float moveSpeed = 6f;
    public float detonationDistance = 1.5f;
    public float explosionRadius = 2.5f;
    public float explosionDelay = 0.5f;
    public GameObject explosionEffectPrefab;
    private Rigidbody2D rb;
    private Transform player;
    private enum AiState { Chasing, Exploding }
    private AiState currentState;
    private float originalMoveSpeed;
    private bool isFrozen = false;
    void Awake() { rb = GetComponent<Rigidbody2D>(); originalMoveSpeed = moveSpeed; }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) { enabled = false; return; }
        currentState = AiState.Chasing;
    }
    void Update()
    {
        if (isFrozen || player == null || currentState != AiState.Chasing) return;
        if (Vector2.Distance(transform.position, player.position) <= detonationDistance)
        {
            currentState = AiState.Exploding;
            StartCoroutine(Explode());
        }
    }
    void FixedUpdate()
    {
        if (isFrozen || player == null || currentState != AiState.Chasing) { rb.linearVelocity = Vector2.zero; return; }
        rb.linearVelocity = (player.position - transform.position).normalized * moveSpeed;
    }
    private IEnumerator Explode()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(explosionDelay);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerHealth>()?.Die();
                break;
            }
        }
        if (explosionEffectPrefab != null) Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void SetFrozen(bool frozen) { isFrozen = frozen; }
    public void SetSpeed(float newSpeed) { moveSpeed = newSpeed; }
    public void ResetSpeed() { moveSpeed = originalMoveSpeed; }
    public float GetOriginalSpeed() { return originalMoveSpeed; }
}