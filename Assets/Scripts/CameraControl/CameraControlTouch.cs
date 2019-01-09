using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO Задокументировать
// TODO Обработка двойного клика
public class CameraControlTouch : AbstractCameraControlTouch {

	// Минимальная дистанция смещения пальца в пикселях, чтобы считалось дивжение, а не тык
	// TODO: не стоит ли перейти к относительным координатам?
	private byte dragTreshold = 64;

	void Update()
	{
		if (attached) {
			// Обновление точек касания при первом касании
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				onTouchStart ();
			}

			// Одно касание - перетаскивание камеры или поиск столкновений
			if (Input.touchCount == 1) {
				onOneTouch ();
			}

			// Два касания - зум
			if (ZoomEnabled && Input.touchCount == 2)
			{
				onTwinTouch ();
			} else {
				zoomStarted = false;
			}
		}
	}

	void onTouchStart () {
		updateTouchPositions ();
		initialTouch = lastTouchPosition [0];
		isInteractionStatic = true;
	}

	void onOneTouch () {
		Touch touch = Input.GetTouch (0);
		Vector2 touchPosition = touch.position;

		// Длинный жест пальцем - перетаскивание
        if (DragEnabled && touch.phase == TouchPhase.Moved && (touchPosition - initialTouch).sqrMagnitude > dragTreshold) {
			ActiveCamera.TranslateScreen(lastTouchPosition[0], touchPosition);
			lastTouchPosition[0] = touchPosition;
			isInteractionStatic = false;
		}

		// Короткое прикосновение - поиск места тыка
		if (touch.phase == TouchPhase.Ended && isInteractionStatic)
		{
			Ray ray = Camera.main.ScreenPointToRay(touchPosition);    
			RaiseClickTap(ray.origin + ray.direction);
		}
	}

	void onTwinTouch () {
		if (!zoomStarted) {
			zoomStarted = true;
			lastZoomCenter = ActiveCamera.Camera.ScreenToWorldPoint ((Input.GetTouch (0).position + Input.GetTouch (1).position) / 2f);
		}
		if (lastTouchPosition.Length > 1) {
			float deltaScale = (lastTouchPosition [0] - lastTouchPosition [1]).magnitude - (Input.GetTouch (0).position - Input.GetTouch (1).position).magnitude;
			ActiveCamera.Zoom (
				lastZoomCenter, 
				ZoomSpeed * deltaScale / ActiveCamera.Camera.ScreenToWorldPoint (Vector3.one).magnitude
			);
			isInteractionStatic = false;
		}
		updateTouchPositions ();
	}

	void updateTouchPositions () {
		lastTouchPosition = (new List<Touch> (Input.touches)).FindAll (touchInProgress).Select(t => t.position).ToArray ();
	}

	private static bool touchInProgress (Touch touch) {
		return touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled;
	}
}
