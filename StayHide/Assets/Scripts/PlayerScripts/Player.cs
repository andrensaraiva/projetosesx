using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Configura��es de Movimento")]
    public float speed = 8f; // Aumente a velocidade para ter mais agilidade
    public VariableJoystick joystick;

    [Header("Refer�ncias")]
    public JumpManager jumpManager; // Refer�ncia direta para o JumpManager

    // --- Componentes ---
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Em um jogo top-down, normalmente n�o queremos gravidade.
        rb.gravityScale = 0;
    }

    void Update()
    {
        // 1. LER INPUTS NO UPDATE (MAIS RESPONSIVO)
        // Pega a dire��o do joystick
        moveInput = joystick.Direction;
        moveVelocity = moveInput.normalized * speed;

        // VERIFICA��O DE MORTE
        // O jogador s� colide com o laser se estiver no ch�o
        if (colidiuComLaser && jumpManager.getOnGround())
        {
            itsOverForBetinha();
        }
    }

    void FixedUpdate()
    {
        // 2. APLICAR F�SICA NO FIXEDUPDATE (MAIS EST�VEL)
        // Usa a velocidade para mover o Rigidbody. � muito mais suave!
        rb.linearVelocity = moveVelocity;
    }

    // Fun��o para ser chamada pelo seu bot�o de pulo na UI
    public void TryJump()
    {
        // Chama a fun��o de pular no JumpManager
        jumpManager.pular();
    }

    private void itsOverForBetinha()
    {
        Debug.Log("Fim de Jogo!");
        // Desativa o movimento para n�o continuar ap�s a morte
        this.enabled = false;
        rb.linearVelocity = Vector2.zero;
         DerrotaScript.instance.FimDeJogo(Dados.instance.getScore());
    }

    // --- L�GICA DE COLIS�O COM O LASER ---
    private bool colidiuComLaser = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            colidiuComLaser = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            colidiuComLaser = false;
        }
    }
}