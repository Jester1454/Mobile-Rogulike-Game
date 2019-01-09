using ItemsSystem;
using Managers;
using View.Enemy;

namespace Environment.Enemy
{
    public abstract class AbstractEnemy : AbstractItem, IEnemy
    {
        protected int Damage;
        protected Room CurrentPosition;
        
        public AbstractEnemy(int damage,  int id, ItemType itemType, int weight, RoomType itemLocation) : 
            base(id, itemType, weight, itemLocation)
        {
            Damage = damage;
        }

        public abstract IView View { get; set; }

        public override void Show(Room room)
        {
            
        }

        public override void PlayerEnterTheRoom(Room room)
        {
            MakeDamage();
            room.RemoveItemFromRoom();
        }

        public virtual void MakeDamage()
        {
            GameManager.Instance.Player.TakeDamage(Damage);
        }

        public abstract void Dead();

        public abstract override object Create(params int[] stats);
    }
}
