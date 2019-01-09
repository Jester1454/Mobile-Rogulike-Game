using UnityEngine;

public class ControllableCamera : MonoBehaviour {

	public LayerMask RaycastLayers;

	private Camera selfCamera;
	public Camera Camera {
		get { return selfCamera; }
	}

	private Transform selfTransform;
	public Transform Transform {
		get { return selfTransform; }
	}

	// Шорткат для текущего размера камеры
	public float Size {
		get { return selfCamera.orthographicSize; }
		set { selfCamera.orthographicSize = value; }
	}

	private Vector3 defaultPosition = Vector3.zero;
	public Vector3 DefaultPosition {
		get { return defaultPosition; }
		set { defaultPosition = value; }
	}
	private float defaultSize = 1f;
	public float DefaultSize {
		get { return defaultSize; }
		set { defaultSize = value; }
	}

	public float MinSize = 0.5f;
	public float MaxSize = 10f;

	public bool TranslationRestrained = true;
	public Vector2 TranslationBounds;

	private Vector2 lastZoomCenterScreen = Vector2.zero;
	private Vector3 lastZoomCenterWorld = Vector2.zero;

	// Events
	public delegate void OnChange();
	public event OnChange onChange = delegate {};
	public event OnChange onTranslate = delegate {};
	public event OnChange onZoom = delegate {};
	private bool queuedTranslate = false;
	private bool queuedZoom = false;

	void Awake () {
		selfCamera = GetComponent<Camera> ();
		selfTransform = GetComponent<Transform> ();
		TranslationBounds = new Vector2 (MaxSize - MinSize/2f, MaxSize - MinSize/2f);

		defaultSize = Size;
		defaultPosition = selfTransform.position;
	}

	void LateUpdate () {
		// Оповещение внешних подписчиков событий
		if (queuedZoom) {
			onZoom ();
		}
		if (queuedTranslate) {
			onTranslate();
		}
		if (queuedTranslate || queuedZoom) {
			queuedZoom = false;
			queuedTranslate = false;
			onChange();
		}
	}

	public void Translate (Vector3 direction) {
		Vector3 targetPosition;
		if (TranslationRestrained) {
			targetPosition = direction + selfTransform.position;

			int signX = 1 * (int)Mathf.Sign(targetPosition.x);
			int signY =  1 * (int)Mathf.Sign(targetPosition.y);
						
			Vector2 defpos = new Vector2( TranslationBounds.x * signX,
				TranslationBounds.y * signY);
						
			targetPosition = new Vector3 
		 	(
				Mathf.Min (Mathf.Abs (targetPosition.x), defaultPosition.x + defpos.x) * signX,
				Mathf.Min (Mathf.Abs (targetPosition.y), defaultPosition.y + defpos.y) * signY,
				targetPosition.z
			) - selfTransform.position;
			
		} else {
			targetPosition = direction;
		}
		selfTransform.Translate(targetPosition);
		queuedTranslate = true;
	}

	public void TranslateScreen (Vector3 from, Vector3 to) {
		Translate (
			selfCamera.ScreenToWorldPoint (from) - selfCamera.ScreenToWorldPoint (to)
		);
	}

	public void Zoom (Vector3 center, float deltaScale) {
		if (selfCamera.orthographic)
		{
			// Лимит на изменение увеличения камеры
			float targetDelta = Mathf.Clamp(
				// Умножение на размер камеры для сохранения постоянного углового увеличения
				deltaScale * Size,
				MinSize - selfCamera.orthographicSize,
				MaxSize - selfCamera.orthographicSize
			);
			Size += targetDelta;

			// Смещение точки увеличения из-за измения размера камеры
			Vector3 deltaSize = new Vector3 (targetDelta*selfCamera.aspect, targetDelta, 0f);

			// Направление смещение центра камеры
			Vector3 centerTranslation = (selfTransform.position - center);
			// Вектор направления не может быть длиннее 1
			float translation = Mathf.Min(centerTranslation.magnitude, 1f);
			centerTranslation = centerTranslation.normalized * translation;

			centerTranslation = new Vector3 (centerTranslation.x * deltaSize.x, centerTranslation.y * deltaSize.y, 0f);

			Translate (centerTranslation);
		}
		queuedZoom = true;
		// TODO: Реализовать зум для 3D-камеры
	}

	public void ZoomScreen (Vector2 center, float deltaScale) {
		if (lastZoomCenterScreen != center) {
			lastZoomCenterScreen = center;
			lastZoomCenterWorld = selfCamera.ScreenToWorldPoint (center);
		}
		Zoom (
			lastZoomCenterWorld,
			deltaScale / selfCamera.ScreenToWorldPoint (Vector3.one).magnitude
		);
	}

	public RaycastHit2D[] Raycast2DWorld (Vector3 worldPosition) {
		return Physics2D.RaycastAll (worldPosition, selfTransform.forward, Mathf.Infinity, RaycastLayers);
	}

	public RaycastHit2D Raycast2DScreen (Vector2 screenPosition) {
		Vector3 worldTouch = selfCamera.ScreenToWorldPoint(screenPosition);
		RaycastHit2D[] hits = Raycast2DWorld (new Vector3 (worldTouch.x, worldTouch.y, 0f));
		return hits.Length > 0 ? hits[0] : default(RaycastHit2D);
	}

	public void Reset () {
		ResetSize ();
		ResetPosition ();
	}

	public void ResetSize () {
		Zoom (defaultPosition, defaultSize / Size);
	}

	public void ResetPosition () {
		Translate (defaultPosition - selfTransform.position);
	}

}
