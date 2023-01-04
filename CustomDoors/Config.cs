using Interactables.Interobjects.DoorUtils;
using System.ComponentModel;

namespace CustomDoors
{
    public class Config
    {
        [Description("Debug command ? (sends a debug message to the server console when using position command) :")]
        public bool DebugCommand { get; set; } = false;

        [Description("List of doors and their caracteristics")]
        public DoorConfig[] Doors { get; set; } = new DoorConfig[2]
        {
            new DoorConfig() { Type = "HCZ", RoomName = "Outside", RelativePositionX =29, RelativePositionY = -8.35f, RelativePositionZ = -42.8f, ScaleX = 1, ScaleY =1, ScaleZ = 1, KeycardPermissions = KeycardPermissions.ExitGates },
            new DoorConfig() { Type = "EZ", RoomName = "LCZ_Airlock (1)", defaultRoomRotationYAxis = 90, RelativePositionX = -3.5f, RelativePositionY = 0.9685f, RelativePositionZ= 0, ScaleX = 1, ScaleY =1, ScaleZ = 1, KeycardPermissions = KeycardPermissions.ContainmentLevelOne, Opened = true }
        };
    }

    public struct DoorConfig
    {
        public string Type { get; set; }
        public string RoomName { get; set; }

        public float defaultRoomRotationYAxis { get; set; }

        public float RelativePositionX { get; set; }
        public float RelativePositionY { get; set; }
        public float RelativePositionZ { get; set; }

        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }

        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }
        public KeycardPermissions KeycardPermissions { get; set; }
        public bool Opened { get; set; }
        public bool Locked { get; set; }
    }
}