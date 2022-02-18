using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityStandardAssets.Characters.FirstPerson;

namespace CybernautX
{
    [RequireComponent(typeof(Rigidbody))]
    public class RobotController : MonoBehaviour, IHittable
    {
        [InlineEditor]
        public Player player;

        private GunModel[] guns;
        public List<Weapon> selectedWeapons { get; private set; }
        public GunModel currentGun { get; private set; }

        private Dictionary<Weapon, GunModel> weaponGunDictionary = new Dictionary<Weapon, GunModel>();
        public Rigidbody rb { get; private set; }

        public RigidbodyFirstPersonController firstPersonController { get; private set; }

        private int currentIndex;

        private void Awake()
        {
            if (player == null)
            {
                this.enabled = false;
                return;
            }

            guns = GetComponentsInChildren<GunModel>();
            rb = GetComponent<Rigidbody>();

            if (player is HumanPlayer)
            {
                firstPersonController = GetComponent<RigidbodyFirstPersonController>();

                if (firstPersonController == null)
                    firstPersonController = gameObject.AddComponent<RigidbodyFirstPersonController>();
            }           
        }

        private void Start() => Initialize();

        private void Update()
        {
            if (!player.isDead)
                player.Think(this);

            if (firstPersonController != null)
                firstPersonController.enabled = player.configuration.movementEnabled;
        }

        public void Initialize()
        {
            selectedWeapons = GetPlayerWeapons();

            foreach (GunModel gun in guns)
            {
                weaponGunDictionary.Add(gun.weapon, gun);
                gun.gunController.SetActive(false);
            }

            SetWeapon(selectedWeapons[0]);
        }

        public void Shoot()
        {
            if (currentGun != null && currentGun.gunController != null)
                currentGun.gunController.Shoot();
        }

        public void Reload()
        {
            if (currentGun != null && currentGun.gunController != null)
                currentGun.gunController.Reload();
        }

        [Button(ButtonSizes.Large)]
        public void NextWeapon()
        {
            currentIndex = (currentIndex < selectedWeapons.Count - 1) ? currentIndex + 1 : 0;
            SetWeapon(selectedWeapons[currentIndex]);
        }

        [Button(ButtonSizes.Large)]
        public void PreviousWeapon()
        {
            currentIndex = (currentIndex > 0) ? currentIndex - 1 : selectedWeapons.Count - 1;
            SetWeapon(selectedWeapons[currentIndex]);
        }

        public void SetWeapon(Weapon weapon)
        {
            GunModel nextGun = GetGunByWeapon(weapon);

            if (nextGun == null) return;

            if (currentGun != null)
            {
                currentGun.gunController.SetActive(false);
            }

            nextGun.gunController.SetActive(true);

            currentGun = nextGun;
        }

        private List<Weapon> GetPlayerWeapons()
        {
            if (!player) return null;

            List<Weapon> weapons = new List<Weapon>();

            foreach (ItemSlot slot in player.weaponSlots)
            {
                if (slot.selectedItem != null && slot.selectedItem is Weapon)
                {
                    weapons.Add((Weapon)slot.selectedItem);
                }
            }

            return weapons;
        }

        public GunModel GetGunByWeapon(Weapon weapon) => weaponGunDictionary[weapon];

        public void GetHitted(int damage)
        {
            if (player != null)
                player.TakeDamage(damage);
        }
    }

}
