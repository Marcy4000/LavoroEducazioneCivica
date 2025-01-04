using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Missions/Mission")]
public class MissionData : ScriptableObject
{
    public string title; // Title of the mission
    public string description; // Mission details
    public string question; // The form question, e.g., "How many km did you travel by car?"
    public AnswerType expectedAnswerType; // Type of answer (e.g., "number", "text")
    public string expectedAnswer; // Expected answer to the question

    public Reward[] rewards; // Rewards for completing the mission
    public MissionConstraint constraint; // Restriction type: Once, Daily, etc.

    private DateTime lastCompletedTime;

    public bool CanBeCompleted()
    {
        switch (constraint)
        {
            case MissionConstraint.Once:
                return lastCompletedTime == default;
            case MissionConstraint.Daily:
                return lastCompletedTime.Date != DateTime.Now.Date;
            case MissionConstraint.Unrestricted:
                return true;
            default:
                return false;
        }
    }

    public bool EvaluateAnswer(string answer)
    {
        if (string.IsNullOrEmpty(answer))
        {
            return false;
        }

        switch (expectedAnswerType)
        {
            case AnswerType.Integer:
                return int.TryParse(answer, out int intResult) && intResult.ToString() == expectedAnswer;
            case AnswerType.Float:
                return float.TryParse(answer, out float floatResult) && floatResult.ToString() == expectedAnswer;
            case AnswerType.Text:
                return string.Equals(answer, expectedAnswer, StringComparison.OrdinalIgnoreCase);
            default:
                return false;
        }
    }

    public void Complete()
    {
        lastCompletedTime = DateTime.Now;
    }
}

[Serializable]
public class Reward
{
    public bool rewardCurrency; // Reward currency flag
    public ShopItemData item; // Reward item (e.g., seeds)
    public int amount; // Quantity of the item
}

public enum MissionConstraint
{
    Once,
    Daily,
    Unrestricted
}

public enum AnswerType
{
    Integer,
    Float,
    Text
}