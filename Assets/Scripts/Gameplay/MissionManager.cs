using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject missionListUI; // Parent UI element displaying all missions
    public GameObject missionItemPrefab; // Prefab for individual mission items
    public MissionFormController missionFormController;

    [Header("Missions")]
    public List<MissionData> missions = new List<MissionData>();

    private List<MissionItem> missionItems = new List<MissionItem>();

    private void Start()
    {
        PopulateMissionList();
    }

    private void PopulateMissionList()
    {
        missionItems.Clear();

        foreach (var mission in missions)
        {
            GameObject missionItemObj = Instantiate(missionItemPrefab, missionListUI.transform);
            var missionItem = missionItemObj.GetComponent<MissionItem>();
            missionItem.Initialize(mission);

            missionItems.Add(missionItem);

            // Subscribe to the mission item's event
            missionItem.OnMissionSelected.AddListener(OnMissionSelected);
        }
    }

    public void OnMissionSelected(MissionData mission)
    {
        if (mission.CanBeCompleted())
        {
            missionFormController.OpenMissionForm(mission);
        }
        else
        {
            Debug.Log("Mission cannot be completed yet!");
        }
    }

    public void CompleteMission(MissionData mission, string answer)
    {
        if (mission.EvaluateAnswer(answer))
        {
            mission.Complete();
            missionItems.Find(x => x.MissionData == mission)?.SetMissionCompleted();
            GrantRewards(mission);
        }
        else
        {
            Debug.Log("Invalid answer for mission.");
        }
    }

    private void GrantRewards(MissionData mission)
    {
        foreach (var reward in mission.rewards)
        {
            if (reward.rewardCurrency)
            {
                CurrencyManager.Instance.AddCurrency(reward.amount);
            }
            else
            {
                InventoryManager.Instance.AddItem(reward.item, reward.amount);
            }
        }
    }
}
