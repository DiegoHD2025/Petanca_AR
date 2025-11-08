using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject boulePrefab;
    public GameObject bolichePrefab;
    public TextMeshProUGUI distanceText;

    private GameObject boliche;
    private List<GameObject> boules = new List<GameObject>();

    void Start()
    {
        SpawnBoliche(Vector3.zero);
    }

    public void SpawnBoliche(Vector3 position)
    {
        if (boliche == null)
            boliche = Instantiate(bolichePrefab, position, Quaternion.identity);
    }

    public void SpawnBoule(Vector3 position)
    {
        GameObject b = Instantiate(boulePrefab, position, Quaternion.identity);
        boules.Add(b);
    }

    void Update()
    {
        UpdateClosestDistance();
    }

    void UpdateClosestDistance()
    {
        if (boliche == null || boules.Count == 0) return;

        float closest = float.MaxValue;
        foreach (var b in boules)
        {
            float d = Vector3.Distance(b.transform.position, boliche.transform.position);
            if (d < closest)
                closest = d;
        }

        distanceText.text = $"Closest boule: {closest:F2} m";
    }

    public void ClearBoules()
    {
        foreach (var b in boules)
            Destroy(b);
        boules.Clear();
    }
}
