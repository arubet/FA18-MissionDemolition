using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButtons;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Singshot";

    // Use this for initialization
    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();

    }

    // Update is called once per frame
    void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
    }
    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    void Update()
    {
        UpdateGUI();
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButtons.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButtons.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButtons.text = "Show Slingshot";
                break;
        }
    }
    public static void ShotsFired()
    {
        S.shotsTaken++;
    }

}