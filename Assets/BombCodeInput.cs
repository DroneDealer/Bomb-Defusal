using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BombCodeInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public BombCode bombCode;
    public TMP_Text feedbackText;
    public GameObject MatchCount;
    public Transform PINFeedbackScroll;
     public TMP_Text livesText;
    public Button submitButton;
    private string correctCode;
    private int entryCount = 0;
    private void Start()
    {
        correctCode = bombCode.GetCode();
        feedbackText.gameObject.SetActive(false);
    }
    public void SubmitGuess()
    {
        string bombCodeInput = inputField.text;
        if (bombCodeInput.Length == 4 && int.TryParse(bombCodeInput, out _))
        {
            Debug.Log("Checking input: " + bombCodeInput + " vs code: " + correctCode);
            MatchCount.gameObject.SetActive(true);
            if (bombCodeInput == correctCode)
            {
                foreach (Transform child in PINFeedbackScroll)
                {
                    if (child.gameObject.CompareTag("Guesses"))
                    {
                        Destroy(child.gameObject);
                    }
                }
                feedbackText.gameObject.SetActive(true);
                feedbackText.text = "BOMB DEFUSED";
            }
            else
            {
                GenerateFeedback(correctCode, bombCodeInput, out int exactMatch, out int numberMatch);
                GameObject guessEntry = Instantiate(MatchCount, PINFeedbackScroll, false);
                entryCount++;
                TMP_Text[] texts = guessEntry.GetComponentsInChildren<TMP_Text>();
                foreach (TMP_Text text in texts)
                {
                    if (text.name == "GuessText")
                    {
                        text.text = "Guess: " + bombCodeInput;
                    }
                    else if (text.name == "GreenNumber")
                    {
                        text.text = ": " + exactMatch.ToString();
                    }
                    else if (text.name == "YellowNumber")
                    {
                        text.text = ": " + numberMatch.ToString();
                    }
                }
                inputField.text = string.Empty;
                bool gameOver = LivesManager.Instance.LoseLife();
                livesText.text = "Attempts left: " + LivesManager.Instance.CurrentLives;
                if (gameOver)
                {
                    inputField.interactable = false;
                    submitButton.interactable = false;
                    foreach (Transform child in PINFeedbackScroll)
                    {
                        if (child.CompareTag("Guesses"))
                        {
                            Destroy(child.gameObject);
                        }
                    }
                    feedbackText.gameObject.SetActive(true);
                    feedbackText.text = "ERROR: TOO MANY FAILED ATTEMPTS - BOMB DETONATED";
                    Debug.Log("Game Over! You ran out of lives.");
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Invalid Input! Remember, it must be a 4-digit number.");
            feedbackText.text = "ERROR: INVALID INPUT. ONLY ACCEPTS 4 DIGIT NUMBERS.";
            inputField.text = string.Empty;
        }
    }
    private string GenerateFeedback(string bombCode, string userInput, out int exactMatch, out int numberMatch)
    {
        exactMatch = 0;
        numberMatch = 0;
        bool[] bombCodeMatch = new bool[4];
        bool[] guessMatch = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            if (userInput[i] == bombCode[i])
            {
                exactMatch++;
                bombCodeMatch[i] = true;
                guessMatch[i] = true;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (guessMatch[i]) continue;
            for (int j = 0; j < 4; j++)
            {
                if (bombCodeMatch[j]) continue;
                if (userInput[i] == bombCode[j])
                {
                    numberMatch++;
                    bombCodeMatch[j] = true;
                    guessMatch[i] = true;
                    break;
                }
            }
        }
        return $"Right number & spot: {exactMatch}\nRight number, wrong spot: {numberMatch}";
    }
}