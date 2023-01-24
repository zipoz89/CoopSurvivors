using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

namespace Old
{

    public class PlayerGun : NetworkBehaviour
    {
        [SerializeField] private Transform gunModel;
        [SerializeField] private Transform gunBarrel;

        [SerializeField] private GameObject projectile;
        [SerializeField] private float fireRate = 1f;
        private float elapsedTimeFromLastFire = 0;

        private bool isOwner = true;

        public override void OnStartClient()
        {
            base.OnStartClient();
            isOwner = base.IsOwner;
        }

        private void Update()
        {
            HandleFire();
        }

        private void HandleFire()
        {
            elapsedTimeFromLastFire += Time.deltaTime;
            if (elapsedTimeFromLastFire >= fireRate)
            {
                elapsedTimeFromLastFire = 0;
                Fire();
            }
        }

        private void Fire()
        {
            FireServer(gunModel.rotation);
        }

        [ServerRpc]
        public void FireServer(Quaternion startRotation)
        {
            GameObject o = GameObject.Instantiate(projectile, gunBarrel.position, startRotation);
            ServerManager.Spawn(o);
        }

        public void SetRotation(Vector3 rotation)
        {
            float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            var rotationToSet = Quaternion.AngleAxis(angle, Vector3.forward);
            gunModel.rotation = rotationToSet;
            SetRotationServer(rotationToSet);
        }

        [ServerRpc]
        public void SetRotationServer(Quaternion rotation)
        {
            SetRotationClient(rotation);
        }

        [ObserversRpc]
        public void SetRotationClient(Quaternion rotation)
        {
            if (!isOwner)
            {
                gunModel.rotation = rotation;
            }
        }
    }
}