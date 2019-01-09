using System;
using Environment;
using GameStateInfo;
using PlayerBehaviour;
using UI.HUD;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		public CameraBehaviour.CameraBehaviour CameraBehaviour;
		[NonSerialized] public Player Player;
		public static GameManager Instance;
		public HudController Hud;
		public Action PlayerDead;
		
		private void Awake()
		{
			if (Instance == null) 
			{ 
				Instance = this; 
			} 
			else if(Instance == this)
			{
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			LoadGame();
		}

		private void LoadGame()
		{
			WorldGrid.Instance.GenearateRooms(GameState.GetSeed(), GameState.GetCountNumbers(), () => StartGame());
		}
		
		private void StartGame()
		{
			CameraBehaviour.enabled = true;
			CameraBehaviour.Player = Instantiate
			(
				PrefabsManager.LoadPrefab(PrefabsManager.PrefabsList.Player),
				WorldGrid.Instance.StartRoom.WorldPosition, Quaternion.identity
			);
			Player = CameraBehaviour.Player.GetComponent<Player>();
			Hud.Init();
		}
		
		public void ExitFromDungeon()
		{
			GameState.UpdateState();
			SceneManager.LoadScene(1);
		}

		public void ReloadGame()
		{
			GameState.FloorCount = 1;
			
			SceneManager.LoadScene(1);
		}
	}
}
