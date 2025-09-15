using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using Vector = CounterStrikeSharp.API.Modules.Utils.Vector;

namespace Core;


public partial class Main
{
    public void ParticleCreate(Vector startPos, Vector endPos, float duration, Color color, QAngle rot = null!, bool trigger = false)
    {
        var particle = Utilities.CreateEntityByName<CEnvParticleGlow>("env_particle_glow");
        if (particle == null)
        {
            Server.PrintToConsole(" > Error to create Particle - Functions -> ParticleCreate");
            return;
        }
        particle.StartActive = true;
        particle.EffectName = Config.FreezeModel;
        particle.ColorTint = color;
        particle.RenderMode = RenderMode_t.kRenderGlow;

        if (rot != null)
        {
            particle.Teleport(startPos, rot);
        }
        else
        {
            particle.Teleport(startPos);
        }

        particle.AcceptInput("SetControlPoint", value: $"0: {startPos.X} {startPos.Y} {startPos.Z}");
        particle.AcceptInput("SetControlPoint", value: $"1: {endPos.X} {endPos.Y} {endPos.Z}");

        particle.DispatchSpawn();
        particle.AcceptInput("Start");

        Particle[particle.Index] = AddTimer(duration, () => { SafeRemove(particle); });
    }
    public void ParticleSmokeCreate(Vector startPos, float duration, Color color)
    {
        var particle = Utilities.CreateEntityByName<CEnvParticleGlow>("env_particle_glow");
        if (particle == null)
        {
            Server.PrintToConsole(" > Error to create Particle - Functions -> ParticleSmokeCreate");
            return;
        }
        particle.StartActive = true;
        particle.EffectName = Config.FreezeSmokeParticle;
        particle.AlphaScale = 0.1f;
        particle.RadiusScale = 0.5f;

        particle.ColorTint = color;

        particle.RenderFX = RenderFx_t.kRenderFxFadeIn;
        particle.RenderMode = RenderMode_t.kRenderGlow;

        particle.Teleport(startPos);

        particle.DispatchSpawn();
        particle.AcceptInput("Start");

        Particle[particle.Index] = AddTimer(duration, () => { SafeRemove(particle); });
    }

    public void PrecacheResource(ResourceManifest mainfest)
    {
        mainfest.AddResource(Config.FreezeModel);
        mainfest.AddResource(Config.FreezeSound);
        mainfest.AddResource(Config.FreezeSmokeParticle);
        mainfest.AddResource(Config.InToxicSound);
    }
}
