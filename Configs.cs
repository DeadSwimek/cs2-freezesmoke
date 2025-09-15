using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace Core
{
    public class ConfigSounds : BasePluginConfig
    {
        [JsonPropertyName("FreezeModel")] public string FreezeModel { get; set; } = "particles/ui/annotation/ui_annotation_line_segment_arrow.vpcf";
        [JsonPropertyName("FreezeSmokeParticle")] public string FreezeSmokeParticle { get; set; } = "particles/explosions_fx/explosion_smokegrenade_init.vpcf";
        [JsonPropertyName("FreezeSound")] public string FreezeSound { get; set; } = "sounds/physics/snow/snow_impact_bullet6.vsnd";
        [JsonPropertyName("FreezeRadius")] public float FreezeRadius { get; set; } = 60f;
        [JsonPropertyName("FreezeLifeTime")] public float FreezeLifeTime { get; set; } = 10f;
        [JsonPropertyName("FreezeEnable")] public bool FreezeEnable { get; set; } = true;
    }
}
