using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public void ApplyUpgrade(string upgradeType) {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (upgradeType == "Ammo") player.ammo += 2;
        else if (upgradeType == "Health") player.health += 1;
        else if (upgradeType == "Damage") player.headShotDamage += 1f;
    }
}