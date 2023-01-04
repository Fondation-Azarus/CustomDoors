using CommandSystem;
using MapGeneration;
using PluginAPI.Core;
using System;

namespace CustomDoors
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

            
            /*if (RoomIdentifier.RoomsByCoordinates.TryGetValue(new Vector3Int(Mathf.RoundToInt(player.Position.x / RoomIdentifier.GridScale.x), Mathf.RoundToInt(player.Position.y / 100f), Mathf.RoundToInt(player.Position.z / RoomIdentifier.GridScale.x)), out var value))
                room = value;
            else
                room =  RoomIdentifier.RoomsByCoordinates.Values.OrderBy(r => Vector3.Distance(r.transform.position, player.Position)).FirstOrDefault();*/



            response = $"{room.name} :\nRoom position : {room.transform.position}\nRoom rotation : {room.transform.rotation.eulerAngles}\nYour position : {player.Position}\nRelative position to room :\nx = {(player.Position - room.transform.position).x}\ny = {(player.Position - room.transform.position).y}\nz = {(player.Position - room.transform.position).z}";
            
            if (PluginClass.Singleton.config.DebugCommand)
                Log.Debug(response);
            return true;
        }
    }
}