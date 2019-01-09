using UnityEngine;

// TODO Задокументировать
public abstract class AbstractCameraControlMouse : AbstractCameraControl {

	protected Vector2 lastClickPosition = Vector2.zero;
	protected Vector2 initialClick = Vector2.zero;
	protected float lastClickDeltaTime;

	protected virtual void onMouseBtnDown () {
		initialClick = lastClickPosition = Input.mousePosition;
		isInteractionStatic = true;
	}

	protected virtual void onMouseBtnUp () {
		lastClickPosition = Vector2.zero;
		dragStarted = false;
	}

	protected virtual void onMouseBtnHold () {
		lastClickPosition = Input.mousePosition;
	}

}
