using System;
using Environment;
using UnityEngine;

namespace ItemsSystem
{
	[Serializable]
	public abstract class AbstractItem : IItemBehaviour
	{
		public int Id
		{
			get { return id; }
		}

		public ItemType ItemType
		{
			get { return itemType; }
		}

		public int Weight
		{
			get { return weight; }
		}
        
		public RoomType ItemLocation
		{
			get { return itemLocation; }
		}

		[SerializeField] private int id;
		[SerializeField] private ItemType itemType;
		[SerializeField] private int weight;
		[SerializeField] private RoomType itemLocation;

		public AbstractItem(int id, ItemType itemType, int weight, RoomType itemLocation)
		{
			this.id = id;
			this.itemType = itemType;
			this.weight = weight;
			this.itemLocation = itemLocation;
		}

		public abstract void Show(Room room);

		public abstract void PlayerEnterTheRoom(Room room);
		public abstract object Create(params int[] stats);
	}
}
