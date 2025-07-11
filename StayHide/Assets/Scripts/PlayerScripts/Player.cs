using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speed = 8f; // Aumente a velocidade para ter mais agilidade
    public VariableJoystick joystick;

    [Header("Referências")]
    public JumpManager jumpManager; // Referência direta para o JumpManager

    // --- Componentes ---
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Em um jogo top-down, normalmente não queremos gravidade.
        rb.gravityScale = 0;
    }

    void Update()
    {
        // 1. LER INPUTS NO UPDATE (MAIS RESPONSIVO)
        // Pega a direção do joystick
        moveInput = joystick.Direction;
        moveVelocity = moveInput.normalized * speed;

        // VERIFICAÇÃO DE MORTE
        // O jogador só colide com o laser se estiver no chão
        if (colidiuComLaser && jumpManager.getOnGround())
        {
            itsOverForBetinha();
        }
    }

    void FixedUpdate()
    {
        // 2. APLICAR FÍSICA NO FIXEDUPDATE (MAIS ESTÁVEL)
        // Usa a velocidade para mover o Rigidbody. É muito mais suave!
        rb.linearVelocity = moveVelocity;
    }

    // Função para ser chamada pelo seu botão de pulo na UI
    public void TryJump()
    {
        // Chama a função de pular no JumpManager
        jumpManager.pular();
    }

    private void itsOverForBetinha()
    {
        Debug.Log("Fim de Jogo!");
        // Desativa o movimento para não continuar após a morte
        this.enabled = false;
        rb.linearVelocity = Vector2.zero;
         DerrotaScript.instance.FimDeJogo(Dados.instance.getScore());
    }

    // --- LÓGICA DE COLISÃO COM O LASER ---
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