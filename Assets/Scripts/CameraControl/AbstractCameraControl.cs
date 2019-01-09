using UnityEngine;

// TODO: Задокументировать
public abstract class AbstractCameraControl : MonoBehaviour {

	public ControllableCamera ActiveCamera;

	public float ZoomSpeed = 1f;

	public bool ZoomEnabled = true;
	public bool DragEnabled = true;
//	public bool ClickEnabled = false;//	NOT IMPLENTED!
	public bool DoubleClickEnabled = false;

    public bool AutoAttachment = true;

	protected bool dragStarted = false;
	protected bool zoomStarted = false;

	protected bool isInteractionStatic = true;

	protected Vector2 lastZoomCenter = Vector2.zero;

	// Подписка на события ввода и старт/остановка контроллера
	protected virtual void Awake()
	{
        if (AutoAttachment)
        {
            this.Attach();
        }
	}

	protected bool attached = false;
	public virtual void Attach ()
    {
		Detach (); // На всякий случай отписываемся
		attached = true;
	}
	public virtual void Detach ()
    {
		attached = false;
	}

	// Подписка для внешних слушателей
	public delegate void OnClickTap(Vector2 clickPosition);
	public virtual event OnClickTap onClickTap = delegate {};

	protected void RaiseClickTap(Vector2 position)
	{
		onClickTap(position);
	}

	public delegate void OnDoubleClickTap(GameObject go);
    public virtual event OnDoubleClickTap onDoubleClickTap = delegate {};


	protected void RaiseDoubleClickTap(GameObject go) {
		onDoubleClickTap (go);
	}

    protected virtual void OnDestroy()
    {
        Detach();
    }
}
