using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Transform _originalParent;
    private CanvasGroup _canvasGroup;
    [SerializeField]private Canvas canvas;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (gameObject.GetComponent<Clue>().isClueCard)
        {
            canvas = GetComponentInParent<Canvas>();
        }
        //_canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Clue>().isClueCard)
        {
            // Store the original position and parent of the card
            Debug.Log("Pointer down on card: " + gameObject.name);
            _canvasGroup = GetComponent<CanvasGroup>();
        } 
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
           //Debug.Log("This is " + gameObject.name);
        
        //_startPosition = transform.position;
        //_originalParent = transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Clue>().isClueCard)
        {
            // Make the card semi-transparent and allow it to be dragged
            _canvasGroup.alpha = 0.6f;
            _canvasGroup.blocksRaycasts = false;
            //transform.SetParent(transform.root); // Move to root to avoid being clipped by other UI elements
            Debug.Log("Begin dragging card: " + gameObject.name);
        } else
        {

        }

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Clue>().isClueCard)
        {
            // Move the card with the mouse
            Debug.Log("Dragging card: " + gameObject.name);
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;// transform.root.GetComponent<Canvas>().scaleFactor;
                                                                                   //transform.position = Input.mousePosition;
        } else
        {
            Debug.Log("Rotating: " + gameObject.name);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Clue>().isClueCard)
        {
            // Reset the card's position and parent, and make it fully opaque again
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            //transform.SetParent(_originalParent);
            //transform.position = _startPosition;
            Debug.Log("End dragging card: " + gameObject.name);
        }
    }
// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    }
