using ItemsSystem;

namespace Environment
{
	public class EmptyItem : AbstractItem 
	{

		public EmptyItem(int id, ItemType itemType, int weight, RoomType itemLocation) : base(id, itemType, weight, itemLocation)
		{
		
		}

		public override void Show(Room room)
		{
			throw new System.NotImplementedException();
		}

		public override void PlayerEnterTheRoom(Room room)
		{
			throw new System.NotImplementedException();
		}

		public override object Create(params int[] stats)
		{
			throw new System.NotImplementedException();
		}
	}
}
