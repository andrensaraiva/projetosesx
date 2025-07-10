using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Adicionado para o caso de precisar interagir com componentes de UI

public class PlayerController : MonoBehaviour
{
    [Header("Configura��es de Movimento")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private Transform centralPosition;
    [SerializeField] private Transform dogDeliveryPosition;

    [Header("Sprites Direcionais")]
    [Tooltip("O componente que mostra o sprite do motoqueiro.")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;
    [SerializeField] private Sprite spriteIdle;

    private bool isMoving = false;
    private Sprite lastDirectionSprite;

    void Start()
    {
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = spriteIdle;
        }
    }

    // O M�TODO UPDATE FOI REMOVIDO COMPLETAMENTE.
    // A L�GICA AGORA EST� EM FUN��ES P�BLICAS ABAIXO.

    // --- FUN��ES P�BLICAS PARA OS BOT�ES DA UI ---

    public void OnPressMoveUp()
    {
        if (isMoving) return; // Se j� est� se movendo, n�o faz nada
        SetSpriteAndSelectStation(spriteUp, GameManager.FoodStation.Up);
    }

    public void OnPressMoveDown()
    {
        if (isMoving) return;
        SetSpriteAndSelectStation(spriteDown, GameManager.FoodStation.Down);
    }

    public void OnPressMoveLeft()
    {
        if (isMoving) return;
        SetSpriteAndSelectStation(spriteLeft, GameManager.FoodStation.Left);
    }

    public void OnPressMoveRight()
    {
        if (isMoving) return;
        SetSpriteAndSelectStation(spriteRight, GameManager.FoodStation.Right);
    }

    public void OnPressDeliver()
    {
        if (isMoving) return;

        // Ao entregar, ele deve olhar para cima (em dire��o ao cachorro)
        playerSpriteRenderer.sprite = spriteUp;
        lastDirectionSprite = spriteUp; // <<< IMPORTANTE: Define o lastDirectionSprite para a volta
        GameManager.Instance.SubmitSequence();
        StartCoroutine(MoveToTargetAndBack(dogDeliveryPosition));
    }

    // --- FUN��ES PRIVADAS DE L�GICA (permanecem iguais) ---

    private void SetSpriteAndSelectStation(Sprite directionSprite, GameManager.FoodStation station)
    {
        lastDirectionSprite = directionSprite; // Guarda o sprite de ida
        playerSpriteRenderer.sprite = directionSprite;

        GameManager.Instance.PlayerSelectedStation(station);
        Transform targetTransform = GameManager.Instance.GetTransformForStation(station);
        if (targetTransform != null)
        {
            StartCoroutine(MoveToTargetAndBack(targetTransform));
        }
    }

    private IEnumerator MoveToTargetAndBack(Transform target)
    {
        isMoving = true;
        Vector3 startPosition = centralPosition.position;

        // --- IDA PARA O ALVO ---
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target.position;

        yield return new WaitForSeconds(0.1f);

        // --- VOLTA PARA O CENTRO ---
        if (lastDirectionSprite == spriteUp) playerSpriteRenderer.sprite = spriteDown;
        else if (lastDirectionSprite == spriteDown) playerSpriteRenderer.sprite = spriteUp;
        else if (lastDirectionSprite == spriteLeft) playerSpriteRenderer.sprite = spriteRight;
        else if (lastDirectionSprite == spriteRight) playerSpriteRenderer.sprite = spriteLeft;

        while (Vector3.Distance(transform.position, startPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = startPosition;

        playerSpriteRenderer.sprite = spriteIdle;

        isMoving = false;
    }
}