using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GunController[] guns;
    [SerializeField] UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController fpsController;
    [SerializeField] float shootingMovementMod;
    [SerializeField] int currentGun;
    float originalSpeed;

    //Unity Events
    private void Start()
    {
        originalSpeed = fpsController.movementSettings.ForwardSpeed;
        ChangeGun(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            guns[currentGun].Reload();
        }
        if (Input.GetMouseButton(0))
        {
            fpsController.movementSettings.ForwardSpeed = originalSpeed / 2;
            guns[currentGun].Shoot();
        }
        else if (fpsController.movementSettings.ForwardSpeed != originalSpeed)
        {
            fpsController.movementSettings.ForwardSpeed = originalSpeed;
        }

        if (Input.GetKeyDown("q"))
        {
            currentGun--;
            if (currentGun < 0) currentGun = 0;
            ChangeGun(currentGun);
        }
        if (Input.GetKeyDown("e"))
        {
            currentGun++;
            if (currentGun >= guns.Length) currentGun = guns.Length-1;
            ChangeGun(currentGun);
        }
    }

    //Methods
    void ChangeGun(int gunIndex)
    {
        currentGun = gunIndex;
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(i == gunIndex);
        }
    }
}