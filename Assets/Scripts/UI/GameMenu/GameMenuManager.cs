using GameStateInfo;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.GameMenu
{
	public class GameMenuManager : MonoBehaviour
	{
		public GameObject LosePanel;
		public Button RetryButton;
		public Button ExitButton;
		public Text CountText;
		
		void Start () 
		{
			LosePanel.SetActive(false);
			RetryButton.onClick.AddListener(RetryClick);
			ExitButton.onClick.AddListener(ExitClick);
			GameManager.Instance.PlayerDead += ShowLosePanel;
		}

		private void RetryClick()
		{
			GameManager.Instance.ReloadGame();
		}
		
		private void ExitClick()
		{
			SceneManager.LoadScene(0);
		}

		private void ShowLosePanel()
		{
			CountText.text += GameState.FloorCount;
			LosePanel.SetActive(true);
		}
	
	}
}
