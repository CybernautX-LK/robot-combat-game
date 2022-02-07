using UnityEngine;
using TMPro;

public class UIGun : MonoBehaviour
{
	[SerializeField] GunController[] guns;
	[SerializeField] GunController currentGun;
    [SerializeField] TextMeshProUGUI ammo;

    //Unity Events
    private void Start()
    {
        //Link all guns with ui to know which gun is active currently
        foreach (GunController gun in guns)
        {
            gun.SettingActive += OnSetActive;
        }
        GetActiveGun();

        currentGun.AmmoChanged += OnAmmoChange;
    }
    private void OnDestroy()
    {
        foreach (GunController gun in guns)
        {
            gun.SettingActive -= OnSetActive;
        }

        currentGun.AmmoChanged -= OnAmmoChange;
    }

    //Methods
    void GetActiveGun()
    {
        //Get active gun and show it's ammo
        foreach (GunController gun in guns)
        {
            if (gun.isActive)
            {
                currentGun = gun;
                OnAmmoChange();
                return;
            }
        }
    }

    //Event Receivers
    void OnSetActive(bool isActive)
    {
        if (isActive)
        {
            currentGun.AmmoChanged -= OnAmmoChange;
            GetActiveGun();
            currentGun.AmmoChanged += OnAmmoChange;
            return;
        }
    }
    void OnAmmoChange()
    {
        if (currentGun.data.maxAmmo <= 0)
            ammo.text = "∞";
        else
            ammo.text = currentGun.publicData.ammo + "/" + currentGun.publicData.backupAmmo;
    }
}