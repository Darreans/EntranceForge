using Unity.Entities;
using ProjectM.CastleBuilding;

namespace EntranceForge.Systems
{
    public class TypeRegistrationSystem : SystemBase
    {
        public override void OnCreate()
        {
            try
            {
                var typeIndex = TypeManager.GetTypeIndex<DynamicBuffer<CastleBuildingAttachedChildrenBuffer>>();
            }
            catch (System.Exception)
            {
            }
        }

        public override void OnUpdate()
        {
            Enabled = false;
        }
    }
}