using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionFormController : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text missionTitleText; // Displays the mission title
    public TMP_Text missionQuestionText; // Displays the mission question
    public TMP_InputField answerInputField; // Field for the player's answer
    public Button submitButton;

    private MissionData currentMission;
    private MissionManager missionManager;

    private void Start()
    {
        missionManager = FindObjectOfType<MissionManager>();

        gameObject.SetActive(false);
    }

    public void OpenMissionForm(MissionData mission)
    {
        currentMission = mission;

        missionTitleText.text = mission.title;
        missionQuestionText.text = mission.question;
        answerInputField.text = "";
        switch (mission.expectedAnswerType)
        {
            case AnswerType.Integer:
                answerInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                break;
            case AnswerType.Float:
                answerInputField.contentType = TMP_InputField.ContentType.DecimalNumber;
                break;
            case AnswerType.Text:
                answerInputField.contentType = TMP_InputField.ContentType.Standard;
                break;
            default:
                break;
        }

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnSubmit);

        gameObject.SetActive(true);
    }

    private void OnSubmit()
    {
        string answer = answerInputField.text;
        missionManager.CompleteMission(currentMission, answer);
        gameObject.SetActive(false);
    }
}
