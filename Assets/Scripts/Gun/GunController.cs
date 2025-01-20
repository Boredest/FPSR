using System;
using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField] GunData gunData;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] GameObject bulletPrefab;

    private Vector3 destination;


    private float timeSinceLastShot;

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f); 

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }



    public void Shoot()
    {
        Debug.Log(gunData.currentAmmo);
        if(gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    destination = hitInfo.point;
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunData.damage);
                    InstantiateProjectile();
                      
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
            }
        }
    }

    private void InstantiateProjectile()
    {
        var bullet = Instantiate(bulletPrefab, gunBarrel.transform.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody>().linearVelocity = (destination - gunBarrel.transform.position).normalized * gunData.bulletSpeed;
        
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    

    
}
