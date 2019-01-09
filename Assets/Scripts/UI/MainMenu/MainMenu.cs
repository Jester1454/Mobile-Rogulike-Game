using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenu
{
	public class MainMenu : MonoBehaviour
	{
		public Button PlayButton;

		private void Awake()
		{
			PlayButton.onClick.AddListener(OnStartClick);
		}

		private void OnStartClick()
		{
			SceneManager.LoadScene(1);
		}
	}
}
