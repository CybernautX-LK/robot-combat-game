using System;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Action AmmoChanged;
    public Action<bool> SettingActive;
	public GunData publicData { get { return data; } }	
	public bool isActive { get; private set; }	
    [SerializeField] GameObject shootEffect;
    [SerializeField] internal Transform shootEffectOrigin;
	[SerializeField] internal GunData data;
    [SerializeField] internal bool successfulShot;

    //Unity Events
    private void Awake()
    {
        if (data.backupAmmo <= 0)
            data.backupAmmo = data.maxBackupAmmo;
        if (data.ammo <= 0)
            data.ammo = data.maxAmmo;
    }
    private void Update()
    {
        if (data.reloadTimer > 0) AdvanceCooldown();
        if (data.fireTimer > 0) AdvanceFireTimer();
    }

    //Methods
    public void SetActive(bool enableGun)
    {
        isActive = enableGun;
        SettingActive?.Invoke(enableGun);
    }
    public virtual void Shoot()
    {
        successfulShot = false;
        if (data.ammo < 1 && data.maxAmmo > 0) //if out of ammo, reload (unless infinite ammo)
        {
            if (data.backupAmmo < 1) return; //if there is NO ammo, don't shoot nor reload
            Reload();
            if (data.reloadDuration > 0) 
                return; //only stop if the reload takes time
        }
        if (data.reloadTimer > 0)
            return; //if gun is in cooldown, don't shoot
        if (data.fireTimer > 0)
            return; //if gun fired, wait until next shot is available
        Instantiate(shootEffect, shootEffectOrigin);

        if (data.maxAmmo > 0) data.ammo--;
        AmmoChanged.Invoke();
        data.fireTimer = 1 / data.fireRate;
        successfulShot = true;
        //yaba yaba
        //use Gun.Shoot() in child classes
    }
    public void Reload()
    {
        if (data.backupAmmo < 1) return; //if there is NO ammo, don't reload
        
        //Take ammo from backup and add it to ammo in use
        data.backupAmmo -= data.maxAmmo;
        data.ammo = data.maxAmmo;

        //Increase reload timer, if not already in use
        if (data.reloadTimer <= 0)
            data.reloadTimer = data.reloadDuration;
        AmmoChanged?.Invoke();
    }
	void AdvanceCooldown()
    {
        data.reloadTimer -= Time.deltaTime;
    }
    void AdvanceFireTimer()
    {
        data.fireTimer -= Time.deltaTime;
    }
}