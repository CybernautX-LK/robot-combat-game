using UnityEngine;
using CybernautX;

public class GunModel : MonoBehaviour
{
    public Weapon weapon;
    public GunController gunController;
    MeshRenderer meshRenderer;

    //Unity Events
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        gunController.SettingActive += OnSetActive;
    }
    private void OnDestroy()
    {
        gunController.SettingActive -= OnSetActive;
    }

    //Methods
    void OnSetActive(bool isActive)
    {
        if (meshRenderer)
        {
            meshRenderer.enabled = isActive; //set active main 3d model
        }

        //set active all children (all parts of grenade launcher, for example)
        int childrenNumber = transform.childCount;
        for (int i = 0; i < childrenNumber; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }
}