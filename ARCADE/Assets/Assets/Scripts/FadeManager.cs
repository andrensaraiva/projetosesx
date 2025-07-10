using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    // --- PADRÃO SINGLETON ---
    // Permite acesso fácil e global através de "FadeManager.Instance".
    public static FadeManager Instance;

    [Header("Configuração")]
    // Arraste o objeto "FadeScreen" (que tem o Animator) aqui pelo Inspector.
    public Animator animator;
    // Duração do fade, deve ser igual ao tempo das suas animações de fade.
    public float tempoDeFade = 1f;

    // Awake é chamado antes de qualquer Start, ideal para configurar o Singleton.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // A MÁGICA: Este objeto não será destruído ao carregar uma nova cena.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Evita duplicatas do FadeManager se você voltar para a cena principal.
            Destroy(gameObject);
        }
    }

    // O método Start é chamado uma vez quando o FadeManager é criado.
    private void Start()
    {
        // Se o FadeScreen começar desativado, esta rotina não fará nada visível,
        // o que é o comportamento esperado. Se ele começar ativo e transparente,
        // teremos um fade-in suave no início do jogo.
        if (animator != null && animator.gameObject.activeInHierarchy)
        {
            animator.SetTrigger("StartFadeIn");
        }
    }

    // Método público que qualquer script (como o GameManager) pode chamar para trocar de cena.
    public void FadeParaCena(string nomeDaCena)
    {
        // Inicia a rotina que faz a transição completa.
        StartCoroutine(RotinaDeFade(nomeDaCena));
    }

    // A corrotina que orquestra todo o processo de fade e carregamento de cena.
    private IEnumerator RotinaDeFade(string nomeDaCena)
    {
        // --- PARTE 1: FADE-OUT ---

        // **NOVO E IMPORTANTE:** Garante que o objeto FadeScreen esteja ativo antes de usá-lo.
        // Isso permite que você deixe o FadeScreen desativado por padrão na cena.
        if (animator != null && !animator.gameObject.activeInHierarchy)
        {
            animator.gameObject.SetActive(true);
        }

        // 1. Dispara o gatilho para a animação de fade-out (escurecer a tela).
        animator.SetTrigger("StartFadeOut");

        // 2. Espera a animação de fade-out terminar.
        // Usamos WaitForSecondsRealtime para funcionar mesmo com Time.timeScale = 0.
        yield return new WaitForSecondsRealtime(tempoDeFade);

        // --- PARTE 2: CARREGAMENTO DA CENA ---

        // 3. Carrega a nova cena. A tela ainda está preta neste ponto porque o
        //    FadeManager (e seu filho FadeScreen) não foram destruídos.
        SceneManager.LoadScene(nomeDaCena);

        // --- PARTE 3: FADE-IN ---

        // 4. Dispara o gatilho para a animação de fade-in (revelar a nova cena).
        animator.SetTrigger("StartFadeIn");

        // 5. Espera a animação de fade-in terminar.
        yield return new WaitForSecondsRealtime(tempoDeFade);

        // Opcional: Desativar a tela de fade depois de tudo para economizar performance,
        // embora para um objeto simples como este, não seja estritamente necessário.
        // animator.gameObject.SetActive(false);
    }
}