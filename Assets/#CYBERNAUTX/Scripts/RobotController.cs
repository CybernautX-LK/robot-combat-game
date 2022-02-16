using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace CybernautX
{
    public class RobotController : MonoBehaviour, IHittable
    {
        [SerializeField]
        private Player player;

        private GunModel[] guns;
        private List<Weapon> selectedWeapons = new List<Weapon>();
        private Weapon currentWeapon;

        [ShowInInspector]
        [ReadOnly]
        private int currentIndex { get => (selectedWeapons.Count > 0 && selectedWeapons[0] != null && currentWeapon != null) ? selectedWeapons.IndexOf(currentWeapon) : 0; }

        private void Awake()
        {
            guns = GetComponentsInChildren<GunModel>();
        }

        private void Start() => Initialize();

        public void Initialize()
        {
            selectedWeapons = GetPlayerWeapons();

            foreach (GunModel gun in guns)
            {
                bool isSelected = selectedWeapons.Contains(gun.weapon);
                gun.gameObject.SetActive(isSelected);
            }

            SetWeapon(selectedWeapons[0]);
        }

        [Button]
        public void NextWeapon()
        {
            int index = (currentIndex < selectedWeapons.Count - 1) ? currentIndex + 1 : 0;
            SetWeapon(selectedWeapons[index]);
        }

        [Button]
        public void PreviousWeapon()
        {
            int index = (currentIndex > 0) ? currentIndex - 1 : selectedWeapons.Count - 1;
            SetWeapon(selectedWeapons[index]);
        }

        public void SetWeapon(Weapon weapon)
        {
            GunModel nextGun = GetGunByWeapon(weapon);

            if (nextGun == null) return;

            if (currentWeapon != null)
            {
                GunModel currentGun = GetGunByWeapon(currentWeapon);
                currentGun.gameObject.SetActive(false);
            }
            

            nextGun.gameObject.SetActive(true);

            currentWeapon = weapon;
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

        private GunModel GetGunByWeapon(Weapon weapon) => guns.FirstOrDefault((x) => x.weapon == weapon);

        public void GetHitted(int damage)
        {
            if (player != null)
                player.Damage(damage);
        }
    }

}
