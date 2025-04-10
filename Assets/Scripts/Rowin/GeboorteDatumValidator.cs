using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeeftijdValidator : MonoBehaviour
{
    public TMP_InputField leeftijdInput; // UI Input Field
    public TMP_Text foutmelding; // Optioneel: een UI-element om fouten te tonen

    public void ValideerLeeftijd()
    {
        if (int.TryParse(leeftijdInput.text, out int leeftijd))
        {
            // Controleer of de leeftijd binnen de geldige grenzen valt
            if (leeftijd < 4 || leeftijd > 12)
            {
                foutmelding.text = "Leeftijd moet tussen 4 en 12 jaar zijn!";
                Debug.LogError("Leeftijd moet tussen 4 en 12 jaar zijn!");
                return;
            }

            // Leeftijd is binnen de limieten
            foutmelding.text = ""; // Verwijder foutmelding als alles goed is
            Debug.Log($"Geldige leeftijd: {leeftijd}");
        }
        else
        {
            foutmelding.text = "Ongeldige leeftijd! Voer een geldig getal in.";
            Debug.LogError("Ongeldige leeftijd! Voer een geldig getal in.");
        }
    }
}

