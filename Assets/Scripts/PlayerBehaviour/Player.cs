using System;
using ActiveAbility;
using GameStateInfo;
using Managers;
using PlayerBehaviour.Movement;
using UnityEngine;

namespace PlayerBehaviour
{
	public class Player : MonoBehaviour
	{
		private int currentHp;
		private int countArmor;
		public Action HpChanged;
		public Action ArmorChanged;
		public Action UpdateAbility;
		private IUseAbility ability;
		private bool canMove = true;
		
		public IUseAbility Ability
		{
			get { return ability; }
		}

		public int CurrentHp
		{
			get
			{
				return currentHp;
			}
			set
			{
				currentHp = value > MaxHp ? MaxHp : value;
				if(HpChanged!=null)
					HpChanged();
			}
		}

		public int MaxHp;

		public int CountArmor
		 {
			get { return countArmor; }
			set
			{
				countArmor = value;
				if(ArmorChanged!=null)
					ArmorChanged();
			} 
		}

		public float Speed;
		[NonSerialized] public PlayerMovement Movement;
		
		private void Awake()
		{
			Movement = GetComponent<PlayerMovement>();
		}

		private void Start()
		{
			InitPlayer();
		}
		
#if UNITY_EDITOR
		private void Update()
		{
			if (ability != null && Input.GetKeyDown(KeyCode.Space))
			{
				ability.UseAbility();
			}
		}
#endif
		private void InitPlayer()
		{
			Movement.Speed = Speed;
			
			if (GameState.FloorCount == 1)
			{
				CurrentHp = GameState.StartHP;
				CountArmor = GameState.StartArmor;
			}
			else
			{
				CurrentHp = GameState.CurrentHp;
				CountArmor = GameState.CurrentArmor;
			}
			
			ability = GameState.CurrentAbilityAbility;
			
			ability.PickUpAbility();
		}

		public void UseAbility()
		{
			if(canMove)
				ability.UseAbility();
		}

		public void TakeDamage(int damage)
		{
			if (countArmor - damage < 0)
			{
				CurrentHp -= (damage - countArmor);
				CountArmor = 0;
			}
			else
			{
				CountArmor -= damage;
			}

			if (currentHp <= 0)
			{
				Death();
			}
		}

		public void Heal(int value)
		{
			CurrentHp += value;
		}

		private void Death()
		{
			Destroy(this.gameObject);
			if(GameManager.Instance.PlayerDead!=null)
				GameManager.Instance.PlayerDead();
		}
		
		public void PickUpAbility(IUseAbility useAbility)
		{
			if (useAbility != ability)
			{
				if (ability != null)
					ability.DropAbility(Movement.CurrentPosition);

				ability = useAbility;
			}

			if(UpdateAbility!=null)
				UpdateAbility();
		}

		public void SetMovementActive(bool value)
		{
			canMove = value;
			Movement.MovementActive = value;
		}
	}
}
