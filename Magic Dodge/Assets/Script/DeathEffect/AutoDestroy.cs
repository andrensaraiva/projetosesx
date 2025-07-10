// AutoDestroy.cs (Script Genérico para Efeitos)
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Tempo de espera para destruir, caso não haja Animator ou ParticleSystem.")]
    public float fallbackDelay = 3f;

    void Start()
    {
        float lifetime = 0f;

        // 1. Tenta usar o ParticleSystem para definir o tempo de vida
        if (TryGetComponent<ParticleSystem>(out var particles))
        {
            // O tempo de vida total é a duração da emissão + o tempo de vida da partícula mais longa.
            // Adicionamos um pequeno buffer para garantir que tudo desapareceu.
            lifetime = particles.main.duration + particles.main.startLifetime.constantMax + 0.5f;
        }
        // 2. Se não houver partículas, tenta usar o Animator
        else if (TryGetComponent<Animator>(out var animator))
        {
            // Pega a duração do clipe de animação que está tocando no estado atual
            lifetime = animator.GetCurrentAnimatorStateInfo(0).length;
        }
        // 3. Se não houver nenhum dos dois, usa o tempo de espera manual
        else
        {
            lifetime = fallbackDelay;
            Debug.LogWarning($"O objeto '{name}' não tem ParticleSystem nem Animator. Usando o delay de fallback: {fallbackDelay}s.", this);
        }

        // Agenda a destruição do objeto (o efeito) após o tempo de vida calculado
        Destroy(gameObject, lifetime);
    }
}