using ItemsSystem;
using Managers;
using UnityEngine;
using View.Buffs;
using View.Enemy;

namespace Environment.Buffs
{
	public class Armor : AbstractItem
	{
		private int countArmor;
		private ArmorView view;
			
		public Armor(int countArmor, int id, ItemType itemType, int weight, RoomType itemLocation) : 
			base(id, itemType, weight, itemLocation)
		{	
			this.countArmor = countArmor;
		}

		public IView View
		{
			get { return view; }
			set { view = (ArmorView) value; }
		}

		public override void Show(Room room)
		{	
			view = GameObject.Instantiate
			(
				PrefabsManager.LoadPrefab(PrefabsManager.PrefabsList.DefaultArmor)
			).GetComponent<ArmorView>();
			
			view.ShowView(countArmor, room.WorldPosition);
		}

		public override void PlayerEnterTheRoom(Room room)
		{
			GameManager.Instance.Player.CountArmor += countArmor;
			room.RemoveItemFromRoom();
			if(view!=null)
			view.Destroy();
		}

		public override object Create(params int[] stats)
		{
			var armor = new Armor(stats[0], Id, ItemType, Weight, ItemLocation);
			return armor;
		}
	}
}
