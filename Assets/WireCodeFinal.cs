using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WireCodeFinal : MonoBehaviour
{
    [SerializeField] private RectTransform wireBounds;
    [SerializeField] private RectTransform pulse;
    [SerializeField] private RectTransform redZone;
    [SerializeField] private RectTransform yellowZone;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private Image redZoneImage;
    [SerializeField] private Image yellowZoneImage;
    [SerializeField] private Image greenZoneImage;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float pulseSpeed = 300f;
    private bool movingRight = true;
    private bool isActive = false;
    private enum ZoneColor { Red, Yellow, Green }
    private ZoneColor currentTarget;
    private int currentRound = 1;
    public int maxRounds = 5;

    [Header("Audio")]
    public AudioSource audioSource;
    [SerializeField] private AudioClip wireSnip;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip fail;
    private readonly Color activeRed = Color.red;
    private readonly Color activeYellow = Color.yellow;
    private readonly Color activeGreen = Color.green;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            return;

        CheckPulse();
    }
    void MovePulse()
    {
        float direction = movingRight ? 1f : -1f;
        pulse.anchoredPosition += new Vector2(direction * pulseSpeed * Time.deltaTime, 0f);

        float leftBound = wireBounds.rect.xMin;
        float rightBound = wireBounds.rect.xMax;

        if (pulse.anchoredPosition.x >= rightBound)
            movingRight = false;
        else if (pulse.anchoredPosition.x <= leftBound)
            movingRight = true;
    }
    void CheckPulse()
    {
        audioSource.PlayOneShot(wireSnip);

        float pulseX = pulse.anchoredPosition.x;

        RectTransform targetZone = currentTarget switch
        {
            ZoneColor.Red => redZone,
            ZoneColor.Yellow => yellowZone,
            ZoneColor.Green => greenZone,
            _ => null
        };

        float targetMin = targetZone.anchoredPosition.x - (targetZone.rect.width / 2f);
        float targetMax = targetZone.anchoredPosition.x + (targetZone.rect.width / 2f);

        if (pulseX >= targetMin && pulseX <= targetMax)
        {
            audioSource.PlayOneShot(success);
            isActive = false;
            Debug.Log("Wire cut successfully!");
            currentRound++;
            if(currentRound > maxRounds)
            {
                feedbackText.text = "ALL WIRES CUT. DISARMING BOMB...";
                feedbackText.color = Color.green;
            }
            else
            {
                feedbackText.text = "ROUND " + currentRound + " COMPLETE. PROGRESSING...";
                NewTargetZone();
            }
        }
        else
        {
            audioSource.PlayOneShot(fail);
            bool gameOver = LivesManager.Instance.LoseLife();
            if (gameOver)
            {
                feedbackText.text = "ERROR: OUT OF ATTEMPTS. BOMB DETONATING...";
                isActive = false;
                return;
            }
            else
            {
                feedbackText.text = "ERROR: WIRE CUT INCORRECTLY";
                Debug.Log("Wire cut failed! Try again.");
                return;
            }
        }
    }
    void NewTargetZone()
    {
        int colorIndex = Random.Range(0, 3);
        currentTarget = (ZoneColor)colorIndex;

        switch (currentTarget)
        {
            case ZoneColor.Red:
                redZoneImage.color = activeRed;
                feedbackText.text = "CUT AT RED ZONE";
                break;
            case ZoneColor.Yellow:
                yellowZoneImage.color = activeYellow;
                feedbackText.text = "CUT AT YELLOW ZONE";
                break;
            case ZoneColor.Green:
                greenZoneImage.color = activeGreen;
                feedbackText.text = "CUT AT GREEN ZONE";
                break;
        }
        isActive = true;
    }
}