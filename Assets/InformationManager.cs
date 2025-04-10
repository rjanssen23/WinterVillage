using UnityEngine;
using UnityEngine.UI;

public class EersteEilandManager : MonoBehaviour
{
    public Button EersteButton;
    public RawImage EersteVakjeInformatie;

    public void EnableInformation()
    {
        Debug.Log("Clicked");
        EersteVakjeInformatie.gameObject.SetActive(true);
    }

    public void DisableInformation()
    {
        Debug.Log("Clicked");
        EersteVakjeInformatie.gameObject.SetActive(false);
    }
}
