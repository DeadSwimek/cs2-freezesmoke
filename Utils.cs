using CounterStrikeSharp.API.Core;
namespace Core
{
    public partial class Main
    {
      
        public static bool PlayerIsValid(CCSPlayerController player)
        {
            if (player == null && player!.IsValid) return false;
            return true;
        }
        public void KillAllTimers()
        {
            for (int i = 0; i < FreezeTimer.Length; i++)
            {
                if (FreezeTimer[i] != null)
                {
                    FreezeTimer[i]?.Kill();
                    FreezeTimer[i] = null;
                }
            }
            for (int i = 0; i < ToxicTimer.Length; i++)
            {
                if (ToxicTimer[i] != null)
                {
                    ToxicTimer[i]?.Kill();
                    ToxicTimer[i] = null;
                }
            }
        }
        void SafeRemove(CEntityInstance ent)
        {
            if (ent != null && ent.IsValid)
                ent.Remove();
        }
    }
}