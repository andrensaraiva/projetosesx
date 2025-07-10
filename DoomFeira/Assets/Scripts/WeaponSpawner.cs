using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("Configura��o de Spawn de Armas")]
    public WeaponProfile[] availableWeaponProfiles;

    // --- MUDAN�A: Substitu�mos o raio por um Vector3 para definir o tamanho da caixa ---
    [Header("�rea de Spawn (Paralelep�pedo)")]
    public Vector3 spawnAreaSize = new Vector3(40f, 1f, 40f); // Largura(X), Altura(Y), Comprimento(Z)

    [Header("Controle de Spawn")]
    public float spawnInterval = 15f;


    // --- ADI��O: A fun��o que desenha o Gizmo no Editor ---
    // Esta fun��o especial da Unity � chamada automaticamente na janela Scene
    void OnDrawGizmosSelected()
    {
        // Define a cor que o Gizmo ter�. Usaremos um verde semitransparente.
        Gizmos.color = new Color(0, 1, 0, 0.5f); // R, G, B, Alpha

        // Desenha um cubo (paralelep�pedo) na posi��o deste objeto, com o tamanho que definimos.
        // Este Gizmo s� ser� vis�vel quando voc� selecionar o objeto Spawner na Hierarchy.
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }


    void Start()
    {
        StartCoroutine(SpawnWeaponsRoutine());
    }

    IEnumerator SpawnWeaponsRoutine()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (availableWeaponProfiles.Length == 0)
            {
                Debug.LogWarning("WeaponSpawner n�o tem perfis de arma para spawnar.");
                continue;
            }

            int randomIndex = Random.Range(0, availableWeaponProfiles.Length);
            WeaponProfile randomProfile = availableWeaponProfiles[randomIndex];

            if (randomProfile.pickupPrefab == null)
            {
                Debug.LogWarning($"O perfil '{randomProfile.name}' n�o tem um 'Pickup Prefab' definido.");
                continue;
            }

            // --- MUDAN�A: L�gica de gera��o de posi��o ---
            // Agora geramos um ponto dentro do nosso paralelep�pedo

            // Calcula a metade das dimens�es para facilitar o c�lculo do intervalo
            float halfWidth = spawnAreaSize.x / 2;
            float halfDepth = spawnAreaSize.z / 2;

            // Gera um deslocamento aleat�rio a partir do centro do spawner
            Vector3 randomOffset = new Vector3(
                Random.Range(-halfWidth, halfWidth),
                0, // Mant�m o spawn na mesma altura Y do spawner
                Random.Range(-halfDepth, halfDepth)
            );

            // A posi��o final � a posi��o do spawner mais o deslocamento aleat�rio
            Vector3 spawnPosition = transform.position + randomOffset;
            // --- FIM DA MUDAN�A ---


            GameObject pickupInstance = Instantiate(randomProfile.pickupPrefab, spawnPosition, Quaternion.identity);

            WeaponPickup pickupScript = pickupInstance.GetComponent<WeaponPickup>();
            if (pickupScript != null)
            {
                pickupScript.weaponProfile = randomProfile;
            }
        }
    }
}