using UnityEngine;
using System.Collections; // Adicionado para podermos usar Coroutines (para o tempo de vida)

public class WeaponPickup : MonoBehaviour
{
    // Esta parte do seu c�digo permanece a mesma
    public WeaponProfile weaponProfile;

    // --- ADI��ES ---
    [Header("Efeitos e Tempo de Vida")]
    public float rotationSpeed = 60f; // Velocidade que o item gira no ch�o. Configur�vel no Inspector.
    public float lifetime = 15f;      // Tempo em segundos para o item desaparecer. Configur�vel no Inspector.

    // A fun��o Start � executada uma vez quando o objeto � criado
    void Start()
    {
        // Esta linha garante que a imagem correta (a do item no ch�o) seja mostrada.
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null && weaponProfile != null && weaponProfile.pickupSprite != null)
        {
            // --- MUDAN�A AQUI ---
            sr.sprite = weaponProfile.pickupSprite;
            // --- FIM DA MUDAN�A ---
        }

        // O resto da sua l�gica de Start, como a coroutine de tempo de vida.
        StartCoroutine(LifetimeRoutine());
    }

    // A fun��o Update � executada a cada frame
    void Update()
    {
        // Faz o objeto girar no seu eixo Y (para cima), criando o efeito de rota��o
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    // Esta � a rotina que cuida do tempo de vida
    private IEnumerator LifetimeRoutine()
    {
        // Espera pela quantidade de segundos definida na vari�vel 'lifetime'
        yield return new WaitForSeconds(lifetime);

        // Ap�s a espera, destr�i o objeto do item do ch�o
        Destroy(gameObject);
    }
    // --- FIM DAS ADI��ES ---


    // Esta parte do seu c�digo permanece a mesma
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponManager_Profiled weaponManager = other.GetComponent<WeaponManager_Profiled>();
            if (weaponManager != null)
            {
                // A sua fun��o para pegar a arma, que j� funciona
                weaponManager.PickupWeapon(weaponProfile);
                Destroy(gameObject);
            }
        }
    }
}