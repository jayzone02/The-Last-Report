using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Image buttonImage;

    public float hoverScale = 1.05f;
    public float speed = 10f;

    public Color normalColor = new Color(1f, 1f, 1f, 0f);
    public Color hoverColor = new Color(1f, 0.75f, 0.1f, 0.25f);

    private Vector3 targetScale;
    private Color targetColor;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        buttonImage = GetComponent<Image>();
        targetColor = normalColor;

        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);

        if (buttonImage != null)
        {
            buttonImage.color = Color.Lerp(buttonImage.color, targetColor, Time.deltaTime * speed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
        targetColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        targetColor = normalColor;
    }
}