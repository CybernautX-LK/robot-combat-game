using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace CybernautX
{
    [CreateAssetMenu(menuName = "CybernautX/Player/AI", order = 0)]
    public class AIPlayer : Player
    {
        [BoxGroup("Settings")]
        public string targetTag = "Player";

        [BoxGroup("Settings")]
        [Range(0.1f, 2.0f)]
        public float moveSpeed = 1.0f;

        [BoxGroup("Settings")]
        [Range(0.1f, 10.0f)]
        public float turnSpeed = 1.0f;

        [BoxGroup("Settings")]
        [Range(5.0f, 20.0f)]
        public float sightRange = 5.0f;

        [BoxGroup("Settings")]
        [Range(0.1f, 2.0f)]
        public float sightRadius = 1.0f;

        [BoxGroup("Debug")]
        [SerializeField]
        private bool drawDebugRay;

        [BoxGroup("Debug")]
        [ShowInInspector]
        [ReadOnly]
        private RobotController currentTarget;

        private bool targetInSight;

        public override void Think(RobotController controller)
        {
            if (!TargetAlive(controller)) return;

            Chase(controller);
            Fokus(controller);

            ChooseWeapon(controller);

            if (TargetInRange(controller) && TargetInSight(controller))
                Attack(controller);
        }

        private void Attack(RobotController controller)
        {
            if (!controller.player.configuration.shootingEnabled) return;

            controller.Shoot();
        }

        private void Chase(RobotController controller)
        {
            if (!controller.player.configuration.movementEnabled) return;

            float optimalDistance = controller.currentGun.gunController.data.range * 0.5f;
            controller.rb.MovePosition(Vector3.Lerp(controller.transform.position, currentTarget.transform.position - TargetDirection(controller) * optimalDistance, Time.deltaTime * moveSpeed));
        }

        private void Fokus(RobotController controller)
        {
            if (!controller.player.configuration.turningEnabled) return;

            controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, Quaternion.LookRotation(TargetDirection(controller)), Time.deltaTime * turnSpeed);
        }

        private void ChooseWeapon(RobotController controller)
        {
            if (!controller.player.configuration.weaponSwitchingEnabled) return;

            float currentValue = Mathf.Abs(TargetDistance(controller) - controller.currentGun.gunController.data.range);

            foreach (Weapon weapon in controller.selectedWeapons)
            {
                GunModel gun = controller.GetGunByWeapon(weapon);

                if (gun.gunController.data.ammo <= 0) continue;

                float value = Mathf.Abs(TargetDistance(controller) - gun.gunController.data.range);

                if (value < currentValue)
                    controller.SetWeapon(weapon);
            }
        }

        private Vector3 TargetDirection(RobotController controller) => Vector3.Normalize(currentTarget.transform.position - controller.transform.position);

        private float TargetDistance(RobotController controller)
        {
            if (currentTarget == null) return Mathf.Infinity;

            float distance = Vector3.Distance(controller.transform.position, currentTarget.transform.position);
            return distance;
        }

        private bool TargetAlive(RobotController controller)
        {
            if (currentTarget == null)
            {
                RobotController[] allRobotsInScene = FindObjectsOfType<RobotController>();
                currentTarget = allRobotsInScene.FirstOrDefault((x) => x != controller);
            }

            return currentTarget != null && !currentTarget.player.isDead;
        }

        private bool TargetInRange(RobotController controller) => TargetDistance(controller) <= controller.currentGun.gunController.data.range;

        private bool TargetInSight(RobotController controller)
        {
            bool targetInSight = false;

            Vector3 origin = controller.currentGun.transform.position;
            Vector3 direction = TargetDirection(controller);

            if (currentTarget != null)
            {
                if (Physics.SphereCast(origin, sightRadius, direction, out RaycastHit hit, sightRange))
                {
                    targetInSight = hit.transform.CompareTag(targetTag);
                }
            }

            if (drawDebugRay)
            {
                Color rayColor = targetInSight ? Color.green : Color.cyan;
                Debug.DrawRay(origin, direction * sightRange, rayColor);
            }

            return targetInSight;
        }


    }
}
