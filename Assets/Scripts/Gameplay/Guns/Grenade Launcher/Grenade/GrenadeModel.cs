using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeModel : MonoBehaviour
{
    [SerializeField] GrenadeController controller;
    [SerializeField] GameObject explosionEffect;

    private void Start()
    {
        controller.Exploded += OnExplode;
    }
    private void OnDestroy()
    {
        controller.Exploded -= OnExplode;
    }

    void OnExplode()
    {
        //Set Effect
        GameObject explosion = Instantiate(explosionEffect, transform);
        explosion.transform.parent = null;
    }
}
