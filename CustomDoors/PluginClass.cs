using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace CustomDoors;

public sealed class PluginClass
{
    public static PluginClass Singleton { get; private set; }

    [PluginPriority(LoadPriority.Low)]
    [PluginEntryPoint("Custom Doors", "1.4.0", "More doors/items = More fun ! Allows server owners to add custom doors and items.", "Bonjemus")]
    void LoadPlugin()
    {
        Singleton = this;

        EventManager.RegisterEvents<EventHandler>(this);

        PluginHandler eventHandler = PluginHandler.Get(this);
        eventHandler.SaveConfig(this, nameof(config));
    }

    [PluginConfig("configs/CustomDoors.yml")]
    public Config config;
}