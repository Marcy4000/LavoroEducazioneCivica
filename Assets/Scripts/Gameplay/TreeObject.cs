using System;
using UnityEngine;

public class TreeObject : MonoBehaviour
{
    private int treeId;
    private float growthTime;
    private Vector3 startScale;
    private Vector3 fullScale;

    private DateTime plantedTime;
    private const string SaveKey = "TreeData";

    [SerializeField] private GameObject[] treeSprites;

    public void InitializeTree(int id, float growthDuration, Vector3 initialScale, Vector3 finalScale)
    {
        treeId = id;
        growthTime = growthDuration;
        startScale = initialScale;
        fullScale = finalScale;

        plantedTime = DateTime.UtcNow;
        SaveTreeData();

        foreach (GameObject treeSprite in treeSprites)
        {
            treeSprite.SetActive(false);
        }

        treeSprites[UnityEngine.Random.Range(0, treeSprites.Length)].SetActive(true);
    }

    private void Start()
    {
        LoadTreeData();
        UpdateGrowth();
    }

    private void Update()
    {
        UpdateGrowth();
    }

    private void UpdateGrowth()
    {
        TimeSpan elapsedTime = DateTime.UtcNow - plantedTime;
        float elapsedSeconds = (float)elapsedTime.TotalSeconds;

        float growthProgress = Mathf.Clamp01(elapsedSeconds / growthTime);
        transform.localScale = Vector3.Lerp(startScale, fullScale, growthProgress);

        if (growthProgress >= 1f)
        {
            if (transform.localScale != fullScale)
            {
                transform.localScale = fullScale;
            }
        }
    }

    private string SaveTreeData()
    {
        return $"{treeId},{plantedTime.ToString()}";
    }

    private void LoadTreeData()
    {
        string saveData = PlayerPrefs.GetString($"{SaveKey}_{GetInstanceID()}", null);
        if (!string.IsNullOrEmpty(saveData))
        {
            string[] dataParts = saveData.Split(',');
            treeId = int.Parse(dataParts[0]);
            plantedTime = DateTime.Parse(dataParts[1]);
        }
    }
}
