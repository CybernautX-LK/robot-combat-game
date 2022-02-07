using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncherController : GunController
{
    [Header("Constant Values")]
    [SerializeField] GameObject grenadeTemplate;
    [SerializeField] float grenadeExplosionDelay;
    [Tooltip("Percentage of ammo (1 = 100% of ammo needs to hit")]
    [SerializeField] float hitsNeededToReduceCooldown = 1f;
    [SerializeField] float shorterCooldown;
    [SerializeField] float grenadeSpeedMod;
    [Header("Current Values")]
    [SerializeField] int grenadesThatHitted;

    //Unity Events
    private void LateUpdate()
    {
        //if gun reload lasted more than short cooldown
        if (data.reloadTimer > 0 && data.reloadTimer <= data.reloadDuration - shorterCooldown)
        {
            //and needed grenades hitted, finish the cooldown
            if (grenadesThatHitted >= data.maxAmmo * hitsNeededToReduceCooldown)
            {
                data.reloadTimer = 0;
            }

            //set hits to 0 one way or another
            grenadesThatHitted = 0;
        }
    }

    //Methods
    public override void Shoot()
    {
        base.Shoot();
        if (!successfulShot) return;

        //Create grenade
        GameObject go = Instantiate(grenadeTemplate, shootEffectOrigin);
        go.transform.parent = null;
        go.transform.localScale = grenadeTemplate.transform.localScale;

        //set data and link action
        GrenadeController grenade = go.GetComponent<GrenadeController>();
        SetGrenadeData(grenade);
        grenade.TargetHitted += OnGrenadeHitted;

        //Launch it
        go.GetComponent<Rigidbody>()?.AddForce(transform.forward * (data.range / grenadeExplosionDelay) * grenadeSpeedMod);
        //Debug.Break();
    }
    void SetGrenadeData(GrenadeController grenade)
    {
        grenade.launcher = transform.parent;
        grenade.targetLayers = data.targetLayers;
        grenade.damage = data.damage;
        grenade.explosionDelay = grenadeExplosionDelay;
    }

    //Event Receivers
    void OnGrenadeHitted()
    {
        grenadesThatHitted++;
    }
}