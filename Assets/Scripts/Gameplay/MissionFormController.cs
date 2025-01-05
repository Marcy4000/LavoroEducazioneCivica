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
    public ClickableLink link;

    private MissionData currentMission;
    private MissionManager missionManager;

    private void Start()
    {
        missionManager = FindObjectOfType<MissionManager>();

        gameObject.SetActive(false);

        link.OnClick += () =>
        {
            if (string.IsNullOrWhiteSpace(currentMission.link))
            {
                return;
            }
            Application.OpenURL(currentMission.link);
        };
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
        bool vaild = missionManager.CompleteMission(currentMission, answer);
        if (vaild)
        {
            gameObject.SetActive(false);
        }

    }
}
