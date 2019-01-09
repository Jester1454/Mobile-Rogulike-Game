using UnityEngine;

// TODO Задокументировать
// TODO Обработка двойного клика
public class CameraControlMouse : AbstractCameraControlMouse {

	private byte dragTreshold = 64;

	#if UNITY_EDITOR
	void Update () {
		if (attached) {
			if (Input.GetMouseButtonDown (0)) {
				onMouseBtnDown ();
			}
			if (DragEnabled && Input.GetMouseButton (0)) {
				onMouseHold ();
				onMouseBtnHold ();
			}
			if (Input.GetMouseButtonUp (0)) {
				onMouseBtnUp ();
				onMouseClick ();
			}
            if (ZoomEnabled && !Mathf.Approximately(Input.mouseScrollDelta.y, 0)) {
				onMouseScroll ();
			}
		}
	}
	#endif

	void onMouseHold () {
		Vector2 clickPosition = Input.mousePosition;

		// Длинный жест пальцем - перетаскивание
        if ((clickPosition - initialClick).sqrMagnitude > dragTreshold) {
			ActiveCamera.TranslateScreen(lastClickPosition, clickPosition);
            isInteractionStatic = false;
        }
	}

	
	void onMouseClick () 
	{
		if (isInteractionStatic)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
			RaiseClickTap(ray.origin + ray.direction);    
		}
	}

	void onMouseScroll () {
		ActiveCamera.ZoomScreen (Input.mousePosition, ZoomSpeed*Input.mouseScrollDelta.y);
	}
}
