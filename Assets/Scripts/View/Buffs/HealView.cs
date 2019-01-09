using UnityEngine;
using View.Enemy;

namespace View.Buffs
{
	public class HealView : MonoBehaviour, IView
	{
		public TextMesh Text;
		
		private void Awake()
		{
			Text.GetComponent<MeshRenderer>().sortingLayerName = "Items";
			Text.GetComponent<MeshRenderer>().sortingOrder = 1;
		}

		public void ShowView(int healValue, Vector2 worldPosition)
		{
			transform.position = worldPosition;
			Text.text = healValue.ToString();
		}

		public void Destroy()
		{
			Destroy(this.gameObject);
		}
	}
}
