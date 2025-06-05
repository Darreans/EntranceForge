using ProjectM;
using ProjectM.Shared; 
using Unity.Entities;
using EntranceForge.Utils; 

namespace EntranceForge.Utils
{
    public static class Helper
    {
      
        public static void DestroyEntity(Entity entity, DestroyReason destroyReason = DestroyReason.Default, DestroyDebugReason destroyDebugReason = DestroyDebugReason.None)
        {
            if (!VWorld.IsServerWorldReady() || !entity.Exists()) return;
            DestroyUtility.CreateDestroyEvent(VWorld.EntityManager, entity, destroyReason, destroyDebugReason);
        }
    }
}