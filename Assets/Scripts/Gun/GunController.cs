using System;
using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField] GunData gunData;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] GameObject bulletPrefab;

    private Animator animator;

    private Vector3 destination;


    private float timeSinceLastShot;

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f); 

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("Animator is missing!");
        }
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        Q3Movement.Q3PlayerController.OnWalkingStateChanged += UpdateWalkAnimation;
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
                    animator.SetTrigger("Shoot");
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

    private void UpdateWalkAnimation(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    private void OnDestroy()
    {
        Q3Movement.Q3PlayerController.OnWalkingStateChanged -= UpdateWalkAnimation;
    }




}
