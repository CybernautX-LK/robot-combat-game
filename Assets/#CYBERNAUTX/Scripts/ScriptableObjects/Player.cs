using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace CybernautX
{
    [CreateAssetMenu(menuName = "CybernautX/General/Player", order = 0)]
    public class Player : ScriptableObject
    {
        [BoxGroup("General")]
        public new string name = "Insert player name here...";

        [BoxGroup("Health")]
        public float maxHealth = 100f;

        [BoxGroup("Health")]
        public float minHealth = 100f;

        [BoxGroup("Health")]
        public float currentHealth;

        [BoxGroup("Weapons")]
        [InlineEditor]
        public List<ItemSlot> weaponSlots = new List<ItemSlot>();

        //[BoxGroup("Weapons")]
        //public List<Weapon> availableWeapons = new List<Weapon>();
        //
        //[BoxGroup("Weapons")]
        //public Weapon[] currentWeapons;

        public UnityAction<float> OnHealthUpdatedEvent;

        private void OnEnable()
        {
            //currentWeapons = new Weapon[weaponSlots];
            currentHealth = maxHealth;
        }

        public void Damage(float value) => SetHealth(currentHealth - value);


        public void Heal(float value) => SetHealth(currentHealth + value);

        public void Die()
        {
            currentHealth = 0.0f;
        }

        [Button]
        public void SetHealth(float value)
        {
            currentHealth = Mathf.Clamp(value, 0.0f, maxHealth);

            if (currentHealth <= 0.0f)
                Die();

            OnHealthUpdatedEvent?.Invoke(currentHealth);
        }

        //public void SetItem(Item item) { }

        //public void SetItem(Item item, int index)
        //{
        //    if (item is Weapon)
        //    {
        //        if (weaponSlots <= index) return;
        //        Weapon weapon = (Weapon)item;             
        //        currentWeapons[index] = weapon;
        //    }
        //
        //
        //}
    }
}


