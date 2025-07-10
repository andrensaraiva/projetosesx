// RangedTower.cs
using UnityEngine;

public class RangedTower : TowerWithBuffs
{
    [Header("Atributos Ranged")]
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // Ponto de onde o projétil sai
    protected override void Start()
    {
        base.Start();
        originalDamage = damage; // Guarda o dano original
    }

    protected override void HandleDamageBuff(float multiplier, bool isApplying)
    {
        damage = isApplying ? Mathf.RoundToInt(originalDamage * multiplier) : originalDamage;
    }
    protected override void Attack()
    {
        if (projectilePrefab == null || firePoint == null || currentTarget == null) return;

        Vector3 spawnPosition = firePoint.position;

        // Calcula a direção exata para o alvo
        Vector2 direction = currentTarget.position - spawnPosition;

        // Instancia o projétil
        GameObject projGO = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Pega o script e lança!
        Projectile projectileScript = projGO.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Launch(direction);
        }
    }
}