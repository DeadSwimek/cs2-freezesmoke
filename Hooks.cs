using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using Vector = CounterStrikeSharp.API.Modules.Utils.Vector;

namespace Core;

public partial class Main
{
    HookResult EventDecoyFiring(EventDecoyFiring @event, GameEventInfo inf)
    {
        inf.DontBroadcast = true;

        var thrower = @event.Userid;
        if (thrower == null)
            return HookResult.Continue;
        if(Config.ToxicPermission != null)
        {
            if(!AdminManager.PlayerHasPermissions(thrower, Config.ToxicPermission))
            {
                return HookResult.Continue;
            }
        }


        float radius = Config.ToxicRadius;
        float duration = Config.ToxicLifeTime;
        bool enable = Config.ToxicEnable;
        string sound = Config.InToxicSound;
        if (!enable) return HookResult.Continue;
        int points = 20;

        float centerX = (float)@event.X;
        float centerY = (float)@event.Y;
        float centerZ = (float)@event.Z;
        Vector center = new Vector(centerX, centerY, centerZ);

        foreach (var player in Utilities.GetPlayers())
        {
            var playerOrigin = player.PlayerPawn?.Value!.AbsOrigin!;
            float dx = center.X - playerOrigin.X;
            float dy = center.Y - playerOrigin.Y;
            float dz = center.Z - playerOrigin.Z;
            float distance = MathF.Sqrt(dx * dx + dy * dy + dz * dz);

            if (distance <= radius)
            {
                if (player.PawnIsAlive)
                {
                    var pawn = player.PlayerPawn!.Value!;
                    if(Config.ToxicFreezeEnable)
                        Freeze[player.Index] = true;
                    Toxic[player.Index] = true;
                    ToxicTimer[player.Index] = AddTimer(1.0f, () =>
                    {
                        if (Toxic[player.Index] == true)
                        {
                            player.ExecuteClientCommand($"play {sound}");
                            int hp = pawn.Health;
                            pawn.Health = hp - Config.DamageToxicPerSec;
                            pawn.TakeDamageFlags = TakeDamageFlags_t.DFLAG_SUPPRESS_HEALTH_CHANGES;
                            pawn.TakesDamage = true;
                            Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
                            if (hp == 0) pawn.CommitSuicide(false, true);
                        }
                    }, CounterStrikeSharp.API.Modules.Timers.TimerFlags.REPEAT);
                    AddTimer(duration, () => {
                        if (Config.ToxicFreezeEnable)
                            Freeze[player.Index] = false;
                        Toxic[player.Index] = false;
                    });
                }
                ParticleSmokeCreate(new Vector(centerX, centerY, centerZ), duration, Color.Green);
            }
            else
            {
                ParticleSmokeCreate(new Vector(centerX, centerY, centerZ), duration, Color.Gray);
            }
        }
        for (int i = 0; i < points; i++)
        {
            double angle = 2 * Math.PI * i / points;

            float x = centerX + (float)(radius * Math.Cos(angle));
            float z = centerZ;
            float y = centerY + (float)(radius * Math.Sin(angle));

            ParticleCreate(new Vector(x, y, z), new Vector(x, y, z), duration, Color.Green);
        }
        var entity = Utilities.GetEntityFromIndex<CDecoyProjectile>(@event.Entityid);
        if (entity != null)
        {
            entity.Remove();
        }

        return HookResult.Continue;
    }
    HookResult EventSmokegrenadeDetonate(EventSmokegrenadeDetonate @event, GameEventInfo inf)
    {

        var thrower = @event.Userid;
        if (thrower == null)
            return HookResult.Continue;
        if (Config.FreezePermission != null)
        {
            if (!AdminManager.PlayerHasPermissions(thrower, Config.FreezePermission))
            {
                return HookResult.Continue;
            }
        }

        inf.DontBroadcast = true;


        float radius = Config.FreezeRadius;
        float duration = Config.FreezeLifeTime;
        bool enable = Config.FreezeEnable;
        string sound = Config.FreezeSound;
        if (!enable) return HookResult.Continue;
        int points = 20;

        float centerX = (float)@event.X;
        float centerY = (float)@event.Y;
        float centerZ = (float)@event.Z;
        Vector center = new Vector(centerX, centerY, centerZ);

        foreach (var player in Utilities.GetPlayers())
        {
            var playerOrigin = player.PlayerPawn?.Value!.AbsOrigin!;
            float dx = center.X - playerOrigin.X;
            float dy = center.Y - playerOrigin.Y;
            float dz = center.Z - playerOrigin.Z;
            float distance = MathF.Sqrt(dx * dx + dy * dy + dz * dz);
            if (distance <= radius)
            {
                if (player.PawnIsAlive)
                {
                    var pawn = player.PlayerPawn!.Value!;
                    Freeze[player.Index] = true;
                    player.ExecuteClientCommand($"play {sound}");
                    pawn.Render = Color.FromArgb(0, 0, 255);
                    Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");
                    AddTimer(duration, () => { 
                        Freeze[player.Index] = false; 
                        pawn.Render = Color.FromArgb(255, 255, 255);
                        Utilities.SetStateChanged(pawn, "CBaseModelEntity", "m_clrRender");
                    });
                }
                ParticleSmokeCreate(new Vector(centerX, centerY, centerZ), duration, Color.Blue);
            }
            else
            {
                ParticleSmokeCreate(new Vector(centerX, centerY, centerZ), duration, Color.Gray);
            }
        }
        for (int i = 0; i < points; i++)
        {
            double angle = 2 * Math.PI * i / points;

            float x = centerX + (float)(radius * Math.Cos(angle));
            float z = centerZ;
            float y = centerY +(float)(radius * Math.Sin(angle));

            ParticleCreate(new Vector(x, y, z), new Vector(x, y, z), duration, Color.Blue);
        }
        var entity = Utilities.GetEntityFromIndex<CSmokeGrenadeProjectile>(@event.Entityid);
        if (entity != null) {
            entity.Remove();
        }

        return HookResult.Continue;
    }
    HookResult EventPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null) return HookResult.Continue;

        FreezeTimer[player.Index]?.Kill();
        ToxicTimer[player.Index]?.Kill();
        Freeze[player.Index] = false;
        Toxic[player.Index] = false;

        return HookResult.Continue;
    }
    HookResult EventRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        KillAllTimers();

        return HookResult.Continue;
    }
    HookResult EventRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        foreach (var kvp in Particle.ToList()) 
        {
            kvp.Value?.Kill();
            Particle.Remove(kvp.Key);
        }
        return HookResult.Continue;
    }
    HookResult EventPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;

        if (player == null) return HookResult.Continue;
        if (!PlayerIsValid(player)) return HookResult.Continue;
        if (player.IsBot)
            return HookResult.Continue;

        Freeze[player.Index] = false;
        Toxic[player.Index] = false;
        FreezeTimer[player.Index]?.Kill();
        ToxicTimer[player.Index]?.Kill();

        return HookResult.Continue;
    }
}
