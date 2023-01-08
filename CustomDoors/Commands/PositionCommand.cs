using CommandSystem;
using InventorySystem.Items.Pickups;
using InventorySystem;
using MapGeneration;
using Mirror;
using PluginAPI.Core;
using System;
using System.Linq;
using UnityEngine;

namespace CustomDoors.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PositionCommand : ICommand
    {
        public string Command { get; } = "Position";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Gives you your position.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(((CommandSender)sender).SenderId);
            RoomIdentifier room = player.Room;
            Vector3 position = player.Position;

            if (arguments.Count > 0 && bool.TryParse(arguments.First(), out bool doRaycast) && doRaycast && Physics.Raycast(player.Camera.position, player.Camera.forward, out RaycastHit hitInfo, 50))
            {
                position = hitInfo.point;
                if (RoomIdentifier.RoomsByCoordinates.TryGetValue(new Vector3Int(Mathf.RoundToInt(hitInfo.point.x / RoomIdentifier.GridScale.x), Mathf.RoundToInt(hitInfo.point.y / 100f), Mathf.RoundToInt(hitInfo.point.z / RoomIdentifier.GridScale.z)), out RoomIdentifier value))
                    room = value;
            }
            
            response = $"{room.name} :\nRoom position : {room.transform.position}\nRoom rotation : {room.transform.rotation.eulerAngles}\nPosition : {position}\nRelative position to room :\nx = {(position - room.transform.position).x}\ny = {(position - room.transform.position).y}\nz = {(position - room.transform.position).z}";

            if (PluginClass.Singleton.config.InfoPositionCommand)
                Log.Info(response);
            return true;
        }
    }
}