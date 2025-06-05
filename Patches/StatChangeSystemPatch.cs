using HarmonyLib;
using ProjectM;
using ProjectM.Shared;
using Unity.Entities;
using Unity.Collections;
using Stunlock.Core;
using EntranceForge.Utils;
using EntranceForge.Systems;
using ProjectM.CastleBuilding;
using ProjectM.Gameplay.Systems;

namespace EntranceForge.Patches
{
    [HarmonyPatch(typeof(StatChangeSystem), nameof(StatChangeSystem.OnUpdate))]
    public static class StatChangeSystem_DamageToEntrances_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(StatChangeSystem __instance)
        {
            if (!VWorld.IsServerWorldReady()) return true;

            EntityManager em = VWorld.EntityManager;
            NativeArray<Entity> statChangeEventEntities = __instance._Query.ToEntityArray(Allocator.Temp);

            try
            {
                foreach (Entity eventEntity in statChangeEventEntities)
                {
                    if (!eventEntity.Exists() || !eventEntity.Has<StatChangeEvent>()) continue;

                    StatChangeEvent originalEvent = eventEntity.Read<StatChangeEvent>();

                    if (originalEvent.StatType != StatType.Health || originalEvent.Change >= 0) continue;

                    Entity targetEntity = originalEvent.Entity;
                    Entity sourceOfDamageEffect = originalEvent.Source;

                    if (!targetEntity.Exists() || !targetEntity.Has<CastleHeartConnection>()) continue;

                    CastleHeartConnection chc = targetEntity.Read<CastleHeartConnection>();
                    Entity castleHeartEntity = chc.CastleHeartEntity._Entity;
                    if (!castleHeartEntity.Exists()) continue;

                    if (!targetEntity.Has<PrefabGUID>()) continue;
                    PrefabGUID targetPrefabGuid = targetEntity.Read<PrefabGUID>();
                    if (!EntranceDefenseRules.IsEntranceProtectedType(targetPrefabGuid)) continue;

                    PrefabGUID abilityGuid = default;
                    if (sourceOfDamageEffect.Exists() && sourceOfDamageEffect.Has<PrefabGUID>())
                    {
                        abilityGuid = sourceOfDamageEffect.Read<PrefabGUID>();
                    }
                    else if (sourceOfDamageEffect.Exists() && sourceOfDamageEffect.Has<EntityOwner>())
                    {
                        Entity ownerEntity = sourceOfDamageEffect.Read<EntityOwner>().Owner;
                        if (ownerEntity.Exists() && ownerEntity.Has<PrefabGUID>())
                        {
                            abilityGuid = ownerEntity.Read<PrefabGUID>();
                        }
                        else continue;
                    }
                    else continue;

                    if (!EntranceDefenseRules.IsAbilityWatched(abilityGuid)) continue;

                    bool hasDoor = false;
                    if (targetEntity.HasBuffer<CastleBuildingAttachedChildrenBuffer>())
                    {
                        var attachments = targetEntity.ReadBuffer<CastleBuildingAttachedChildrenBuffer>();
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
                    if (!hasDoor) continue;

                    StatChangeEvent modifiedEvent = originalEvent;
                    modifiedEvent.Change = -0.01f;
                    modifiedEvent.StatChangeFlags = (int)StatChangeFlag.None;

                    if (eventEntity.Exists())
                    {
                        eventEntity.Write(modifiedEvent);
                    }
                }
            }
            catch (System.Exception)
            {

            }
            finally
            {
                if (statChangeEventEntities.IsCreated) statChangeEventEntities.Dispose();
            }

            return true;
        }
    }
}