using UnityEngine;

public class ChainsawController : GunController
{
    [Header("Constant Values")]
    [SerializeField] float startingDamageMod;
    [SerializeField] float timeToFullDamage;
    [Tooltip("Activates when gun reaches max damage")]
    [SerializeField] float fullDamageCooldown;
    [Header("Current Values")]
    [SerializeField] float fullDamageTimer;
    [SerializeField] float currentDamage;
    [SerializeField] bool shooting;

    //Unity Events
    private void LateUpdate()
    {
#if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + transform.forward * data.range, Color.red);
#endif

        if (!shooting) //if gun is not shooting, stop
        {
            //if gun WAS shooting, enter in early cooldown
            if (fullDamageTimer > 0)
            {
                fullDamageTimer = 0;
            }
            return;
        }

        //if gun is in it's max damage, enter big cooldown
        if (fullDamageTimer >= timeToFullDamage)
        {
            fullDamageTimer = 0;
            data.reloadTimer = fullDamageCooldown;
        }

        shooting = false; //set shooting to false, so next frame is only activated if Shoot() is called
    }

    //Methods
    public override void Shoot()
    {
        base.Shoot();

        if (data.reloadTimer > 0) return; //return if the gun can't shoot

        //Calculate damage while gun is pressed, even if it's not in the shooting frame

        CalculateCurrentDamage();
        fullDamageTimer += Time.deltaTime;
        shooting = true;

        if (!successfulShot) return; //return if the gun is not in shooting frame

        RaycastHit[] targets = Physics.RaycastAll(transform.position, transform.forward, data.range, data.targetLayers);
        if (targets == null) return;

        foreach (RaycastHit target in targets)
        {
            if (target.transform.GetComponent<IHittable>() == null) continue;
            target.transform.GetComponent<IHittable>().GetHitted((int)currentDamage);
            Debug.Log(target.transform.name);
        }
    }
    void CalculateCurrentDamage()
    {
        currentDamage = data.damage * startingDamageMod;
        currentDamage += data.damage * (1 - startingDamageMod) * (fullDamageTimer / timeToFullDamage);
    }
}