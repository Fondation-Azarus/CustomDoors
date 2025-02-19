using CommandSystem;
using MapGeneration;
using PluginAPI.Core;
using System;
using System.Linq;
using UnityEngine;

namespace CustomDoors.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public sealed class PositionCommand : ICommand
{
    public string Command { get; } = "Position";

    public string[] Aliases { get; } = [];

    public string Description { get; } = "Gives you your position.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player player = Player.Get(((CommandSender)sender).SenderId);
        RoomIdentifier? room = player.Room;
        if (room == null) 
        {
            response = "You're not in a room.";
            return false;
        }

        Vector3 position = player.Position;

        if (arguments.Count > 0 && bool.TryParse(arguments.First(), out bool doRaycast) && doRaycast && Physics.Raycast(player.Camera.position, player.Camera.forward, out RaycastHit hitInfo, 50))
        {
            position = hitInfo.point;
            if (RoomIdentifier.RoomsByCoordinates.TryGetValue(new Vector3Int(Mathf.RoundToInt(hitInfo.point.x / RoomIdentifier.GridScale.x), Mathf.RoundToInt(hitInfo.point.y / 100f), Mathf.RoundToInt(hitInfo.point.z / RoomIdentifier.GridScale.z)), out RoomIdentifier value))
                room = value;
        }

        Vector3 relativePos = room.transform.InverseTransformPoint(position);
        response = $"Room name : {room.name}" +
            $"\nRoom position : {room.transform.position}" +
            $"\nPosition : {position}" +
            $"\nRelative rotation to room (y axis) : {player.Rotation.y - room.transform.rotation.eulerAngles.y}" +
            $"\nRelative position to room :" +
            $"\n{relativePos.x}, {relativePos.y}, {relativePos.z}";

        if (PluginClass.Singleton.config.InfoPositionCommand)
            Log.Info(response);
        return true;
    }
}