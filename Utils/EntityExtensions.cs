using Il2CppInterop.Runtime;
using ProjectM;
using Unity.Entities;
using Stunlock.Core;

namespace EntranceForge.Utils
{
    public static class EntityExtensions
    {
        private static EntityManager Em => VWorld.EntityManager;

        public static bool Exists(this Entity entity)
        {
            return entity.Index > 0 && Em != default && Em.Exists(entity);
        }

        public static bool Has<T>(this Entity entity) where T : struct
        {
            if (entity == Entity.Null || Em == default) return false;
            ComponentType ct = new ComponentType(Il2CppType.Of<T>());
            return Em.HasComponent(entity, ct);
        }

        public static bool HasBuffer<T>(this Entity entity) where T : struct
        {
            if (entity == Entity.Null || Em == default) return false;
           
            return Em.HasBuffer<T>(entity);
        }

        public static T Read<T>(this Entity entity) where T : struct
        {
            if (Em == default) return default(T);
            return Em.GetComponentData<T>(entity);
        }

        public static bool TryRead<T>(this Entity entity, out T component) where T : struct
        {
            component = default;
            if (entity.Exists() && entity.Has<T>())
            {
                try
                {
                    component = Em.GetComponentData<T>(entity);
                    return true;
                }
                catch { return false; }
            }
            return false;
        }

        public static void Write<T>(this Entity entity, T componentData) where T : struct
        {
            if (Em == default || !entity.Exists()) return;
            Em.SetComponentData(entity, componentData);
        }

        public static DynamicBuffer<T> ReadBuffer<T>(this Entity entity) where T : struct
        {
            if (Em == default || !entity.Exists() || !Em.HasBuffer<T>(entity)) return default;
         
            return Em.GetBuffer<T>(entity);
        }

        public static PrefabGUID GetPrefabGUID(this Entity entity)
        {
            if (entity.Exists() && entity.Has<PrefabGUID>())
            {
                return entity.Read<PrefabGUID>();
            }
            return PrefabGUID.Empty;
        }
    }
}