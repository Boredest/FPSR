using UnityEngine;


[CreateAssetMenu(fileName = "Gun", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public int fireRate;
    public int reloadTime;

    [Header("Bullets")]
    public float bulletSpeed;

    [HideInInspector]
    public bool reloading;
    

}
