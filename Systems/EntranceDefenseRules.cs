using Stunlock.Core;
using System.Collections.Generic;
using EntranceForge.Data; 

namespace EntranceForge.Systems
{
    public static class EntranceDefenseRules
    {
        public static readonly Dictionary<PrefabGUID, float> WatchedGolemAbilities = new Dictionary<PrefabGUID, float>()
        {
            { PrefabGuids.AB_Shapesfhit_Golem_T02_MeleeAttack_Hit, 50f },
            { PrefabGuids.AB_Shapesfhit_Golem_T02_FistSlam_Hit, 75f },
            { PrefabGuids.AB_Shapeshift_Golem_T02_GroundSlam_Hit, 150f },
            { PrefabGuids.AB_Shapeshift_Golem_T02_Ranged_Projectile, 100f},
        };

        public static readonly HashSet<PrefabGUID> ProtectedEntranceGuids = new HashSet<PrefabGUID>()
        {
            PrefabGuids.TM_Castle_Wall_Tier02_Stone_Entrance,
            PrefabGuids.TM_Castle_Wall_Tier02_Stone_EntranceWide
        };

        public static bool IsAbilityWatched(PrefabGUID abilityGuid)
        {
            return WatchedGolemAbilities.ContainsKey(abilityGuid);
        }

        public static bool IsEntranceProtectedType(PrefabGUID entranceGuid)
        {
            return ProtectedEntranceGuids.Contains(entranceGuid);
        }
    }
}