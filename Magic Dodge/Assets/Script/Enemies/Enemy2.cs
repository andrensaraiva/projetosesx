// Enemy2.cs (CORRIGIDO)
using UnityEngine;
public class Enemy2 : MonoBehaviour, IControllableAI
{
    public float speed = 3f;
    public float followDuration = 3f;
    private Rigidbody2D rb;
    private Transform player;
    private enum AiState { FollowingPlayer, MovingStraight }
    private AiState currentState;
    private float followTimer;
    private Vector2 straightMoveDirection;
    private float originalSpeed;
    private bool isFrozen = false;
    void Awake() { rb = GetComponent<Rigidbody2D>(); originalSpeed = speed; }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) { enabled = false; return; }
        currentState = AiState.FollowingPlayer;
        followTimer = followDuration;
    }
    void FixedUpdate()
    {
        if (isFrozen || player == null) { rb.linearVelocity = Vector2.zero; return; }
        switch (currentState)
        {
            case AiState.FollowingPlayer: HandleFollowingPlayerState(); break;
            case AiState.MovingStraight: rb.linearVelocity = straightMoveDirection * speed; break;
        }
    }
    private void HandleFollowingPlayerState()
    {
        followTimer -= Time.fixedDeltaTime;
        rb.linearVelocity = (player.position - transform.position).normalized * speed;
        if (followTimer <= 0)
        {
            straightMoveDirection = Random.insideUnitCircle.normalized;
            if (straightMoveDirection == Vector2.zero) straightMoveDirection = Vector2.down;
            currentState = AiState.MovingStraight;
        }
    }
    public void SetFrozen(bool frozen) { isFrozen = frozen; }
    public void SetSpeed(float newSpeed) { speed = newSpeed; }
    public void ResetSpeed() { speed = originalSpeed; }
    public float GetOriginalSpeed() { return originalSpeed; }
}