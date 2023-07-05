using Interactables.Interobjects.DoorUtils;
using System.ComponentModel;

namespace CustomDoors;

public sealed class Config
{
    [Description("Inform of position command's response ? (sends a debug message to the server console when using position command) :")]
    public bool InfoPositionCommand { get; set; } = false;

    [Description("Inform of attachments command's response ? (sends a debug message to the server console when using attachments command) :")]
    public bool InfoAttachmentsCommand { get; set; } = false;

    /*[Description("List of spawnable objects and their caracteristics")] Doesn't work : if you restart your server configs get changed to a useless config file that only contains SpawnableConfig but no DoorConfig nor ItemConfig 
    public SpawnableConfig[] SpawnableObjects { get; set; } = new SpawnableConfig[5]
    {
        new DoorConfig() { Type = "HCZ", RoomName = "Outside", RelativePositionX =29, RelativePositionY = -8.35f, RelativePositionZ = -42.8f, ScaleX = 1, ScaleY =1, ScaleZ = 1, KeycardPermissions = KeycardPermissions.ExitGates },
        new DoorConfig() { Type = "EZ", RoomName = "LCZ_Airlock (1)", DefaultRoomRotationYAxis = 90, RelativePositionX = -3.5f, RelativePositionY = 0.9685f, RelativePositionZ= 0, ScaleX = 1, ScaleY =1, ScaleZ = 1, KeycardPermissions = KeycardPermissions.ContainmentLevelOne, Opened = true },
        new ItemConfig() { Item = ItemType.Medkit, RoomName = "LCZ_ClassDSpawn (1)", ScaleX = 1, ScaleY = 1, ScaleZ = 1 },
        new ItemConfig() { Item = ItemType.Medkit, RoomName = "LCZ_ClassDSpawn (1)", ScaleX = 1, ScaleY = 1, ScaleZ = 1, RelativeRotationY = 90 },
        new ItemConfig() { Item = ItemType.Medkit, RoomName = "LCZ_ClassDSpawn (1)", ScaleX = 2, ScaleY = 1, ScaleZ = 2 },
    };*/

    [Description("List of doors and their caracteristics")]
    public DoorConfig[] Doors { get; set; } = new DoorConfig[2]
    {
        new DoorConfig() { Type = "HCZ", RoomName = "Outside", RelativePositionX =29, RelativePositionY = -8.35f, RelativePositionZ = -42.8f, ScaleX = 1, ScaleY =1, ScaleZ = 1, KeycardPermissions = KeycardPermissions.ExitGates },
        new DoorConfig() { Type = "EZ", RoomName = "LCZ_Airlock (1)", DefaultRoomRotationYAxis = 90, RelativePositionX = -3.5f, RelativePositionY = 0, RelativePositionZ = 0, ScaleX = 1, ScaleY =1, ScaleZ = 1, KeycardPermissions = KeycardPermissions.ContainmentLevelOne, Opened = false }
    };

    [Description("List of items and their caracteristics")]
    public ItemConfig[] Items { get; set; } = new ItemConfig[3]
    {
        new ItemConfig() { RelativePositionY = 1, Item = ItemType.Medkit, RoomName = "LCZ_ClassDSpawn (1)", ScaleX = 1, ScaleY = 1, ScaleZ = 1 },
        new ItemConfig() { RelativePositionY = 1, Item = ItemType.Medkit, RoomName = "LCZ_ClassDSpawn (1)", ScaleX = 1, ScaleY = 1, ScaleZ = 1, RelativeRotationY = 90 },
        new ItemConfig() { RelativePositionY = 1, Item = ItemType.Medkit, RoomName = "LCZ_ClassDSpawn (1)", ScaleX = 2, ScaleY = 1, ScaleZ = 2 },
    };
}

public abstract class SpawnableConfig
{
    public string RoomName { get; set; }

    public float DefaultRoomRotationYAxis { get; set; }

    public float RelativePositionX { get; set; }
    public float RelativePositionY { get; set; }
    public float RelativePositionZ { get; set; }

    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public float ScaleZ { get; set; }

    public float RelativeRotationX { get; set; }
    public float RelativeRotationY { get; set; }
    public float RelativeRotationZ { get; set; }
}

public sealed class DoorConfig : SpawnableConfig
{
    public string Type { get; set; }

    public KeycardPermissions KeycardPermissions { get; set; }
    public bool Opened { get; set; }
    public bool Locked { get; set; }
}

public sealed class ItemConfig : SpawnableConfig
{
    public ItemType Item { get; set; }
    public ushort Ammo { get; set; }
    public uint Attachments { get; set; }
}

/*public class PrimitiveConfig : SpawnableConfig
{
    public PrimitiveType PrimitiveType { get; set; }
    public Color Color { get; set; }
}

public class LightConfig : SpawnableConfig
{
    public PrimitiveType PrimitiveType { get; set; }
    public Color Color { get; set; }
    public float Range { get; set; }
}*/