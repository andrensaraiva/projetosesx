using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BolaController : MonoBehaviour
{
    // A vida de cada bola agora será lida diretamente do valor que você definir
    // no Inspector do seu respectivo prefab (Bola, BolaMedia, BolaPequena).
    [Header("Atributos da Bola")]
    public int vida;
    public int bonusDeDestruicao = 5;
    public GameObject bolaMenorPrefab;
    public TextMeshPro textoVida;

    [Header("Efeitos")]
    public GameObject efeitoDeMortePrefab;

    [Header("Configurações de Spawn")]
    public float forcaImpulsoInicial = 3f;

    private Rigidbody2D rb;
    private int vidaInicialDestaBola; // Usado apenas para o bônus de destruição.

    // Awake é chamado quando o objeto é criado. Ideal para pegar componentes.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start é chamado no primeiro frame que o objeto está ativo.
    void Start()
    {
        // A vida inicial desta bola é o valor que está no seu próprio Inspector.
        vidaInicialDestaBola = vida;
        AtualizarTextoVida();
    }

    // --- LÓGICA DE DANO E MORTE ---
    public void LevarDano(int dano)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdicionarPontos(1);
        }

        vida -= dano;
        if (vida <= 0)
        {
            Morrer();
        }
        else
        {
            AtualizarTextoVida();
        }
    }

    // --- MÉTODO 'MORRER' ATUALIZADO ---
    private void Morrer()
    {
        // 1. Dar pontos ao jogador.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdicionarPontos(bonusDeDestruicao);
        }

        // 2. Tentar dividir a bola.
        if (bolaMenorPrefab != null)
        {
            // Não calculamos mais a vida aqui. A nova bola instanciada a partir
            // do 'bolaMenorPrefab' já terá a vida que você definiu no Inspector dela.

            // Bola 1 (Esquerda)
            GameObject bola1_objeto = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            // Apenas inicializamos com o pulo. A vida já está correta no prefab.
            bola1_objeto.GetComponent<BolaController>().InicializarParaDivisao(new Vector2(-2f, 3f));

            // Bola 2 (Direita)
            GameObject bola2_objeto = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            bola2_objeto.GetComponent<BolaController>().InicializarParaDivisao(new Vector2(2f, 3f));
        }

        // 3. Criar o efeito de partícula.
        if (efeitoDeMortePrefab != null)
        {
            Instantiate(efeitoDeMortePrefab, transform.position, Quaternion.identity);
        }

        // 4. Por fim, destruir a bola original.
        Destroy(gameObject);
    }

    // --- MÉTODO 'InicializarParaDivisao' ATUALIZADO ---
    // Agora, este método cuida apenas do movimento inicial da bola dividida.
    public void InicializarParaDivisao(Vector2 direcaoDoPulo)
    {
        // A vida não é mais definida aqui. O método Start() da nova bola cuidará
        // de ler o valor de vida do seu próprio Inspector.
        rb.linearVelocity = direcaoDoPulo;
    }


    #region Métodos Inalterados
    // Os métodos abaixo não foram alterados e estão corretos.
    public void InicializarParaSpawner(Vector2 direcao)
    {
        gameObject.layer = LayerMask.NameToLayer("SpawningBall");
        rb.AddForce(direcao * forcaImpulsoInicial, ForceMode2D.Impulse);
        StartCoroutine(AtivarColisoesFinais());
    }

    IEnumerator AtivarColisoesFinais()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = LayerMask.NameToLayer("Bolas");
    }

    private void AtualizarTextoVida()
    {
        if (textoVida != null)
        {
            textoVida.text = vida.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projetil"))
        {
            LevarDano(1);
            Destroy(other.gameObject);
        }
    }
    #endregion
}