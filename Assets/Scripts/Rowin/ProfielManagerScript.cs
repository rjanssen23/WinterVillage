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

    // Vervang Meisje- en Jongen-objecten door profile2 en profile1
    public GameObject Profile2ButtonObject;
    public GameObject Profile1ButtonObject;

    public GameObject LoginPanel;
    public GameObject MainMenuButtons;

    public GameObject[] Profile1Objecten;
    public GameObject[] Profile2Objecten;
    public Transform[] SpawnPosities;

    public Transform[] environments;
    private int currentIndex = 0;

    public TMP_InputField ProfielNaam;
    public TMP_InputField GeboorteDatumInput;

    public TMP_Text Wereld1Text;
    public TMP_Text Wereld2Text;
    public TMP_Text Wereld3Text;

    public Button ProfielToevoegenButton;
    public Button NaarProfielSelectieButton;
    public Button MaakProfielButton;
    public Button Profile1Button;    // eerder JongenButton
    public Button Profile2Button;    // eerder MeisjeButton
    public Button VolgendeSceneButton;
    public Button Profile2Prefab;    // eerder MeisjePrefab
    public Button Profile1Prefab;    // eerder JongenPrefab
    public Button TerugNaarMenu;
    public Button BootBackButton;

    public Button[] KindKnoppen;

    public TMP_Dropdown wereldDropdown;

    public ProfielkeuzeApiClient profielkeuzeApiClient;

    private int spawnIndex = 0;
    // isProfile1Gekozen vervangt isJongenGekozen
    private bool isProfile1Gekozen = true;
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
        Profile1Button.onClick.AddListener(Profile1Gekozen);
        Profile2Button.onClick.AddListener(Profile2Gekozen);
        MaakProfielButton.onClick.AddListener(SpawnObject);
        VolgendeSceneButton.onClick.AddListener(VolgendeSceneSwitch);
        Profile2Prefab.onClick.AddListener(VolgendeSceneSwitch);
        Profile1Prefab.onClick.AddListener(VolgendeSceneSwitch);
        TerugNaarMenu.onClick.AddListener(HoofdmenuSwitch);
        BootBackButton.onClick.AddListener(BootBackNaarProfiel);
        wereldDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(wereldDropdown); });

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

        if (Wereld1Text != null)
            Wereld2Text.text = selectedText;
        Wereld2Text.text = selectedText;
        Wereld3Text.text = selectedText;
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
        Profile2ButtonObject.SetActive(true);
        Profile1ButtonObject.SetActive(true);
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
        string arts = (wereldDropdown != null && wereldDropdown.options.Count > 0)
            ? wereldDropdown.options[wereldDropdown.value].text
            : "";

        // Sla als avatar op: "profile1" of "profile2"
        ProfielKeuze newProfielKeuze = new ProfielKeuze
        {
            name = ProfielNaam.text,
            geboorteDatum = geboorteDatum,
            arts = arts,
            avatar = isProfile1Gekozen ? "profile1" : "profile2",
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

    // Methode voor profile1 (voorheen Jongen)
    public void Profile1Gekozen()
    {
        isProfile1Gekozen = true;
        Profile2ButtonObject.SetActive(false);
        Profile1ButtonObject.SetActive(true);
    }

    // Methode voor profile2 (voorheen Meisje)
    public void Profile2Gekozen()
    {
        isProfile1Gekozen = false;
        Profile1ButtonObject.SetActive(false);
        Profile2ButtonObject.SetActive(true);
    }

    public void BootBackNaarProfiel()
    {
        ProfielSelectieScherm.SetActive(true);
        ProfielAanmakenScherm.SetActive(false);
    }

    public void SpawnObject()
    {
        // Gebruik de juiste objecten op basis van de keuze
        GameObject[] gekozenObjecten = isProfile1Gekozen ? Profile1Objecten : Profile2Objecten;

        if (aantalProfielenAangemaakt == 3)
        {
            Debug.LogWarning("Geen beschikbare objecten of spawnposities meer!");
            return;
        }

        GameObject newObject = Instantiate(gekozenObjecten[spawnIndex], SpawnPosities[spawnIndex].position, Quaternion.identity);
        if (ProfilePrison != null)
            newObject.transform.SetParent(ProfilePrison.transform, false);

        if (textPrefab != null)
        {
            GameObject newText = Instantiate(textPrefab, SpawnPosities[spawnIndex].position, Quaternion.identity);
            newText.transform.SetParent(newObject.transform, false);

            TMP_Text textComponent = newText.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = isProfile1Gekozen ? "profile1" : "profile2";
                newText.transform.localPosition = new Vector3(0, -73, 0);
                textComponent.fontSize = 50;
            }
        }

        spawnIndex = (spawnIndex + 1) % gekozenObjecten.Length;
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
            // Kies het juiste prefab op basis van de avatar-waarde ("profile1" of "profile2")
            GameObject prefabToUse = profiel.avatar == "profile1" ? Profile1Prefab.gameObject : Profile2Prefab.gameObject;
            Transform spawnPosition = SpawnPosities[aantalProfielenAangemaakt % SpawnPosities.Length];
            GameObject newButton = Instantiate(prefabToUse, spawnPosition.position, Quaternion.identity, ProfilePrison.transform);

            TMP_Text textComponent = newButton.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
                textComponent.text = profiel.avatar == "profile1" ? "profile1" : "profile2";

            if (textPrefab != null)
            {
                GameObject newText = Instantiate(textPrefab, spawnPosition.position, Quaternion.identity);
                newText.transform.SetParent(newButton.transform, false);

                TMP_Text textPrefabComponent = newText.GetComponent<TMP_Text>();
                if (textPrefabComponent != null)
                {
                    textPrefabComponent.text = profiel.avatar == "profile1" ? "profile1" : "profile2";
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
        string displayName = profiel.avatar == "profile1" ? "profile1" : "profile2";
        Debug.Log($"Name: {displayName}, Arts: {profiel.arts}, GeboorteDatum: {profiel.geboorteDatum}, Avatar: {profiel.avatar}");
    }

    private void SelectProfile(ProfielKeuze profiel)
    {
        profielkeuzetoken = profiel.id;
        SelectedProfielKeuzeId = profiel.id;
        Debug.Log("Selected profile: " + (profiel.avatar == "profile1" ? "profile1" : "profile2"));
        Debug.Log("Profielkeuze token: " + profielkeuzetoken);
    }
}

