using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentAanmaker : MonoBehaviour
{
    public GameObject scene2;
    public GameObject scene3;

    public GameObject environment1;
    public GameObject environment2;
    public GameObject environment3;

    public GameObject environment1ButtonObject;
    public GameObject environment2ButtonObject;
    public GameObject environment3ButtonObject;

    public Button environment1Button;
    public Button environment2Button;
    public Button environment3Button;

    public Button Environment1ToevoegenButton;
    public Button Environment2ToevoegenButton;
    public Button Environment3ToevoegenButton;

    public Button environment1VerwijderenButton;
    public Button environment2VerwijderenButton;
    public Button environment3VerwijderenButton;

    public TMP_InputField EnvironmentNaam;

    public
    // Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        Environment1ToevoegenButton.onClick.AddListener(VoegEnvironment1Toe);
        Environment2ToevoegenButton.onClick.AddListener(VoegEnvironment2Toe);
        Environment3ToevoegenButton.onClick.AddListener(VoegEnvironment3Toe);

        environment1VerwijderenButton.onClick.AddListener(VerwijderEnvironment1);
        environment2VerwijderenButton.onClick.AddListener(VerwijderEnvironment2);
        environment3VerwijderenButton.onClick.AddListener(VerwijderEnvironment3);

        environment1Button.onClick.AddListener(SelecteerEnvironment1);
        environment1Button.onClick.AddListener(SelecteerEnvironment2);
        environment1Button.onClick.AddListener(SelecteerEnvironment3);


    }

    public void VoegEnvironment1Toe()
    {
        environment1ButtonObject.SetActive(true);
    }
    public void VoegEnvironment2Toe()
    {
        environment2ButtonObject.SetActive(true);
    }
    public void VoegEnvironment3Toe()
    {
        environment3ButtonObject.SetActive(true);
    }

    public void VerwijderEnvironment1()
    {
        environment1ButtonObject.SetActive(false);
    }
    public void VerwijderEnvironment2()
    {
        environment2ButtonObject.SetActive(false);
    }
    public void VerwijderEnvironment3()
    {
        environment3ButtonObject.SetActive(false);
    }

    public void SelecteerEnvironment1()
    {
        scene2.SetActive(false);
        scene3.SetActive(true);
        environment1.SetActive(true);
        environment2.SetActive(false);
        environment3.SetActive(false);
    }

    public void SelecteerEnvironment2()
    {
        scene2.SetActive(false);
        scene3.SetActive(true);
        environment1.SetActive(false);
        environment2.SetActive(true);
        environment3.SetActive(false);
    }

    public void SelecteerEnvironment3()
    {
        scene2.SetActive(false);
        scene3.SetActive(true);
        environment1.SetActive(false);
        environment2.SetActive(false);
        environment3.SetActive(true);
    }

}

