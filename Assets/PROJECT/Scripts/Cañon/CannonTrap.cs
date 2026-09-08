using UnityEngine;

public class CannonTrap : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float fireRate = 2.5f;
    [SerializeField] private float initialDelay = 1f;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), initialDelay, fireRate);
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        CannonProjectile projectile = projectileObj.GetComponent<CannonProjectile>();

        if (projectile != null)
        {
            projectile.Launch(firePoint.forward, projectileSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(firePoint.position, firePoint.forward * 3f);
        }
    }
}