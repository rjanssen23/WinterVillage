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
    public GameObject environmentNietGoed;

    public GameObject Scene1;
    public GameObject Scene2;

    public int aantalProfielenAangemaakt = 0;

    public GameObject MeisjeButtonObject;
    public GameObject JongenButtonObject;

    public GameObject LoginPanel;
    public GameObject MainMenuButtons;

    public GameObject[] JongenObjecten;
    public GameObject[] MeisjeObjecten;
    public Transform[] SpawnPosities;

    public Transform[] environments; // Sleep hier je spawnpunten in
    private int environmentIndex = 0;


    public TMP_InputField ProfielNaam;
    public TMP_InputField GeboorteDatumInput;

    public TMP_Text Dokter1Text;
    public TMP_Text Dokter2Text;
    public TMP_Text Dokter3Text;

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


    public TMP_Dropdown dokterDropdown;

    public ProfielkeuzeApiClient profielkeuzeApiClient;

    private int spawnIndex = 0;
    private bool isJongenGekozen = true;
    private bool verkeerdeNaam = true;

    public string SelectedProfielKeuzeId { get; private set; }

    private string profielkeuzetoken;

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
        dokterDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dokterDropdown); });

        if (GeboorteDatumInput == null)
            Debug.LogError("GeboorteDatumInput is NULL at Start!");

        foreach (Button knop in KindKnoppen)
        {
            knop.onClick.AddListener(ProfielGeselecteerd);
        }

        FetchProfiles();
    }

    public void Reset()
    {
        ProfielSelectieScherm.SetActive(true);
        ProfielAanmakenScherm.SetActive(false);
        VolgendeScene.SetActive(false);
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int selectedIndex = dropdown.value;
        string selectedText = dropdown.options[selectedIndex].text;

        if (Dokter1Text != null)
            Dokter1Text.text = selectedText;
        Dokter2Text.text = selectedText;
        Dokter3Text.text = selectedText;
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
        environmentNietGoed.SetActive(false);
        FetchProfiles();
    }

    public async void MaakProfiel()
    {
        if (ProfielNaam == null || string.IsNullOrWhiteSpace(ProfielNaam.text))
        {
            Debug.LogError("ProfielNaam is verplicht!");
            return;
        }

        if (ProfielNaam.text.Length < 1 || ProfielNaam.text.Length > 25)
        {
            Debug.LogError("De profielnaam moet tussen de 1 en 25 tekens zijn.");
            environmentNietGoed.SetActive(true);
            return;
        }

        string geboorteDatum = GeboorteDatumInput != null ? GeboorteDatumInput.text : "";
        string arts = (dokterDropdown != null && dokterDropdown.options.Count > 0)
            ? dokterDropdown.options[dokterDropdown.value].text
            : "";

        ProfielKeuze newProfielKeuze = new ProfielKeuze
        {
            name = ProfielNaam.text,
            geboorteDatum = geboorteDatum,
            arts = arts,
            avatar = isJongenGekozen ? "Jongen" : "Meisje",
        };

        if (profielkeuzeApiClient == null)
        {
            Debug.LogError("profielkeuzeApiClient is NULL!");
            return;
        }

        IWebRequestReponse webRequestResponse = await profielkeuzeApiClient.CreateProfielKeuze(newProfielKeuze);

        switch (webRequestResponse)
        {
            case WebRequestData<ProfielKeuze>:
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Create profielKeuze error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("Unhandled webRequestResponse type");
        }

        environmentNietGoed.SetActive(false);
        ProfielSelectieScherm.SetActive(true);
        ProfielAanmakenScherm.SetActive(false);
        FetchProfiles();
    }

    public void JongenGekozen()
    {
        isJongenGekozen = true;
        MeisjeButtonObject.SetActive(false);
        JongenButtonObject.SetActive(true);
    }

    public void MeisjeGekozen()
    {
        isJongenGekozen = false;
        JongenButtonObject.SetActive(false);
        MeisjeButtonObject.SetActive(true);
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

        // Spawn bij de huidige environment positie
        Transform spawnPoint = environments[environmentIndex];
        GameObject newObject = Instantiate(gekozenObjecten[spawnIndex], spawnPoint.position, Quaternion.identity);

        if (ProfilePrison != null)
            newObject.transform.SetParent(ProfilePrison.transform, false);

        if (textPrefab != null)
        {
            GameObject newText = Instantiate(textPrefab, spawnPoint.position, Quaternion.identity);
            newText.transform.SetParent(newObject.transform, false);

            TMP_Text textComponent = newText.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = ProfielNaam.text;
                newText.transform.localPosition = new Vector3(0, -73, 0);
                textComponent.fontSize = 50;
            }
        }

        // Volgende indices bijhouden
        spawnIndex = (spawnIndex + 1) % gekozenObjecten.Length;
        environmentIndex = (environmentIndex + 1) % environments.Length;
        aantalProfielenAangemaakt++;
    }


    public async void FetchProfiles()
    {
        IWebRequestReponse webRequestResponse = await profielkeuzeApiClient.ReadProfielKeuzes();

        switch (webRequestResponse)
        {
            case WebRequestData<List<ProfielKeuze>> dataResponse:
                DisplayProfiles(dataResponse.Data);
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Read profielKeuzes error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("Unhandled webRequestResponse type");
        }
    }

    private void DisplayProfiles(List<ProfielKeuze> profielKeuzes)
    {
        foreach (Transform child in ProfilePrison.transform)
        {
            Destroy(child.gameObject);
        }

        aantalProfielenAangemaakt = 0;

        foreach (ProfielKeuze profiel in profielKeuzes)
        {
            GameObject prefabToUse = profiel.avatar == "Jongen" ? JongenPrefab.gameObject : MeisjePrefab.gameObject;
            Transform spawnPosition = SpawnPosities[aantalProfielenAangemaakt % SpawnPosities.Length];
            GameObject newButton = Instantiate(prefabToUse, spawnPosition.position, Quaternion.identity, ProfilePrison.transform);

            TMP_Text textComponent = newButton.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
                textComponent.text = profiel.name;

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
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => SelectProfile(profiel));
            }

            aantalProfielenAangemaakt++;
            DisplayProfileData(profiel);
        }
    }

    public async void VerwijderGeselecteerdProfiel()
    {
        if (string.IsNullOrEmpty(SelectedProfielKeuzeId))
        {
            Debug.LogWarning("Geen profiel geselecteerd om te verwijderen.");
            return;
        }

        IWebRequestReponse response = await profielkeuzeApiClient.DeleteProfielKeuze(SelectedProfielKeuzeId);

        switch (response)
        {
            case WebRequestData<object>:
                Debug.Log("Profiel succesvol verwijderd.");
                SelectedProfielKeuzeId = null;
                FetchProfiles();
                break;
            case WebRequestError error:
                Debug.LogError("Fout bij verwijderen profiel: " + error.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekend antwoord bij verwijderen profiel.");
                break;
        }
    }

    private void DisplayProfileData(ProfielKeuze profiel)
    {
        Debug.Log($"Name: {profiel.name}, Arts: {profiel.arts}, GeboorteDatum: {profiel.geboorteDatum}, Avatar: {profiel.avatar}");
    }

    private void SelectProfile(ProfielKeuze profiel)
    {
        profielkeuzetoken = profiel.id;
        SelectedProfielKeuzeId = profiel.id;
        Debug.Log("Selected profile: " + profiel.name);
        Debug.Log("Profielkeuze token: " + profielkeuzetoken);
    }

    public void SpawnAtEnvironment(GameObject objectToSpawn)
    {
        // Kies welk object moet worden gespawnd (Jongen of Meisje)
        GameObject[] gekozenObjecten = isJongenGekozen ? JongenObjecten : MeisjeObjecten;

        if (environments.Length == 0 || objectToSpawn == null)
        {
            Debug.LogWarning("Missing environment points or object to spawn.");
            return;
        }

        // Spawn at current environment position and rotation
        Instantiate(gekozenObjecten[spawnIndex], environments[environmentIndex].position, environments[environmentIndex].rotation);

        // Volgende environment gebruiken
        environmentIndex = (environmentIndex + 1) % environments.Length;
    }

}



