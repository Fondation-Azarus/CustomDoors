using PluginAPI.Core.Attributes;
using Mirror;
using PluginAPI.Enums;
using UnityEngine;
using Interactables.Interobjects.DoorUtils;
using System.Linq;
using MapGeneration;
using System;
using PluginAPI.Core;
using InventorySystem;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.Firearms;
using System.Collections.Generic;
using InventorySystem.Items.Firearms.Ammo;
using InventorySystem.Items;

namespace CustomDoors
{
    public class EventHandler
    {
        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void WaitingForPlayers()
        {
            List<SpawnableConfig>  spawnableObjects = new(PluginClass.Singleton.config.Doors);
            spawnableObjects.AddRange(PluginClass.Singleton.config.Items);

            //foreach (SpawnableConfig spawnableConfig in PluginClass.Singleton.config.SpawnableObjects) bugged
            foreach (SpawnableConfig spawnableConfig in spawnableObjects)
            {
                RoomIdentifier room = RoomIdentifier.AllRoomIdentifiers.FirstOrDefault(r => r.name == spawnableConfig.RoomName);

                if (room == null)
                    continue;

                Vector3 scale = new(spawnableConfig.ScaleX, spawnableConfig.ScaleY, spawnableConfig.ScaleZ);

                float roomRotationDiffY = spawnableConfig.DefaultRoomRotationYAxis - room.transform.rotation.eulerAngles.y;

                Vector3 objectRotation = new(spawnableConfig.RelativeRotationX, spawnableConfig.RelativeRotationY + roomRotationDiffY, spawnableConfig.RelativeRotationZ);

                Vector3 finalPosition = GetRotatedPosition(roomRotationDiffY, new(spawnableConfig.RelativePositionX, spawnableConfig.RelativePositionY, spawnableConfig.RelativePositionZ)) + room.transform.position;

                if (spawnableConfig is DoorConfig doorConfig)
                {
                    DoorVariant door = UnityEngine.Object.Instantiate(UnityEngine.Object.FindObjectsOfType<DoorSpawnpoint>().First(d => d.TargetPrefab.name.Contains(doorConfig.Type)).TargetPrefab, finalPosition, Quaternion.Euler(objectRotation));
                    door.transform.localScale = scale;
                    door.RequiredPermissions.RequiredPermissions = doorConfig.KeycardPermissions;
                    NetworkServer.Spawn(door.gameObject);
                    door.ServerChangeLock(DoorLockReason.SpecialDoorFeature, doorConfig.Locked);
                    door.TargetState = doorConfig.Opened;
                }

                else if (spawnableConfig is ItemConfig itemConfig)
                {
                    ItemBase itemBase = ReferenceHub.HostHub.inventory.ServerAddItem(itemConfig.Item);

                    ItemPickupBase itemPickup = itemBase.ServerDropItem();

                    itemPickup.transform.position = finalPosition;
                    itemPickup.transform.rotation = Quaternion.Euler(objectRotation);
                    itemPickup.transform.localScale = scale;

                    if (itemPickup is AmmoPickup ammoPickup)
                        ammoPickup.NetworkSavedAmmo = itemConfig.Ammo;

                    else if (itemPickup is FirearmPickup firearmPickup)
                        firearmPickup.Status = new FirearmStatus((byte)itemConfig.Ammo, itemConfig.Ammo == 0 ? FirearmStatusFlags.None : FirearmStatusFlags.MagazineInserted, itemConfig.Attachments);
                }
            }
        }

        public Vector3 GetRotatedPosition(float rotationDiffY, Vector3 originalPosition) =>
        new(
            originalPosition.x * (float)Math.Cos(rotationDiffY * Math.PI / 180) - originalPosition.z * (float)Math.Sin(rotationDiffY * Math.PI / 180), // Math.PI / 180 : converts angle to radian
            originalPosition.y,
            originalPosition.z * (float)Math.Cos(rotationDiffY * Math.PI / 180) + originalPosition.x * (float)Math.Sin(rotationDiffY * Math.PI / 180)
        );
    }
}