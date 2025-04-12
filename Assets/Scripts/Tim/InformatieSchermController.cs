using UnityEngine;
using UnityEngine.UI;

public class InformatieSchermController : MonoBehaviour
{
    // alle scenes
    
    public GameObject scene2;


    public GameObject scene3; 
    public GameObject scene4;

    public GameObject ProfielSelectieScherm;
    public GameObject ProfielAanmakenScherm;

    public GameObject TextScreen;
    public GameObject VideoScreen;

    public Button BackToProfiles;
    public Button NextToProfiles;
    public Button ToText;
    public Button ToVideo;
    public Button ToScene4;
  

    void Start()
    {
        // Voeg event listeners toe aan de knoppen
        BackToProfiles.onClick.AddListener(ToProfileManager);
        NextToProfiles.onClick.AddListener(ToProfileManager);
        ToText.onClick.AddListener(ToInfoScreen);
        ToVideo.onClick.AddListener(ShowVideo);
        ToText.onClick.AddListener(ShowText);
        ToScene4.onClick.AddListener(ToWorldMap);
    }

    void ToProfileManager()
    {
        scene3.SetActive(false);
        scene4.SetActive(false);

        ProfielSelectieScherm.SetActive(true);
        ProfielAanmakenScherm.SetActive(false);
    }

    void ToInfoScreen()
    {

        scene2.SetActive(false);
        scene3.SetActive(true);
        scene4.SetActive(false);
    }

    void ToWorldMap()
    {

        
        scene2.SetActive(false);
        scene3.SetActive(false);
        scene4.SetActive(true);
    }

    void ShowText()
    {
        TextScreen.SetActive(true);
        VideoScreen.SetActive(false);
    }

    void ShowVideo()
    {
        TextScreen.SetActive(false);
        VideoScreen.SetActive(true);
    }
}
