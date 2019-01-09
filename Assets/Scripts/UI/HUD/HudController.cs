using GameStateInfo;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
	public class HudController : MonoBehaviour
	{
		public Text CountHp;
		public Image HpImage;
		public Text CountArmor;
		public Button AbilityUse;
		public Button MapOpen;
		public Text MaxCoolDownText;
		public Text CurrentChargeText;
		public Image AbilityImage;
		
		public void Init()
		{
			CountArmor.text = GameManager.Instance.Player.CountArmor.ToString();
			CountHp.text = GameManager.Instance.Player.CurrentHp.ToString();

			GameManager.Instance.Player.HpChanged += HpChanged;
			GameManager.Instance.Player.ArmorChanged += ArmorChanged;

			GameManager.Instance.Player.UpdateAbility += UpdateAbility;
		}

		private void UpdateAbility()
		{
			MaxCoolDownText.text = GameManager.Instance.Player.Ability.Cooldown.ToString();
			AbilityUse.onClick.AddListener(OnUseAbilityClick);
			MapOpen.onClick.AddListener(OnOpenMapClick);
			GameManager.Instance.Player.Ability.UpdateCharge += OnChargeChanger;
			OnChargeChanger(GameManager.Instance.Player.Ability.CurrentCharge);
		}

		private void OnUseAbilityClick()
		{
			GameManager.Instance.Player.UseAbility();
		}

		private void OnOpenMapClick()
		{
			GameManager.Instance.CameraBehaviour.MapButtonClick();
		}

		private void OnChargeChanger(int count)
		{
			CurrentChargeText.text = count.ToString();
			AbilityImage.fillAmount = (float) count/GameManager.Instance.Player.Ability.Cooldown;
		}

		private void HpChanged()
		{
			CountHp.text = GameManager.Instance.Player.CurrentHp.ToString();
			HpImage.fillAmount =(float) GameManager.Instance.Player.CurrentHp/GameManager.Instance.Player.MaxHp ;
		}
		
		private void ArmorChanged()
		{
			CountArmor.text = GameManager.Instance.Player.CountArmor.ToString();
		}

		private void OnDisable()
		{
			GameManager.Instance.Player.HpChanged -= HpChanged;
			GameManager.Instance.Player.ArmorChanged -= ArmorChanged;
			GameManager.Instance.Player.UpdateAbility -= UpdateAbility;
			GameManager.Instance.Player.Ability.UpdateCharge -= OnChargeChanger;
		}
	}
}
