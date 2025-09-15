using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.UserMessages;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;


namespace Core;
public partial class Main : BasePlugin, IPluginConfig<ConfigSounds>

{

    public override string ModuleAuthor => "DeadSwim";
    public override string ModuleName => "SpecialsGrenade";
    public override string ModuleVersion => "2.0";

    public required ConfigSounds Config { get; set; }

    /// <summary>
    /// ////////////////////////////////////////
    /// </summary>
    // Player's save public infos
    public static bool?[] Freeze = new bool?[64];
    public static bool?[] Toxic = new bool?[64];
    // Timers
    public static readonly Timer?[] FreezeTimer = new Timer?[64];
    Dictionary<uint, Timer> Particle = new();
    /// <summary>
    /// ///////////////////////////////////////
    /// </summary>

    public void OnConfigParsed(ConfigSounds config)
    {
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        Console.WriteLine(" ");
        RegisterEventHandler<EventRoundStart>(EventRoundStart);
        RegisterEventHandler<EventPlayerSpawn>(EventPlayerSpawn);
        RegisterEventHandler<EventPlayerDisconnect>(EventPlayerDisconnect);
        RegisterEventHandler<EventRoundEnd>(EventRoundEnd);
        RegisterEventHandler<EventSmokegrenadeDetonate>(EventSmokegrenadeDetonate);
        RegisterEventHandler<EventDecoyFiring>(EventDecoyFiring);
        RegisterListener<Listeners.OnServerPrecacheResources>(PrecacheResource);

        RegisterListener<Listeners.OnMapEnd>(() => { KillAllTimers(); });
            RegisterListener<Listeners.OnTick>(() => {
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                var ent = NativeAPI.GetEntityFromIndex(i);
                if (ent == 0)
                    continue;

                var client = new CCSPlayerController(ent);
                if (client == null || !client.IsValid || !client.PawnIsAlive)
                    continue;
                if (Freeze[client.Index] == true && client.PawnIsAlive)
                {
                        if (client.PlayerPawn.Value is { } playerPawn)
                            playerPawn.VelocityModifier = 0.0f;
                        if (client.Buttons.HasFlag(PlayerButtons.Jump))
                            client.PlayerPawn!.Value!.AbsVelocity.Z = 0;
                }
            }
        });
    }    
}