using UnityEngine;
using TMPro;

public class TMPDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown tmpDropdown; // Reference to the TMP Dropdown component

    void Awake()
    {
        // Auto-assign the TMP_Dropdown component if not manually set
        if (tmpDropdown == null)
        {
            tmpDropdown = GetComponent<TMP_Dropdown>();
        }

        // Add listener to handle value change
        tmpDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        string selectedOption = tmpDropdown.options[index].text;
        Debug.Log(selectedOption + " has been selected");
    }
}

