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
        [JsonPropertyName("FreezePermission")] public string FreezePermission { get; set; } = null!;
        [JsonPropertyName("ToxicEnable")] public bool ToxicEnable { get; set; } = true;
        [JsonPropertyName("DamageToxicPerSec")] public int DamageToxicPerSec { get; set; } = 10;
        [JsonPropertyName("ToxicRadius")] public float ToxicRadius { get; set; } = 60f;
        [JsonPropertyName("ToxicLifetTime")] public float ToxicLifeTime { get; set; } = 5f;
        [JsonPropertyName("ToxicPermission")] public string ToxicPermission { get; set; } = null!;
        [JsonPropertyName("ToxicFreezeEnable")] public bool ToxicFreezeEnable { get; set; } = true;
        [JsonPropertyName("InToxicSound")] public string InToxicSound { get; set; } = "sounds/ui/lobby_notification_chat.vsnd";
    }
}
