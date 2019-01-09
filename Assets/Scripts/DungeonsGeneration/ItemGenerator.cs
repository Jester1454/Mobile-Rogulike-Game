using ActiveAbility;
using Environment;
using Environment.Buffs;
using ItemsSystem;

namespace DungeonsGeneration
{   
    public class ItemGenerator
    {
        private BehaviourDictionary behaviourDictionary;
        
        public ItemGenerator()
        {
            behaviourDictionary = new BehaviourDictionary();
        }

        public AbstractItem GetRandomObject(RoomType roomType)
        {
            return behaviourDictionary.GetRandomItem(roomType);
        }
    }
}
