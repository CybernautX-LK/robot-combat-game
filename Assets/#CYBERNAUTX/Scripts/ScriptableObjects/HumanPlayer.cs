using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace CybernautX
{
    [CreateAssetMenu(menuName = "CybernautX/Player/Human", order = 0)]
    public class HumanPlayer : Player
    {
        [BoxGroup("Input")]
        [SerializeField]
        private KeyCode shootKey = KeyCode.Mouse0;

        [BoxGroup("Input")]
        [SerializeField]
        private KeyCode reloadKey = KeyCode.R;

        [BoxGroup("Input")]
        [SerializeField]
        private KeyCode nextWeaponKey = KeyCode.E;

        [BoxGroup("Input")]
        [SerializeField]
        private KeyCode previousWeaponKey = KeyCode.Q;

        public override void Think(RobotController controller)
        {
            if (controller.player.configuration.shootingEnabled)
            {
                if (Input.GetKey(shootKey))
                    controller.Shoot();

                if (Input.GetKey(reloadKey))
                    controller.Reload();
            }

            if (controller.player.configuration.weaponSwitchingEnabled)
            {
                if (Input.GetKeyDown(nextWeaponKey))
                    controller.NextWeapon();

                if (Input.GetKeyDown(previousWeaponKey))
                    controller.PreviousWeapon();
            }
        }
    }
}

