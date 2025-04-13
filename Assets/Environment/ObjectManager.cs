using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    // Menu om objecten vanuit te plaatsen
    public GameObject UISideMenu;

    public Transform parentVoorNieuweObjecten; // Sleep hier het gewenste object in de Inspector

    // Switch van scene
    public List <GameObject> scenesDeactivate;
    public List <GameObject> scenesActivate;

    // Lijst met objecten die geplaatst kunnen worden die overeenkomen met de prefabs in de prefabs map
    public List<GameObject> prefabObjects;

    // Lijst met objecten die geplaatst zijn in de wereld
    private List<GameObject> placedObjects;

    // Methode om een nieuw 2D object te plaatsen
    public void PlaceNewObject2D(int index)
    {
        // Verberg het zijmenu
        UISideMenu.SetActive(false);
        // Instantieer het prefab object op de positie (0,0,0) met geen rotatie
        GameObject instanceOfPrefab = Instantiate(prefabObjects[index], Vector3.zero, Quaternion.identity);
        // Haal het Object2D component op van het nieuw geplaatste object
        Object2D object2D = instanceOfPrefab.GetComponent<Object2D>();
        // Stel de objectManager van het object in op deze instantie van ObjectManager
        object2D.objectManager = this;
        // Zet de isDragging eigenschap van het object op true zodat het gesleept kan worden
        object2D.isDragging = true;
    }

    // Methode om het menu te tonen
    public void ShowMenu()
    {
        UISideMenu.SetActive(true);
    }

    public void TerugKnop()
    {
        foreach (GameObject obj in scenesDeactivate)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in scenesActivate)
        {
            obj.SetActive(true);
        }
    }

    // Methode om de huidige sc�ne te resetten
    public void Reset()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
