using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class SimonSaysDisplayButon : MonoBehaviour
{
    public Image displayImage;
    public float glowEffectDuration = 0.5f;
    public void SetColor(Color color)
    {
        displayImage.color = color;
    }
    public void Flash(Color flashColor)
    {
        StartCoroutine(FlashSequence(flashColor));
    }
    private IEnumerator FlashSequence(Color flashColor)
    {
        Color originalColor = displayImage.color;
        displayImage.color = flashColor;
        yield return new WaitForSeconds(glowEffectDuration);
        displayImage.color = originalColor;
    }
}