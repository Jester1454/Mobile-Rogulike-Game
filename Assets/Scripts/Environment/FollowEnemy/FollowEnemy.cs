using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Environment.FollowEnemy
{
	public class FollowEnemy : MonoBehaviour
	{
		private List<Vector2> PlayerPath = new List<Vector2>();
		private Transform mTransform;
		public float Speed;
		private int currentCountInOPath = 0;
	
	
		void Awake ()
		{
			mTransform = GetComponent<Transform>();
			GameManager.Instance.Player.Movement.EnterTheRoom += NextStep;
		}

		private void Update()
		{
			if (currentCountInOPath > 1)
			{
				MovementTo(PlayerPath[currentCountInOPath-2]);
			}
		}

		private void NextStep(Vector2 worldPosition)
		{
			PlayerPath.Add(worldPosition);
			currentCountInOPath++;
		}
	
		private void MovementTo(Vector2 position)
		{
			mTransform.position = Vector2.Lerp(mTransform.position, position, Speed * Time.deltaTime);
		}
	}
}
