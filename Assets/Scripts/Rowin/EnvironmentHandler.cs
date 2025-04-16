using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnvironmentHandler : MonoBehaviour
{
    // Buttons used to create new environments
    public Button environmentCreatorButton1;
    public Button environmentCreatorButton2;
    public Button environmentCreatorButton3;

    // Buttons used to select/enter already created environments
    public Button environmentButton1;
    public Button environmentButton2;
    public Button environmentButton3;

    // Buttons for going back
    public Button goBackToMenuButton1;
    public Button goBackToMenuButton2;
    public Button goBackToMenuButton3;

    // The actual environment GameObjects that will be activated/deactivated
    public GameObject environment1;
    public GameObject environment2;
    public GameObject environment3;

    // Transforms for environment spawns
    public Transform parentEnvironment0;
    public Transform parentEnvironment1;
    public Transform parentEnvironment2;

    // UI scenes - scene2 is the creator/menu view, scene3 is the environment view
    public GameObject scene2;
    public GameObject scene3;

    // Input fields where users type the name for each environment
    public TMP_InputField nameInput1;
    public TMP_InputField nameInput2;
    public TMP_InputField nameInput3;

    // Text components for environment buttons (to update button text)
    public TextMeshProUGUI environmentButtonText1;
    public TextMeshProUGUI environmentButtonText2;
    public TextMeshProUGUI environmentButtonText3;

    // Called when the script starts
    private void Start()
    {
        Initialize(); // Set initial state

        // Add listeners to creator buttons that activate corresponding environment buttons if a name is provided
        environmentCreatorButton1.onClick.AddListener(() => TryActivateButton(nameInput1, environmentButton1, "Creator Button 1"));
        environmentCreatorButton2.onClick.AddListener(() => TryActivateButton(nameInput2, environmentButton2, "Creator Button 2"));
        environmentCreatorButton3.onClick.AddListener(() => TryActivateButton(nameInput3, environmentButton3, "Creator Button 3"));

        // Add listeners to environment selection buttons to activate environments
        environmentButton1.onClick.AddListener(() => ActivateEnvironment(environment1, "Environment 1"));
        environmentButton2.onClick.AddListener(() => ActivateEnvironment(environment2, "Environment 2"));
        environmentButton3.onClick.AddListener(() => ActivateEnvironment(environment3, "Environment 3"));

        // Add listeners to go-back buttons to return to menu
        goBackToMenuButton1.onClick.AddListener(ReturnToMenu);
        goBackToMenuButton2.onClick.AddListener(ReturnToMenu);
        goBackToMenuButton3.onClick.AddListener(ReturnToMenu);

        // Add listeners to change the value of environmentName
        nameInput1.onValueChanged.AddListener(UpdateText);
        nameInput2.onValueChanged.AddListener(UpdateText);
        nameInput3.onValueChanged.AddListener(UpdateText);
    }

    // Initializes the UI by hiding the environment scene and deactivating all environments
    private void Initialize()
    {
        scene3.SetActive(false); // Hide the environment scene at the start
        DeactivateAllEnvironments(); // Ensure all environments are off
    }

    // Deactivates all environment GameObjects
    void DeactivateAllEnvironments()
    {
        environment1.SetActive(false);
        environment2.SetActive(false);
        environment3.SetActive(false);
    }

    // Switches back to the main menu/creator scene
    void ReturnToMenu()
    {
        DeactivateAllEnvironments(); // Turn off any active environments
        scene3.SetActive(false);     // Hide the environment scene
        scene2.SetActive(true);      // Show the creator/menu scene
    }

    // Tries to activate a selection button only if the input field has a non-empty name
    void TryActivateButton(TMP_InputField input, Button button, string label)
    {
        string inputText = input.text.Trim(); // Trim whitespace

        if (!string.IsNullOrEmpty(inputText))
        {
            button.gameObject.SetActive(true); // Show the environment button
            Debug.Log($"{label} clicked - Activated {button.name} with name '{inputText}'");
        }
        else
        {
            Debug.LogWarning($"{label} clicked - Name field is empty, cannot activate {button.name}");
        }
    }

    // Activates a specific environment and switches to the environment scene
    void ActivateEnvironment(GameObject environment, string label)
    {
        scene2.SetActive(false);        // Hide the creator/menu scene
        DeactivateAllEnvironments();    // Turn off all environments
        scene3.SetActive(true);         // Show the environment scene
        environment.SetActive(true);    // Activate the selected environment
        Debug.Log($"{label} is now active");
    }

    // Updates the text on the environment button based on the input field's value
    void UpdateText(string newText)
    {
        // Update the button text based on the corresponding input field
        if (nameInput1.isFocused)
        {
            environmentButtonText1.text = string.IsNullOrEmpty(newText) ? "Environment 1" : newText;
        }
        if (nameInput2.isFocused)
        {
            environmentButtonText2.text = string.IsNullOrEmpty(newText) ? "Environment 2" : newText;
        }
        if (nameInput3.isFocused)
        {
            environmentButtonText3.text = string.IsNullOrEmpty(newText) ? "Environment 3" : newText;
        }
    }
}
