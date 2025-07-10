using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Profile", menuName = "FPS/Weapon Profile")]
public class WeaponProfile : ScriptableObject
{
    [Header("Identifica��o e Visuais")]
    public GameObject pickupPrefab;

    public string weaponName = "New Weapon";
    public Sprite handSprite;
    public Sprite[] shootAnimationFrames;
    public float shootAnimationFPS = 15f;  // <<-- ADICIONE ESTA LINHA
    public Sprite pickupSprite;

    [Header("Atributos de Combate")] // Movi os atributos para baixo para melhor organiza��o
    public float damagePerShot = 50f;
    public float fireRate = 5f;
    public float projectileSpeed = 30f;
    public int projectilesPerShot = 1;
    public float spreadAngle = 0f;

    [Header("Muni��o")] // Movi os atributos para baixo para melhor organiza��o
    public int maxAmmo = 100;
    public int startingAmmo = 50;

    [Header("Posicionamento na M�o")]
    [Tooltip("A posi��o da arma em rela��o � c�mera.")]
    public Vector3 handPosition = new Vector3(0, -0.3f, 1f);
    [Tooltip("A escala da arma na m�o.")]
    public Vector3 handScale = Vector3.one; // Vector3.one � o mesmo que (1, 1, 1)

    [Header("Componentes")] // Movi os atributos para baixo para melhor organiza��o
    public GameObject projectilePrefab;

    [Tooltip("Se marcado, o 'Fire Rate' ser� ignorado e a cad�ncia ser� ditada pela dura��o da anima��o de tiro.")]
    public bool lockFireRateToAnimation = false;

}