using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class FoodCollectorSettings : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public FoodCollectorArea[] listArea;

    public int totalScore;
    public Text scoreText;

    StatsRecorder m_Recorder;

    public int curNumOfAgents;

    public void Awake()
    {
        curNumOfAgents = 0;
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        m_Recorder = Academy.Instance.StatsRecorder;
    }

    void EnvironmentReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("food"));
        ClearObjects(GameObject.FindGameObjectsWithTag("badFood"));
        //ClearObjects(GameObject.FindGameObjectsWithTag("agent"));
        curNumOfAgents = 0;
        agents = GameObject.FindGameObjectsWithTag("agent");
        listArea = FindObjectsOfType<FoodCollectorArea>();
        foreach (var fa in listArea)
        {
            fa.ResetFoodArea(agents);
        }

        totalScore = 0;
    }

    void ClearObjects(GameObject[] objects)
    {
        foreach (var food in objects)
        {
            Destroy(food);
        }
    }

    public void Update()
    {
        scoreText.text = $"Score: {totalScore}";

        // Send stats via SideChannel so that they'll appear in TensorBoard.
        // These values get averaged every summary_frequency steps, so we don't
        // need to send every Update() call.
        if ((Time.frameCount % 100) == 0)
        {
            m_Recorder.Add("TotalScore", totalScore);
        }

        
    }
}
