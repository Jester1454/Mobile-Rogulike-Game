using ItemsSystem;
using Managers;
using UnityEngine;
using View.Enemy;

namespace Environment.Enemy
{
    public class CommonEnemy : AbstractEnemy
    {
        private CommonEnemyView view;
        
        public CommonEnemy(int damage,  int id, ItemType itemType, int weight, RoomType itemLocation) : 
            base(damage, id, itemType, weight, itemLocation)
        {
            Damage = damage;
        }

        public override IView View
        {
            get { return view; }
            set { view = (CommonEnemyView) value; }
        }

        public override void Show(Room room)
        {
            view = GameObject.Instantiate
            (
                PrefabsManager.LoadPrefab(PrefabsManager.PrefabsList.DefaultEnemy)
            ).GetComponent<CommonEnemyView>();
            
            CurrentPosition = room;
            view.ShowView(Damage, room.WorldPosition);
        }

        public override void PlayerEnterTheRoom(Room room)
        {
            base.PlayerEnterTheRoom(room);
            view.Destroy();
        }

        public override void Dead()
        {
            CurrentPosition.RemoveItemFromRoom();
            view.Destroy();
        }

        public override object Create(params int[] stats)
        {
            CommonEnemy enemy = new CommonEnemy(stats[0], Id, ItemType, Weight, ItemLocation);
            return enemy;
        }
    }
}
