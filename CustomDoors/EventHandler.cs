using PluginAPI.Core.Attributes;
using Mirror;
using PluginAPI.Enums;
using UnityEngine;
using Interactables.Interobjects.DoorUtils;
using System.Linq;
using MapGeneration;
using System;

namespace CustomDoors
{
    public class EventHandler
    {
        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void WaitingForPlayers()
        {
            foreach (DoorConfig doorConfig in PluginClass.Singleton.config.Doors)
            {
                RoomIdentifier room = RoomIdentifier.AllRoomIdentifiers.FirstOrDefault(r => r.name == doorConfig.RoomName);
                
                if (room == null)
                    continue;
                
                Vector3 scale = new(doorConfig.ScaleX, doorConfig.ScaleY, doorConfig.ScaleZ);

                float roomRotationDiffY =  doorConfig.defaultRoomRotationYAxis - room.transform.rotation.eulerAngles.y;
                
                Vector3 doorRotation = new(doorConfig.RotationX, doorConfig.RotationY + roomRotationDiffY, doorConfig.RotationZ);

                Vector3 relativePosition = new(doorConfig.RelativePositionX, doorConfig.RelativePositionY, doorConfig.RelativePositionZ);

                Vector3 rotatedRelativePosition = new(
                    relativePosition.x * (float)Math.Cos(roomRotationDiffY * Math.PI / 180) - relativePosition.z * (float)Math.Sin(roomRotationDiffY * Math.PI / 180), // Math.PI / 180 : convert angle to radian
                    relativePosition.y,
                    relativePosition.z * (float)Math.Cos(roomRotationDiffY * Math.PI / 180) + relativePosition.x * (float)Math.Sin(roomRotationDiffY * Math.PI / 180)
                    );

                DoorVariant door = UnityEngine.Object.Instantiate(UnityEngine.Object.FindObjectsOfType<DoorSpawnpoint>().First(d => d.TargetPrefab.name.Contains(doorConfig.Type)).TargetPrefab, room.transform.position + rotatedRelativePosition,  Quaternion.Euler(doorRotation));
                door.transform.localScale = scale;
                door.RequiredPermissions.RequiredPermissions = doorConfig.KeycardPermissions;
                NetworkServer.Spawn(door.gameObject);
                door.ServerChangeLock(DoorLockReason.SpecialDoorFeature, doorConfig.Locked);
                door.TargetState = doorConfig.Opened;
            }
        }
    }
}