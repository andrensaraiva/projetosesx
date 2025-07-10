using UnityEngine;
using System.Collections.Generic;

public class WeaponManager_Profiled : MonoBehaviour
{
    [Header("Configura��o")]
    public WeaponProfile startingWeaponProfile;
    public WeaponStats weaponScriptInstance; // A refer�ncia para o script da arma na cena

    private PlayerController playerController;
    // Usamos um HashSet para rastrear os perfis que o jogador J� PODE USAR.
    private HashSet<WeaponProfile> unlockedProfiles = new HashSet<WeaponProfile>();

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (weaponScriptInstance == null)
        {
            Debug.LogError("ERRO CR�TICO: 'Weapon Script Instance' n�o foi definido no Player!");
            return;
        }

        // Conecta a inst�ncia da arma ao PlayerController desde o in�cio
        if (playerController != null)
        {
            playerController.currentWeapon = weaponScriptInstance;
        }

        unlockedProfiles.Clear();

        if (startingWeaponProfile != null)
        {
            // Pega a arma inicial
            PickupWeapon(startingWeaponProfile);
        }
        else
        {
            // Come�a desarmado
            weaponScriptInstance.gameObject.SetActive(false);
        }
    }

    public void PickupWeapon(WeaponProfile newProfile)
    {
        if (newProfile == null) return;

        // Torna a arma vis�vel, caso estivesse desativada
        weaponScriptInstance.gameObject.SetActive(true);

        // Pega o perfil da arma que est� equipada NO MOMENTO
        WeaponProfile currentlyEquippedProfile = weaponScriptInstance.GetCurrentProfile();

        // CASO 1: A arma pega � a MESMA que est� na m�o. Apenas recarrega.
        if (currentlyEquippedProfile == newProfile)
        {
            Debug.Log($"Pegou a mesma arma ({newProfile.weaponName}). Recarregando.");
            int ammoToAdd = Mathf.FloorToInt(newProfile.maxAmmo * 0.5f);
            weaponScriptInstance.AddAmmo(ammoToAdd);
        }
        // CASO 2: A arma pega � DIFERENTE da que est� na m�o. Troca para ela.
        else
        {
            Debug.Log($"Trocando para a nova arma: {newProfile.weaponName}");
            // Carrega o novo perfil, o que automaticamente muda o sprite e os stats
            weaponScriptInstance.LoadProfile(newProfile);
        }

        // Adiciona o perfil � lista de desbloqueados (n�o faz mal adicionar de novo)
        if (!unlockedProfiles.Contains(newProfile))
        {
            unlockedProfiles.Add(newProfile);
        }
    }
}