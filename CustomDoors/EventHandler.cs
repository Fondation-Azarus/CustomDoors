using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Ammo;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Usables.Scp244;
using MapGeneration;
using Mirror;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Core.Items;
using PluginAPI.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace CustomDoors;

public sealed class EventHandler
{
    [PluginEvent(ServerEventType.RoundStart)] // Doesn't work anymore at WaitingForPlayers for some reason. :/
    public void RoundStart()
    {
        List<SpawnableConfig> spawnableObjects = [.. PluginClass.Singleton.config.Doors, .. PluginClass.Singleton.config.Items];

        if (RoomIdentifier.AllRoomIdentifiers == null)
        {
            Log.Error($"{nameof(RoomIdentifier.AllRoomIdentifiers)} null : process aborted.");
            return;
        }

        Dictionary<RoomIdentifier, int> roomId = [];

        //foreach (SpawnableConfig spawnableConfig in PluginClass.Singleton.config.SpawnableObjects) bugged
        foreach (SpawnableConfig spawnableConfig in spawnableObjects)
        {
            if (!RoomIdentifier.AllRoomIdentifiers.TryGetFirst(r =>
                r.name == spawnableConfig.RoomName
                && (spawnableConfig.Id == -1 || !roomId.ContainsKey(r) || roomId[r] == spawnableConfig.Id)
                , out RoomIdentifier room))
            {
                Log.Error($"Romm wasn't found : {spawnableConfig.RoomName}, continues to the next room.");
                continue;
            }

            roomId[room] = spawnableConfig.Id;

            Vector3 finalScale = new(spawnableConfig.ScaleX, spawnableConfig.ScaleY, spawnableConfig.ScaleZ);
            
            Quaternion finalRotation = Quaternion.Euler(new(spawnableConfig.RelativeRotationX, spawnableConfig.RelativeRotationY + room.transform.rotation.eulerAngles.y, spawnableConfig.RelativeRotationZ));

            Vector3 finalPosition = room.transform.TransformPoint(new(spawnableConfig.RelativePositionX, spawnableConfig.RelativePositionY, spawnableConfig.RelativePositionZ));

            if (spawnableConfig is DoorConfig doorConfig)
            {
                uint prefabId = doorConfig.Type switch
                {
                    "HCZ" => 2295511789,
                    "EZ" => 1883254029,
                    _ => 3038351124
                };

                DoorVariant door = Object.Instantiate(NetworkClient.prefabs[prefabId].gameObject, finalPosition, finalRotation).GetComponent<DoorVariant>();
                door.transform.localScale = new(spawnableConfig.ScaleX, spawnableConfig.ScaleY, spawnableConfig.ScaleZ);
                door.RequiredPermissions.RequiredPermissions = doorConfig.KeycardPermissions;
                
                door.ServerChangeLock(DoorLockReason.SpecialDoorFeature, doorConfig.Locked);
                door.NetworkTargetState = doorConfig.Opened;

                NetworkServer.Spawn(door.gameObject);

                door.RegisterRooms();

                //if (DoorVariant.DoorsByRoom.TryGetValue(room, out HashSet<DoorVariant> hash))
                //    hash.Add(door);

                if (doorConfig.Health > 0 && door is BreakableDoor breakableDoor)
                {
                    breakableDoor.MaxHealth = doorConfig.Health;
                    breakableDoor.RemainingHealth = doorConfig.Health;
                }

                if (!string.IsNullOrEmpty(doorConfig.Name))
                    door.DoorName = doorConfig.Name;
            }

            else if (spawnableConfig is ItemConfig itemConfig)
            {
                ItemPickup itemPickup = ItemPickup.Create(itemConfig.Item, finalPosition, finalRotation);
                itemPickup.Transform.localScale = finalScale;
                itemPickup.Spawn();

                if (itemPickup.OriginalObject is FirearmPickup firearmPickup)
                    ((MagazineModule)firearmPickup.Template.Modules.FirstOrDefault(m => m is MagazineModule)).ServerSetInstanceAmmo(itemPickup.Serial, itemConfig.Ammo);

                else if (itemPickup.OriginalObject is AmmoPickup ammoPickup)
                    ammoPickup.NetworkSavedAmmo = itemConfig.Ammo;

                else if (itemPickup.OriginalObject is Scp244DeployablePickup scp244)
                    scp244.State = (Scp244State)itemConfig.Ammo;
            }

            else
                Log.Error($"Config type wasn't found : {spawnableConfig}, continues to the next element.");
        }
    }
}