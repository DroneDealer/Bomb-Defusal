using UnityEngine;
using TMPro;

public class BombCodeInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public BombCode bombCode;
    public TMP_Text feedbackText;
    private string correctCode;

    private void Start()
    {
        correctCode = bombCode.BombCombo();
    }

    public void SubmitGuess()
    {
        string bombCodeInput = inputField.text;
        if (bombCodeInput.Length == 4 && int.TryParse(bombCodeInput, out _))
        {
            if (bombCodeInput == correctCode)
            {
                Debug.Log("Correct Code! Bomb defused!");
            }
            else
            {
                Debug.Log("Incorrect Code! Try again.");
                inputField.text = string.Empty;
            }
        }
        else
        {
            Debug.Log("Invalid Input! Remember, it must be a 4-digit number.");
            inputField.text = string.Empty;
        }
    }
    private string GenerateFeedback(string bombCode, string userInput)
    {
        int exactMatch = 0; // correct place, correct digit
        int numberMatch = 0; // wrong place, correct digit
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