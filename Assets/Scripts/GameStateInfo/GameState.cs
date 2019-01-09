using ActiveAbility;
using Environment;
using ItemsSystem;
using Managers;

namespace GameStateInfo
{
	public static class GameState 
	{
		public static int FloorCount = 1;

		public static int StartHP = 10;
		
		public static int StartArmor = 0;
		
		public static int StartAbilityCharge = 0;
		
		public static int CurrentHp = StartHP;
		
		public static int CurrentArmor = StartArmor;
		
		public static IUseAbility CurrentAbilityAbility 
			= new DestroyEnemyAbility(5, 0, 3, ItemType.AbilityItem, 10, RoomType.Default);
		
		public static int GetCountNumbers()
		{
			return FloorCount !=0 ? 10 + FloorCount * 10 : 25;
		}

		public static int GetSeed()
		{
			return UnityEngine.Random.Range(0, short.MaxValue);
		}

		public static void UpdateState()
		{
			FloorCount++;
			CurrentHp = GameManager.Instance.Player.CurrentHp;
			CurrentArmor = GameManager.Instance.Player.CountArmor;
			CurrentAbilityAbility = GameManager.Instance.Player.Ability;
		}
	}
}
