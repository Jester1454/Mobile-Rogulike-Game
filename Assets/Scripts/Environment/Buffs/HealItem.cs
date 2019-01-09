using ItemsSystem;
using Managers;
using UnityEngine;
using View.Buffs;
using View.Enemy;

namespace Environment.Buffs
{
	public class HealItem : AbstractItem
	{

		private int healValue;
		private HealView view;
			
		public HealItem(int healValue, int id, ItemType itemType, int weight, RoomType itemLocation) : 
			base(id, itemType, weight, itemLocation)
		{
			this.healValue = healValue;
		}

		public IView View
		{
			get { return view; }
			set { view = (HealView) value; }
		}

		public override void Show(Room room)
		{
			view = GameObject.Instantiate
			(
				PrefabsManager.LoadPrefab(PrefabsManager.PrefabsList.DefaultHeal)
			).GetComponent<HealView>();
			
			view.ShowView(healValue, room.WorldPosition);
		}

		public override void PlayerEnterTheRoom(Room room)
		{
			GameManager.Instance.Player.Heal(healValue);
			room.RemoveItemFromRoom();
			view.Destroy();
		}

		public override object Create(params int[] stats)
		{
			HealItem item = new HealItem(stats[0], Id, ItemType, Weight, ItemLocation);
			return item;
		}
	}
}
