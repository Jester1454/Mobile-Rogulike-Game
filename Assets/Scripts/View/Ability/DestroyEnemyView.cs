using UnityEngine;
using View.Enemy;

namespace View.Ability
{
	public class DestroyEnemyView : MonoBehaviour, IView
	{
		public SpriteRenderer SpriteAbility;

		public void ShowView(Vector2 worldPosition)
		{
			transform.position = worldPosition;
		}

		public void Destroy()
		{
			Destroy(this.gameObject);
		}
	}
}
