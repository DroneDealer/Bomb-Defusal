using UnityEngine;
using TMPro;

public class BombCodeInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public BombCode bombCode;
    public TMP_Text feedbackText;
    private string correctCode;
    public GameObject MatchCount;
    public Transform PINFeedbackScroll;
    private int entryCount = 0;
    private void Start()
    {
        correctCode = bombCode.GetCode();
        // feedbackText.text = "Waiting for your guess...";
        feedbackText.gameObject.SetActive(false);
    }
    public void SubmitGuess()
    {
        string bombCodeInput = inputField.text;
        // feedbackText.text = "You clicked the button!";
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
                // Add "number of guesses: xxx"
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
                // feedbackText.text = $"Incorrect Code!\n{feedback}";
                inputField.text = string.Empty;
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
        exactMatch = 0; // correct place, correct digit
        numberMatch = 0; // wrong place, correct digit
        bool[] bombCodeMatch = new bool[4]; // Digits matched in bomb code
        bool[] guessMatch = new bool[4]; // Digits matched in user input
                                         // Need both to keep track of digit duplicates

        // Check for exact matches first
        for (int i = 0; i < 4; i++)
        {
            if (userInput[i] == bombCode[i])
            {
                exactMatch++;
                bombCodeMatch[i] = true; // Matched in bomb code
                guessMatch[i] = true; // Matched in user input
            }
        }
        // Check for number matches (wrong place, correct digit) next
        for (int i = 0; i < 4; i++)
        {
            if (guessMatch[i]) continue; // Skip already matched digits
            for (int j = 0; j < 4; j++)
            {
                if (bombCodeMatch[j])
                {
                    continue; // Skip exactly matched digits in bomb code
                }
                if (userInput[i] == bombCode[j])
                {
                    numberMatch++;
                    bombCodeMatch[j] = true; // Matched in bomb code
                    guessMatch[i] = true; // Matched in user input
                    break; // Stop searching for this digit
                }
            }
        }
        return $"Right number & spot: {exactMatch}\nRight number, wrong spot: {numberMatch}";
        // Coding for dupe digits is so hard bro
    }
}