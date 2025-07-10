using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    private WeaponProfile currentProfile;
    private int currentAmmo;
    private float nextFireTime = 0f;

    private SpriteRenderer weaponSpriteRenderer;
    private Transform firePoint;
    private HUDManager hudManager;
    private WeaponAnimator weaponAnimator;

    void Awake()
    {
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        firePoint = transform.Find("FirePoint");
        hudManager = FindObjectOfType<HUDManager>();
        weaponAnimator = GetComponentInChildren<WeaponAnimator>();

    }

    // Fun��o para carregar um novo perfil de arma (chamada pelo Manager)
    public void LoadProfile(WeaponProfile profile)
    {
        currentProfile = profile;
        currentAmmo = profile.startingAmmo;

        weaponSpriteRenderer.sprite = profile.handSprite;

        // --- MUDAN�A AQUI ---
        if (weaponAnimator != null)
        {
            // Agora passamos o valor de FPS da anima��o diretamente do perfil da arma
            weaponAnimator.SetAnimationData(profile.handSprite, profile.shootAnimationFrames, profile.shootAnimationFPS);
        }
        // --- FIM DA MUDAN�A ---

        transform.localPosition = profile.handPosition;
        transform.localScale = profile.handScale;

        UpdateAmmoUI();
    }

    // Fun��o para adicionar muni��o (chamada pelo Manager)
    public void AddAmmo(int amount)
    {
        if (currentProfile == null) return;
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, currentProfile.maxAmmo);
        UpdateAmmoUI();
    }

    public void TryToShoot()
    {
        if (currentProfile == null || Time.time < nextFireTime || currentAmmo <= 0) return;
        nextFireTime = Time.time + 1f / currentProfile.fireRate;

        if (weaponAnimator != null)
        {
            weaponAnimator.PlayShootAnimation();
        }

        Shoot();
    }

    private void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        Quaternion baseRotation = Camera.main.transform.rotation;
        for (int i = 0; i < currentProfile.projectilesPerShot; i++)
        {
            Quaternion spread = Quaternion.Euler(Random.Range(-currentProfile.spreadAngle, currentProfile.spreadAngle), Random.Range(-currentProfile.spreadAngle, currentProfile.spreadAngle), 0);
            InstantiateProjectile(baseRotation * spread);
        }
    }

    private void InstantiateProjectile(Quaternion rotation)
    {
        // ... (c�digo do InstantiateProjectile permanece o mesmo) ...
        GameObject projectileObj = Instantiate(currentProfile.projectilePrefab, firePoint.position, rotation);
        PlayerProjectile_Modular projScript = projectileObj.GetComponent<PlayerProjectile_Modular>();
        if (projScript != null)
        {
            projScript.damage = currentProfile.damagePerShot;
            projScript.speed = currentProfile.projectileSpeed;
        }
    }

    public void UpdateAmmoUI()
    {
        if (hudManager != null && currentProfile != null)
        {
            hudManager.UpdateAmmo(currentAmmo, currentProfile.maxAmmo);
        }
    }

    // Fun��o para saber qual perfil est� carregado
    public WeaponProfile GetCurrentProfile()
    {
        return currentProfile;
    }
}