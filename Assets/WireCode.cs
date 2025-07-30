using UnityEngine;
using TMPro;

public class WireCode : MonoBehaviour
{
    [SerializeField] private RectTransform wireBounds;
    [SerializeField] private RectTransform pulse;
    [SerializeField] private RectTransform redZone;
    [SerializeField] private RectTransform yellowZone;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float pulseSpeed = 300f;
    private bool movingRight = true;
    private bool isActive = false;
    private enum ZoneColor { Red, Yellow, Green }
    private ZoneColor currentTarget;
    [Header("Audio")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip wireSnip;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip fail;
    void Start()
    {
        NewTargetZone();
    }
    void Update()
    {
        if (isActive)
        {
            MovePulse();
        }
    }
    public void OnCutWireButtonPressed()
    {
        if (!isActive)
        {
            return;
        }
        CheckPulse();
    }
    void MovePulse()
    {
        float direction = movingRight ? 1f : -1f;
        pulse.anchoredPosition += new Vector2(direction * pulseSpeed * Time.deltaTime, 0f);
        float leftBound = wireBounds.rect.xMin;
        float rightBound = wireBounds.rect.xMax;
        if (pulse.anchoredPosition.x >= rightBound)
        {
            movingRight = false;
        }
        else if (pulse.anchoredPosition.x <= leftBound)
        {
            movingRight = true;
        }
    }
    void CheckPulse()
    {
        audioSource.PlayOneShot(wireSnip);
        float pulseX = pulse.anchoredPosition.x;
        RectTransform targetZone = GetCurrentTargetZone();
        float targetMin = targetZone.anchoredPosition.x - (targetZone.rect.width / 2f);
        float targetMax = targetZone.anchoredPosition.x + (targetZone.rect.width / 2f);
        if (pulseX >= targetMin && pulseX <= targetMax)
        {
            audioSource.PlayOneShot(success);
            isActive = false;
            Debug.Log("Wire cut successfully!");
        }
        else
        {
            audioSource.PlayOneShot(fail);
            isActive = false;
            Debug.Log("Wire cut failed! Try again.");
        }
    }
    void NewTargetZone()
    {
        RectTransform targetZone = GetCurrentTargetZone();
        float minX = wireBounds.rect.xMin + (targetZone.rect.width / 2f);
        float maxX = wireBounds.rect.xMax - (targetZone.rect.width / 2f);
        float newx = Random.Range(minX, maxX);
        Vector2 newPosition = targetZone.anchoredPosition;
        newPosition.x = newx;
        targetZone.anchoredPosition = newPosition;
        int colorTarget = Random.Range(0, 3);
        currentTarget = (ZoneColor)colorTarget;
        switch (currentTarget)
        {
            case ZoneColor.Red:
                feedbackText.text = "CUT AT RED ZONE";
                break;
            case ZoneColor.Yellow:
                feedbackText.text = "CUT AT YELLOW ZONE";
                break;
            case ZoneColor.Green:
                feedbackText.text = "CUT AT GREEN ZONE";
                break;
        }
        isActive = true;
    }
    RectTransform GetCurrentTargetZone()
    {
        switch (currentTarget)
        {
            case ZoneColor.Red:
                return redZone;
            case ZoneColor.Yellow:
                return yellowZone;
            case ZoneColor.Green:
                return greenZone;
            default:
                return null; // Should never happen. please don't happen.
        }
    }
}