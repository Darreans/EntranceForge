using System.Collections.Generic;
using ProjectM;
using ProjectM.CastleBuilding;
using Stunlock.Core;
using Unity.Entities;
using EntranceForge.Data;
using EntranceForge.Utils;



namespace EntranceForge.Managers
{
    public static class RaidEntrancesFixManager
    {
        private static readonly Dictionary<PrefabGUID, float> WatchedGolemAbilities = new Dictionary<PrefabGUID, float>()
        {
            { PrefabGuids.AB_Shapesfhit_Golem_T02_MeleeAttack_Hit, 50f },
            { PrefabGuids.AB_Shapesfhit_Golem_T02_FistSlam_Hit, 75f },
            { PrefabGuids.AB_Shapeshift_Golem_T02_GroundSlam_Hit, 150f },
            { PrefabGuids.AB_Shapeshift_Golem_T02_Ranged_Projectile, 100f},
        };

        private static readonly HashSet<PrefabGUID> ProtectedEntranceGuids = new HashSet<PrefabGUID>()
        {
            PrefabGuids.TM_Castle_Wall_Tier02_Stone_Entrance,
            PrefabGuids.TM_Castle_Wall_Tier02_Stone_EntranceWide
        };

        public static void Initialize()
        {

            EntranceForgeCustomEvents.OnUnitHealthChanged += HandleOnUnitHealthChanged;
        }

        public static void Dispose()
        {
            EntranceForgeCustomEvents.OnUnitHealthChanged -= HandleOnUnitHealthChanged;
        }

        public static void HandleOnUnitHealthChanged(Entity sourceEntity, Entity eventEntity, Entity target, PrefabGUID ability)
        {
            if (!VWorld.IsServerWorldReady() || !target.Exists()) return;

            if (!target.Has<CastleHeartConnection>()) return;
            var chc = target.Read<CastleHeartConnection>();
            var castleHeartEntity = chc.CastleHeartEntity._Entity;

            if (!castleHeartEntity.Exists()) return;

            PrefabGUID targetPrefabGuid = target.GetPrefabGUID();
            if (targetPrefabGuid == PrefabGUID.Empty || !ProtectedEntranceGuids.Contains(targetPrefabGuid)) return;

            if (!WatchedGolemAbilities.ContainsKey(ability)) return;

            bool hasDoor = false;
            if (target.HasBuffer<CastleBuildingAttachedChildrenBuffer>())
            {
                var attachments = target.ReadBuffer<CastleBuildingAttachedChildrenBuffer>();
                foreach (var attachment in attachments)
                {
                    var childEntity = attachment.ChildEntity._Entity;
                    if (childEntity.Exists() && childEntity.Has<Door>())
                    {
                        hasDoor = true;
                        break;
                    }
                }
            }

            if (!hasDoor) return;

            if (eventEntity.Exists())
            {
                Helper.DestroyEntity(eventEntity);
            }
        }
    }

    
    public static class EntranceForgeCustomEvents
    {
        public static event System.Action<Entity, Entity, Entity, PrefabGUID> OnUnitHealthChanged;
       
        public static void Invoke(Entity s, Entity e, Entity t, PrefabGUID a) => OnUnitHealthChanged?.Invoke(s, e, t, a);
    }
}