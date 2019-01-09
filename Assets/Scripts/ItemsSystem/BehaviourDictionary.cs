using System.Collections.Generic;
using ActiveAbility;
using Environment;
using Environment.Buffs;
using Environment.Enemy;
using GameStateInfo;
using UnityEngine;

namespace ItemsSystem
{
	public class BehaviourDictionary
	{
		public Dictionary<int, AbstractItem> Dictionary;
		private int maxWeight=0;

		public BehaviourDictionary()
		{
			Dictionary = new Dictionary<int, AbstractItem>();
			InitialDictinary();
		}
		
		private void InitialDictinary()
		{
			Dictionary.Add(-1,  new EmptyItem(-1, ItemType.Empty, 20, RoomType.Default));
			Dictionary.Add(0, new HealItem(GetStats(), 0, ItemType.HealItem, 5, RoomType.Default));
			Dictionary.Add(1, new Armor(GetStats(), 1, ItemType.ArmorItem, 7, RoomType.Default));
			Dictionary.Add(2, new DestroyEnemyAbility(5, 0, 2, ItemType.AbilityItem, 0, RoomType.Default));
			Dictionary.Add(3, new CommonEnemy(GetStats(), 3, ItemType.Enemy, 40, RoomType.Default));

			foreach (var item in Dictionary)
			{
				maxWeight += item.Value.Weight;
			}
		}
		
		private int GetStats()
		{
			int min = GameState.FloorCount;
			if (min < 1)
				min = 1;
			int max = GameState.FloorCount * 2;
            
			return UnityEngine.Random.Range(min, max);
		}

		private AbstractItem GetDeadEndItem()
		{
			return (AbstractItem) Dictionary[1].Create(GetStats());
		}
		
		public AbstractItem GetRandomItem(RoomType roomType)
		{
			int roll = UnityEngine.Random.Range(0, maxWeight);
			int weightSum = 0;

			if (roomType == RoomType.DeadEnd)
			{
				return  GetDeadEndItem();
			}
			
			foreach (var item in Dictionary)
			{	
				weightSum += item.Value.Weight;
				
				if (roll <= weightSum)
				{
					if (item.Key == -1)
					{
						return null;
					}

					if(roomType == item.Value.ItemLocation)
						return (AbstractItem) item.Value.Create(GetStats());
				}
			}
			
			Debug.Log("нихуя " + weightSum + " " + maxWeight + " ");
			return null;
		}
	}
}