using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bolichePrefab;
    public GameObject boulePrefab;

    [Header("UI")]
    public TextMeshProUGUI distanceText;
    public Button clearButton;

    private GameObject boliche;
    private List<GameObject> boules = new List<GameObject>();

    void Start()
    {
        clearButton.onClick.AddListener(ClearBoules);
    }

    public void SpawnBoliche(Vector3 position)
    {
        if (boliche != null) Destroy(boliche);
        boliche = Instantiate(bolichePrefab, position, Quaternion.identity);
    }

    public void SpawnBoule(Vector3 position)
    {
        GameObject newBoule = Instantiate(boulePrefab, position, Quaternion.identity);
        boules.Add(newBoule);
    }

    void Update()
    {
        if (boliche == null || boules.Count == 0) return;

        float minDistance = float.MaxValue;

        foreach (GameObject boule in boules)
        {
            float dist = Vector3.Distance(boule.transform.position, boliche.transform.position);
            if (dist < minDistance) minDistance = dist;
        }

        distanceText.text = $"Closest boule: {minDistance:F2} m";
    }

    void ClearBoules()
    {
        // Solo eliminar las boules lanzadas
        foreach (var boule in boules)
        {
            if (boule != null)
                Destroy(boule);
        }

        boules.Clear();

        // Mantiene el boliche en escena
        distanceText.text = "Boules cleared!";
    }

}
