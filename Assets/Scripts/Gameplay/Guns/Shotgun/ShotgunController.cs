using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : GunController
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] float impactForce;
    [SerializeField] float bottomSpreadMod = 0.5f;
    [SerializeField] float hitsNeededForDelay = 0.5f;
    [SerializeField] int extraCooldown;
    [SerializeField] int spread;
    [SerializeField] int pelletsPerBullet;
    [SerializeField] int pelletsThatHitted;
    [SerializeField] int shots;
    List<Ray> pelletRays;

    //Unity Events
    private void Start()
    {
        CalculateRays();        
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        Color rayColor = Color.black;
        foreach (Ray ray in pelletRays)
        {
            rayColor.r += 1 / pelletsPerBullet;
            rayColor.g += 0.5f / pelletsPerBullet;
            rayColor.b += 0.75f / pelletsPerBullet;
            Debug.DrawRay(ray.origin, ray.direction * data.range, rayColor);
        }
    }
#endif

    //Methods
    public override void Shoot()
    {
        base.Shoot();
        if (!successfulShot) return; //return if the gun couldn't shoot

        shots++;

        //Update Rays
        CalculateRays();

        RaycastHit hit;
        GameObject hitFX;
        foreach (var ray in pelletRays)
        {
            if (!Physics.Raycast(ray, out hit, data.range)) continue;
            hitFX = Instantiate(hitEffect);
            hitFX.transform.position = hit.point;
            //Debug.Log(hit.transform.name + " - " + hit.point);
            //Debug.Log(hitFX.transform.name + " - " + hitFX.transform.position);
            if (hit.transform.GetComponent<Rigidbody>() != null)
                hit.transform.GetComponent<Rigidbody>().AddForce(impactForce / pelletsPerBullet * Vector3.forward);
            if (hit.transform.GetComponent<IHittable>() == null) continue;
            pelletsThatHitted++;
            hit.transform.GetComponent<IHittable>().GetHitted((int)(data.damage * (1 - hit.distance / data.range)));
        }

        if (shots < 2) return;
        if (pelletsThatHitted < (pelletsPerBullet*2) * hitsNeededForDelay) 
            data.reloadTimer = extraCooldown; //add delay if less than X% pellets hitted after 2 shots,
        pelletsThatHitted = 0;
        shots = 0;
    }
    void CalculateRays()
    {
        pelletRays = new List<Ray>();
        Ray newRay = new Ray();
        Vector3 newRayDirection;

        newRay.origin = transform.position;
        newRay.direction = transform.forward;
        pelletRays.Add(newRay);

        for (int i = 1; i < pelletsPerBullet; i++)
        {
            if (i < pelletsPerBullet/4) //Randomize top-left rays
            {
                newRayDirection = new Vector3(Random.Range(-spread, 0), Random.Range(0, spread), data.range);
            }
            else if (i < (pelletsPerBullet / 4) * 2) //Randomize top-right rays
            {
                newRayDirection = new Vector3(Random.Range(0, spread), Random.Range(0, spread), data.range);
            }
            else if (i < (pelletsPerBullet / 4) * 3) //Randomize bottom-left rays
            {
                newRayDirection = new Vector3(Random.Range(-spread, 0), Random.Range(-spread* bottomSpreadMod, 0), data.range);
            }
            else  //Randomize bottom-right rays
            {
                newRayDirection = new Vector3(Random.Range(0, spread), Random.Range(-spread * bottomSpreadMod, 0), data.range);
            }

            newRay.origin = transform.position;
            newRay.direction = transform.TransformDirection(newRayDirection.normalized);
            pelletRays.Add(newRay);
        }
    }
}