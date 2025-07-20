using UnityEngine;
using TMPro;

public class BombCodeInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public BombCode bombCode;
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
                Debug.Log("Correct Code!");
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
}
