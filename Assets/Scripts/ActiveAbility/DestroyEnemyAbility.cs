using System;
using Environment;
using Environment.Enemy;
using ItemsSystem;
using Managers;
using UnityEngine;
using View.Ability;
using View.Buffs;
using View.Enemy;

namespace ActiveAbility
{
	public class DestroyEnemyAbility : AbstractItem, IUseAbility
	{
		private DestroyEnemyView view;

		public IView View
		{
			get { return view; }
			set { view = (DestroyEnemyView) value; }
		}

		public void UseAbility()
		{
			if (CanUseAbility)
			{
			    AbstractEnemy abstractEnemy =	WorldGrid.Instance.FindEnemy
				(
					GameManager.Instance.Player.Movement.CurrentPosition.GridX,
					GameManager.Instance.Player.Movement.CurrentPosition.GridY
				);

				if (abstractEnemy != null)
				{
					abstractEnemy.Dead();
					CurrentCharge = 0;
					if (UpdateCharge != null)
						UpdateCharge(CurrentCharge);
				}
			}
		}

		public DestroyEnemyAbility(int cooldown, int startCooldown,  int id, ItemType itemType, int weight, RoomType itemLocation) : 
			base(id, itemType, weight, itemLocation)
		{
			Cooldown = cooldown;
			CurrentCharge = startCooldown;
			
			if (UpdateCharge != null)
				UpdateCharge(CurrentCharge);
		}

		public int Cooldown { get; set; }
		
		private int currentCharge;

		public int CurrentCharge 
		{
			get { return currentCharge; }
			set
			{
				if (value <= Cooldown)
					currentCharge = value;
			} 
		}

		public bool CanUseAbility
		{
			get
			{
				if (CurrentCharge ==Cooldown)
				{
					return true;
				}

				return false;
			}
		}

		public Action<int> UpdateCharge { get; set; }

		private void updateCharge()
		{
			CurrentCharge++;
			if (UpdateCharge != null)
				UpdateCharge(CurrentCharge);			
		}
		
		public void PickUpAbility()
		{
			GameManager.Instance.Player.PickUpAbility(this);
			GameManager.Instance.Player.Movement.EnterTheNewRoom += updateCharge;
			Hide();
		}

		public void DropAbility(Room room)
		{
			room.PlaceItemInRoom(this);
			Show(room);
			UpdateCharge = null;
			GameManager.Instance.Player.Movement.EnterTheNewRoom -= updateCharge;
		}

		public override void Show(Room room)
		{
			view = GameObject.Instantiate
			(
				PrefabsManager.LoadPrefab(PrefabsManager.PrefabsList.DestroyEnemyAbility)
			).GetComponent<DestroyEnemyView>();
			
			view.ShowView(room.WorldPosition);
		}

		private void Hide()
		{
			if (view != null)
			{
				view.Destroy();
			}
		}

		public override void PlayerEnterTheRoom(Room room)
		{
			PickUpAbility();
		}

		public override object Create(params int[] stats)
		{
			DestroyEnemyAbility ability = new DestroyEnemyAbility(Cooldown, CurrentCharge, Id, ItemType, Weight, ItemLocation);
			return ability;
		}
	}
}
