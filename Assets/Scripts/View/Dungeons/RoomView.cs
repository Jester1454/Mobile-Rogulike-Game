using Environment;
using UnityEngine;

namespace View.Dungeons
{
	public class RoomView : MonoBehaviour
	{
		public GameObject[] Doors;
		public GameObject[] Walls;
		public GameObject StartRoom;
		public GameObject EndRoom;
		public SpriteRenderer Fog;
		private Color fogColor = Color.black;
		
		public void InitView(Vector2 worldPosition,  Room room = null, bool isVisible = true)
		{
			if (room != null)
			{
				transform.position = worldPosition;

				for (int i = 0; i < room.Edges.Length; i++)
				{
					Doors[i].SetActive(room.Edges[i]);
					Walls[i].SetActive(!room.Edges[i]);
				}
				
				if(room.RoomType == RoomType.Start)
					StartRoom.SetActive(true);
				
				if(room.RoomType == RoomType.End)
					EndRoom.SetActive(true);

				fogColor.a = 1;
				Fog.color = fogColor;
			}
		}

		public void FullShowRoom()
		{
			Fog.gameObject.SetActive(false);
		}

		public void HalfShowRoom()
		{
			fogColor.a = 0.7f;
			Fog.color = fogColor;
		}
	}
}
