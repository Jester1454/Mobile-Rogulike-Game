using System;
using System.Collections;
using Environment;
using Managers;
using UnityEngine;

namespace CameraBehaviour
{
	public enum CameraState
	{
		MovementForPlayer,
		MovementMap
	}
	
	public class CameraBehaviour : MonoBehaviour
	{
		private Camera camera;
		private ControllableCamera controllableCamera;
		private CameraControlTouch cameraControlTouch;
#if UNITY_EDITOR
		private CameraControlMouse cameraControlMouse;

#endif

		private AbstractCameraControl CameraControl
		{
			get
			{
#if UNITY_EDITOR
				return cameraControlMouse;
#endif
				return cameraControlTouch;
			}
		}
		
		private float ortographicSize;
		private CameraState state;
		[NonSerialized]public GameObject Player;
		public int Speed = 8;
		private Vector3 offset = Vector3.zero;
		private Vector3 newPosition = Vector3.one;
		private Transform mTransform;
		private float constZ;

		public Vector2 BoundsMin;
		public Vector2 BoundsMax;
		private Vector2 mapSize;
		private Vector2 startPosition;
		
		private void Awake()
		{
			camera = GetComponent<Camera>();
			controllableCamera = GetComponent<ControllableCamera>();
#if UNITY_EDITOR
			cameraControlMouse = GetComponent<CameraControlMouse>();
#endif
			cameraControlTouch = GetComponent<CameraControlTouch>();	
			CameraControl.Detach();
		}

		void Start ()
		{
			float vertExtent = camera.orthographicSize;    
			float horzExtent = vertExtent * Screen.width / Screen.height;
			
			mapSize = WorldGrid.Instance.SizeWorld;
			
			startPosition = WorldGrid.Instance.StartRoom.WorldPosition;
			
			BoundsMin.x = horzExtent - mapSize.x / 2.0f + startPosition.x;
			BoundsMax.x = mapSize.x / 2.0f - horzExtent + startPosition.x;
			BoundsMin.y = vertExtent - mapSize.y / 2.0f + startPosition.y;
			BoundsMax.y = mapSize.y / 2.0f - vertExtent + startPosition.y;
			
			ortographicSize = camera.orthographicSize;
			controllableCamera.MinSize = ortographicSize;
			
			state = CameraState.MovementForPlayer;
			mTransform = transform;
			Vector3 newCameraPos = Player.transform.position;
			newCameraPos.z = mTransform.position.z;
			mTransform.position = newCameraPos;
			
			constZ = mTransform.position.z;
			CameraControl.ZoomEnabled = false;
			MapButtonClick();
			CloseMap();
		}
    
		void LateUpdate () 
		{
			if (Player != null && state == CameraState.MovementForPlayer)
			{
				newPosition = Player.transform.position + offset;
				newPosition.z = constZ;
				
				newPosition.x = Mathf.Clamp(newPosition.x, BoundsMin.x, BoundsMax.x);
				newPosition.y = Mathf.Clamp(newPosition.y, BoundsMin.y, BoundsMax.y);
				
				if(mTransform.position!= newPosition)
					MovementTo(newPosition);
			}
		}
		
		private void MovementTo(Vector3 position)
		{
			mTransform.position = Vector3.Lerp(mTransform.position, position, Speed * Time.deltaTime);
		}

		public void Click(Vector2 pos)
		{
			Room room = WorldGrid.Instance.GetRoom(pos);

			if (room != null && room.IsFullVisible)
			{
				GameManager.Instance.Player.Movement.TeleportTo(room);
				CloseMap();
			}
		}
		
		private void OpenMap()
		{			
			CameraControl.Attach();
			state = CameraState.MovementMap;
			GameManager.Instance.Player.SetMovementActive(false);	
			
			camera.orthographicSize = mapSize.x/2;
			
			CameraControl.ZoomEnabled = true;
			Debug.LogError(mapSize);
			controllableCamera.TranslationBounds = mapSize/2;
			
			controllableCamera.Transform.position = new Vector3(startPosition.x,
				startPosition.y, constZ);

			controllableCamera.DefaultPosition = controllableCamera.Transform.position;

			controllableCamera.MaxSize = camera.orthographicSize;
			CameraControl.onClickTap += Click;
		}
		
		public void MapButtonClick()
		{
			if (state == CameraState.MovementMap)
			{
				CloseMap();
			}
			else
			{
				OpenMap();
			}
		}
		
		private void CloseMap()
		{
			state = CameraState.MovementForPlayer;
			CameraControl.onClickTap -= Click;
			CameraControl.ZoomEnabled = false;
			camera.orthographicSize = ortographicSize;
			CameraControl.Detach();
			GameManager.Instance.Player.SetMovementActive(true);
		}
	}
}