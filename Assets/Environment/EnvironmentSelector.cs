using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Reflection;

public class EnvironmentSelector : MonoBehaviour
{
    [Header("Scene Objecten")]
    [SerializeField] private GameObject[] scenes;  // Zorg dat Scene5 op index 4 staat

    [Header("Omgevings Objecten")]
    [SerializeField] private GameObject[] environments;

    [Header("Omgeving Knoppen")]
    [SerializeField] private Button[] environmentButtons;

    [Header("Terugknoppen per Omgeving")]
    [SerializeField] private Button[] terugKnoppen;

    [Header("Neeknoppen in warningscherm")]
    [SerializeField] private Button neeKnop;

    [Header("Ja-knop in warningscherm")]
    [SerializeField] private Button jaKnop;


    private int pendingEnvironmentIndex = -1; // Wachtende omgeving

    private bool[] environmentOpened;

    void Start()
    {
        Reset();

        environmentOpened = new bool[environments.Length];

        // Voeg listeners toe aan de omgeving-knoppen
        for (int i = 0; i < environmentButtons.Length; i++)
        {
            int index = i;
            environmentButtons[i].onClick.AddListener(() => LaadOmgeving(index));
        }

        // Voeg listeners toe aan de terugknoppen
        for (int i = 0; i < terugKnoppen.Length; i++)
        {
            int index = i;
            terugKnoppen[i].onClick.AddListener(() => Terug(index));
        }

        // Voeg listeners toe

        neeKnop.onClick.AddListener(NeeIngedrukt);

        jaKnop.onClick.AddListener(JaIngedrukt);


    }

    public void Reset()
    {
        // Reset-logica
    }

    private void DeactiveerAlleScenes()
    {
        foreach (GameObject scene in scenes)
        {
            scene.SetActive(false);
        }
    }

    private void DeactiveerAlleOmgevingen()
    {
        foreach (GameObject environment in environments)
        {
            environment.SetActive(false);
        }
    }

    private void VerbergAlleTerugknoppen()
    {
        foreach (Button btn in terugKnoppen)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void NeeIngedrukt()
    {
        if (pendingEnvironmentIndex >= 0 && pendingEnvironmentIndex < environments.Length)
        {
            environmentOpened[pendingEnvironmentIndex] = false;

            DeactiveerAlleScenes(); // Verberg warningscherm
            DeactiveerAlleOmgevingen();
            VerbergAlleTerugknoppen();

            scenes[3].SetActive(false);
            scenes[2].SetActive(true); // Of een andere scene als je dat wil

        }
    }


    public void JaIngedrukt()
    {
        if (pendingEnvironmentIndex >= 0 && pendingEnvironmentIndex < environments.Length)
        {
            environmentOpened[pendingEnvironmentIndex] = true;

            DeactiveerAlleScenes(); // Verberg warningscherm
            DeactiveerAlleOmgevingen();
            VerbergAlleTerugknoppen();

            scenes[4].SetActive(true); // Of een andere scene als je dat wil
            environments[pendingEnvironmentIndex].SetActive(true);
            terugKnoppen[pendingEnvironmentIndex].gameObject.SetActive(true);

            pendingEnvironmentIndex = -1; // Reset
        }
    }

    public void LaadOmgeving(int index)
    {
        DeactiveerAlleScenes();
        DeactiveerAlleOmgevingen();
        VerbergAlleTerugknoppen();

        if (index >= 0 && index < environments.Length)
        {
            if (!environmentOpened[index])
            {
                pendingEnvironmentIndex = index;

                if (scenes.Length > 4)
                {
                    scenes[3].SetActive(true); // Warning scene 4
                    scenes[4].SetActive(true); // Warning scene 5
                }
                else
                {
                    Debug.LogWarning("Scene 4 of 5 mist.");
                }

                return; // Stop hier, wacht op bevestiging via “ja”-knop
            }
            else
            {
                scenes[4].SetActive(true); // Normale flow bij latere klik
                environments[index].SetActive(true);
                terugKnoppen[index].gameObject.SetActive(true);
            }
        }
    }



    public void Terug(int index)
    {
        environments[index].SetActive(false);
        terugKnoppen[index].gameObject.SetActive(false);
        scenes[4].SetActive(false); // Verberg Scene5 or hide scene 4
        scenes[2].SetActive(true);
    }
}


