using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public Action TargetHitted;
    public Action Exploded;
    [Header("Constant Values")]
    public Transform launcher;
    public LayerMask targetLayers;
    public int damage;
    public float explosionDelay;
    [SerializeField] float explosionRadius;
    //[Header("Current Values")]
    //[SerializeField] float explosionTimer;

    //Unity Events
    private void Start()
    {
        Invoke("Explode", explosionDelay);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, transform.localScale.x, targetLayers);
        if (hits == null) return;
        transform.parent = collision.transform;
        GetComponent<Rigidbody>().isKinematic = true;
        TargetHitted?.Invoke();
    }

    //Methods
    void Explode()
    {
        //Call Action
        Exploded?.Invoke();

        //Damage targets
        transform.parent?.GetComponent<IHittable>()?.GetHitted(damage);
        if (Vector3.Distance(launcher.position, transform.position) < explosionRadius)
        {
            launcher.GetComponent<IHittable>()?.GetHitted(damage);
        }

        //Destroy after explosion
        Destroy(gameObject);
    }
}