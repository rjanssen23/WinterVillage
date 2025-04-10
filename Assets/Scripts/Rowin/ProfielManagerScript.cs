using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProfielManagerScript : MonoBehaviour
{
    public GameObject ProfielSelectieScherm;
    public GameObject ProfielAanmakenScherm;
    public GameObject VolgendeScene;
    public GameObject textPrefab;
    public GameObject ProfilePrison;
    public GameObject HoofdMenu;

    //tijn
    public GameObject Scene1;
    public GameObject Scene2;
    // 

    public int aantalProfielenAangemaakt = 0;

    public GameObject MeisjeButtonObject;
    public GameObject JongenButtonObject;

    //tijn
    public GameObject LoginPanel;
    public GameObject MainMenuButtons;
    //

    public GameObject[] JongenObjecten; // Array voor de jongen objecten
    public GameObject[] MeisjeObjecten; // Array voor de meisje objecten
    public Transform[] SpawnPosities;

    public TMP_InputField ProfielNaam;

    public Button ProfielToevoegenButton;
    public Button NaarProfielSelectieButton;
    public Button MaakProfielButton;
    public Button JongenButton;
    public Button MeisjeButton;
    public Button VolgendeSceneButton;
    public Button MeisjePrefab;
    public Button JongenPrefab;
    public Button TerugNaarMenu;
    public Button BootBackButton;

    public Button[] KindKnoppen;

    public TMP_Dropdown dokterDropdown; // Assign this in Unity Inspector

    public ProfielkeuzeApiClient profielkeuzeApiClient; // Inject the API client

    private int spawnIndex = 0;
    private bool isJongenGekozen = true; // Default to jongen

    // Add this property to store the selected profielkeuzeId
    public string SelectedProfielKeuzeId { get; private set; }

    void Start()
    {
        Reset();
        FetchProfiles();
        ProfielToevoegenButton.onClick.AddListener(ProfielToevoegenScene);
        NaarProfielSelectieButton.onClick.AddListener(NaarProfielSelectie);
        MaakProfielButton.onClick.AddListener(MaakProfiel);
        JongenButton.onClick.AddListener(JongenGekozen);
        MeisjeButton.onClick.AddListener(MeisjeGekozen);
        MaakProfielButton.onClick.AddListener(SpawnObject);
        VolgendeSceneButton.onClick.AddListener(VolgendeSceneSwitch);
        MeisjePrefab.onClick.AddListener(VolgendeSceneSwitch);
        JongenPrefab.onClick.AddListener(VolgendeSceneSwitch);
        TerugNaarMenu.onClick.AddListener(HoofdmenuSwitch);
        BootBackButton.onClick.AddListener(BootBackNaarProfiel);


        foreach (Button knop in KindKnoppen)
        {
            knop.onClick.AddListener(ProfielGeselecteerd);
        }

        // Fetch profiles when the script starts
        FetchProfiles();
    }

    public void Reset()
    {
        ProfielSelectieScherm.SetActive(true);
        ProfielAanmakenScherm.SetActive(false);
        VolgendeScene.SetActive(false);
    }

    public void HoofdmenuSwitch()
    {
        ProfielSelectieScherm.SetActive(false);
        ProfielAanmakenScherm.SetActive(false);
        VolgendeScene.SetActive(false);
        HoofdMenu.SetActive(true);
        Scene1.SetActive(true);
        Scene2.SetActive(false);
        LoginPanel.SetActive(false);
        MainMenuButtons.SetActive(true);
    }

    public void VolgendeSceneSwitch()
    {
        ProfielSelectieScherm.SetActive(false);
        ProfielAanmakenScherm.SetActive(false);
        VolgendeScene.SetActive(true);
    }

    public void ProfielToevoegenScene()
    {
        ProfielSelectieScherm.SetActive(false);
        ProfielAanmakenScherm.SetActive(true);
        MeisjeButtonObject.SetActive(true);
        JongenButtonObject.SetActive(true);
    }

    public void ProfielGeselecteerd()
    {
        VolgendeScene.SetActive(true);
        ProfielSelectieScherm.SetActive(false);
        ProfielAanmakenScherm.SetActive(false);
    }

    public void NaarProfielSelectie()
    {
        ProfielSelectieScherm.SetActive(true);
        ProfielAanmakenScherm.SetActive(false);
        FetchProfiles(); // Fetch profiles when navigating to profile selection
    }

    public async void MaakProfiel()
    {
        Debug.Log("MaakProfiel() function started!");

        // Check if important objects are null before proceeding

        if (profielkeuzeApiClient == null) { Debug.LogError("profielkeuzeApiClient is NULL!"); return; }

        ProfielKeuze newProfielKeuze = new ProfielKeuze
        {
            name = ProfielNaam.text,
        };

        IWebRequestReponse webRequestResponse = await profielkeuzeApiClient.CreateProfielKeuze(newProfielKeuze);

        switch (webRequestResponse)
        {
            case WebRequestData<ProfielKeuze> dataResponse:
                //Debug.Log("Profiel Aangemaakt: " + dataResponse.Data.id);
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Create profielKeuze error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
        ProfielSelectieScherm.SetActive(true);
        ProfielAanmakenScherm.SetActive(false);
        FetchProfiles(); // Refresh profiles after creating a new one
    }

    public void JongenGekozen()
    {
        isJongenGekozen = true;
        MeisjeButtonObject.SetActive(false);
        JongenButtonObject.SetActive(true);
        Debug.Log("Jongen is gekozen");
    }

    public void MeisjeGekozen()
    {
        isJongenGekozen = false;
        JongenButtonObject.SetActive(false);
        MeisjeButtonObject.SetActive(true);
        Debug.Log("Meisje is gekozen");
    }

    public void BootBackNaarProfiel()
    {
        ProfielSelectieScherm.SetActive(true);
        ProfielAanmakenScherm.SetActive(false);
    }

    public void SpawnObject()
    {
        GameObject[] gekozenObjecten = isJongenGekozen ? JongenObjecten : MeisjeObjecten;

        if (aantalProfielenAangemaakt == 3)
        {
            Debug.LogWarning("Geen beschikbare objecten of spawnposities meer!");
            return;
        }

        GameObject newObject = Instantiate(gekozenObjecten[spawnIndex], SpawnPosities[spawnIndex].position, Quaternion.identity);

        if (ProfilePrison != null)
        {
            newObject.transform.SetParent(ProfilePrison.transform, false);
        }
        else
        {
            Debug.LogWarning("ProfilePrison is not assigned in the Inspector!");
        }

        if (textPrefab != null)
        {
            GameObject newText = Instantiate(textPrefab, SpawnPosities[spawnIndex].position, Quaternion.identity);
            newText.transform.SetParent(newObject.transform, false);

            TMP_Text textComponent = newText.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = ProfielNaam.text;
                newText.transform.localPosition = new Vector3(0, -73, 0);
                textComponent.fontSize = 50;
            }
            else
            {
                Debug.LogWarning("Text prefab has no TMP_Text component!");
            }
        }
        else
        {
            Debug.LogWarning("Text prefab is not assigned!");
        }

        spawnIndex = (spawnIndex + 1) % gekozenObjecten.Length;
        aantalProfielenAangemaakt++;
    }

    public async void FetchProfiles()
    {
        Debug.Log("Fetching profiles...");

        IWebRequestReponse webRequestResponse = await profielkeuzeApiClient.ReadProfielKeuzes();

        switch (webRequestResponse)
        {
            case WebRequestData<List<ProfielKeuze>> dataResponse:
                List<ProfielKeuze> profielKeuzes = dataResponse.Data;
                DisplayProfiles(profielKeuzes);
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Read profielKeuzes error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    private void DisplayProfiles(List<ProfielKeuze> profielKeuzes)
    {
        // Clear existing profile buttons
        foreach (Transform child in ProfilePrison.transform)
        {
            Destroy(child.gameObject);
        }

        // Reset the counter before adding new profiles
        aantalProfielenAangemaakt = 0;

        // Create a button for each profile
        foreach (ProfielKeuze profiel in profielKeuzes)
        {
            GameObject prefabToUse = profiel.avatar == "Jongen" ? JongenPrefab.gameObject : MeisjePrefab.gameObject;
            Transform spawnPosition = SpawnPosities[aantalProfielenAangemaakt % SpawnPosities.Length];
            GameObject newButton = Instantiate(prefabToUse, spawnPosition.position, Quaternion.identity, ProfilePrison.transform);

            TMP_Text textComponent = newButton.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = profiel.name;
            }
            else
            {
                Debug.LogWarning("Prefab has no TMP_Text component!");
            }

            if (textPrefab != null)
            {
                GameObject newText = Instantiate(textPrefab, spawnPosition.position, Quaternion.identity);
                newText.transform.SetParent(newButton.transform, false);

                TMP_Text textPrefabComponent = newText.GetComponent<TMP_Text>();
                if (textPrefabComponent != null)
                {
                    textPrefabComponent.text = profiel.name;
                    newText.transform.localPosition = new Vector3(0, -73, 0);
                    textPrefabComponent.fontSize = 50;
                }
                else
                {
                    Debug.LogWarning("Text prefab has no TMP_Text component!");
                }
            }
            else
            {
                Debug.LogWarning("Text prefab is not assigned!");
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => SelectProfile(profiel));
            }
            else
            {
                Debug.LogWarning("Prefab has no Button component!");
            }

            // Increment the counter for each profile added
            aantalProfielenAangemaakt++;

            // Display profile data in Unity UI elements
            DisplayProfileData(profiel);
        }
    }

    private void DisplayProfileData(ProfielKeuze profiel)
    {
        // Example of displaying profile data in Unity UI elements
        // You can customize this method to display the data as needed
        Debug.Log($"EnvironmentName: {profiel.name}");
    }

    private string profielkeuzetoken;
    private void SelectProfile(ProfielKeuze profiel)
    {
        Debug.Log("Selected profile: " + profiel.name);
        // Handle profile selection logic here
        // Store the profielkeuzetoken
        profielkeuzetoken = profiel.id;
        Debug.Log("Profielkeuze token: " + profielkeuzetoken);

        // Store the selected profielkeuzeId
        SelectedProfielKeuzeId = profiel.id;
    }
}
