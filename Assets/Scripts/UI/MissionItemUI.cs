using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MissionItem : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Text missionTitleText;
    public Image rewardImage;
    public TMP_Text rewardQuantityText;
    public Button missionButton;

    private MissionData missionData;
    private float rewardCycleTimer;
    private int rewardIndex;

    public UnityEvent<MissionData> OnMissionSelected = new UnityEvent<MissionData>(); // Event for mission selection

    public MissionData MissionData => missionData;

    public void Initialize(MissionData mission)
    {
        if (mission == null)
        {
            Debug.LogWarning("Mission is null!");
            return;
        }

        missionData = mission;
        missionTitleText.text = mission.title;

        if (mission.rewards.Length > 0)
        {
            rewardIndex = 0;
            UpdateRewardDisplay();
        }
        else
        {
            rewardImage.enabled = false;
            rewardQuantityText.text = "No Rewards";
        }

        missionButton.onClick.AddListener(() => HandleMissionSelected());
    }

    public void SetMissionCompleted()
    {
        missionButton.interactable = false;
        missionTitleText.fontStyle = FontStyles.Strikethrough;
    }

    private void HandleMissionSelected()
    {
        OnMissionSelected?.Invoke(missionData); // Invoke the event to notify listeners
    }

    private void Update()
    {
        if (missionData != null && missionData.rewards.Length > 0)
        {
            rewardCycleTimer += Time.deltaTime;
            if (rewardCycleTimer >= 5f)
            {
                rewardCycleTimer = 0f;
                rewardIndex = (rewardIndex + 1) % missionData.rewards.Length;
                UpdateRewardDisplay();
            }
        }
    }

    private void UpdateRewardDisplay()
    {
        var reward = missionData.rewards[rewardIndex];
        rewardImage.sprite = reward.rewardCurrency ? CurrencyManager.Instance.CurrencySprite : reward.item.icon;
        rewardQuantityText.text = $"x{reward.amount}";
    }

    private void OnDestroy()
    {
        // Cleanup to prevent memory leaks
        missionButton.onClick.RemoveAllListeners();
    }
}
