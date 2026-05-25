using System.Collections;
using UnityEngine;

public class PanelZoomAnimation : MonoBehaviour
{
    public float animationSpeed = 8f;

    private CanvasGroup canvasGroup;
    private Coroutine currentAnimation;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void OnEnable()
    {
        PlayOpen();
    }

    public void PlayOpen()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true;

        currentAnimation = StartCoroutine(AnimatePanel(Vector3.one, 1f, null));
    }

    public void PlayClose(System.Action onFinished)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        canvasGroup.blocksRaycasts = false;

        currentAnimation = StartCoroutine(AnimatePanel(new Vector3(0.75f, 0.75f, 0.75f), 0f, onFinished));
    }

    private IEnumerator AnimatePanel(Vector3 targetScale, float targetAlpha, System.Action onFinished)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f || Mathf.Abs(canvasGroup.alpha - targetAlpha) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * animationSpeed);

            yield return null;
        }

        transform.localScale = targetScale;
        canvasGroup.alpha = targetAlpha;

        onFinished?.Invoke();
    }
}