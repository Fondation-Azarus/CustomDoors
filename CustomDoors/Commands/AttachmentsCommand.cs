using CommandSystem;
using PluginAPI.Core;
using System;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;

namespace CustomDoors.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public sealed class AttachmentsCommand : ICommand
{
    public string Command { get; } = "Attachments";

    public string[] Aliases { get; } = new string[] { };

    public string Description { get; } = "Display your weapon's attachments value.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        ItemBase itemBase = Player.Get(((CommandSender)sender).SenderId).CurrentItem;

        response = "Error.";
        bool allow = false;


        if (itemBase is not Firearm firearm)
            response = "You're not holding a weapon.";


        /*else if (arguments.Count > 0) forceattachments exists actually
        {
            if (uint.TryParse(arguments.FirstOrDefault(), out uint attachment))
            {
                firearm.Status = new FirearmStatus(firearm.Status.Ammo, firearm.Status.Flags, attachment);
                allow = true;
            }
            else
                response = "The argument must be a number (uint).";
        }*/

        else
        {
            response = firearm.Status.Attachments.ToString();
            allow = true;
        }

        if (PluginClass.Singleton.config.InfoAttachmentsCommand)
            Log.Info(response);
        return allow;
    }
}