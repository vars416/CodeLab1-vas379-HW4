using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //make instance of this variable

    private const string PLAY_PREF_KEY_HS = "High Score"; //Player preferences key
    private const string FILE_HS = "/CodeLab1-highscore.txt"; //highscore file

    public float timer = 30;
    public Text TimerText;

    public int Timer
    {
        get
        {
            return (int)timer;
        }

        set
        {
            timer = value; //
/*            if (timer > highscore) //if timer is higher than highscore
            {
                Highscore = (int)timer;
            }*/
        }
    }

    public int highscore = 10;

    public int Highscore
    {
        get
        {
            return highscore;
        }

        set
        {
            highscore = value;
            //PlayerPrefs.SetInt(PLAY_PREF_KEY_HS, highscore);
            File.WriteAllText(Application.dataPath + FILE_HS, highscore + ""); //write Highscore on to a file
        }
    }

    public List<string> highScoreNames; //list of high score names
    public List<float> highScoreNums;  //list of high score values

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; //set instance to this object
            DontDestroyOnLoad(gameObject); //Dont Destroy this object when you load a new scene
        }
        else
        { //if the instance is already set to an object
            Destroy(gameObject); //destroy this new object, so there is only ever one
        }
    }

    public void changescene(string stringname)
    {
        SceneManager.LoadScene(stringname); //scene changer
    }

    public void TimerReset()
    {
        //instance.TimerText.enabled = true;
        instance.timer = 30; //reset to 30
        instance.TimerText.text = "Time: " + (int)timer; //show timer
    }

    // Start is called before the first frame update
    void Start()
    {
        highScoreNames = new List<string>(); //init highScoreNames
        highScoreNums = new List<float>();  //init highScoreNums

        TimerText = GetComponentInChildren<Text>();

        if (File.Exists(Application.dataPath + FILE_HS)) //check if the file exists
        {
            string hsString = File.ReadAllText(Application.dataPath + FILE_HS); //read the text from highscore file
            print(hsString); //print highscore
            string[] scorePairs = hsString.Split('\n'); //split it on the newline, making each space in the array a line in the file

            for (int i = 0; i < 10; i++)
            { //loop through the 10 scores
                string[] nameScores = scorePairs[i].Split(' '); //split each line on the space
                highScoreNames.Add(nameScores[0]); //the first part of the split is the name
                highScoreNums.Add(float.Parse(nameScores[1])); //the second part is the value
            }
        }
        else //if the high score file doesn't exist
        {
            for (int i = 0; i < 10; i++) //create a new default high score list
            {
                highScoreNames.Add("VAR");
                highScoreNums.Add(100 + i * 10);
            }
        }

        Debug.Log(Application.dataPath);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; //increase the timer by deltaTime every frame
        TimerText.text = "Time: " + (int)timer + " High Score: " + highscore; //update the text component with the score and time

/*        if (timer <= 0)
        {
            //SceneManager.LoadScene("LoseScreen");
            TimerText.text = "Game Over";
        }*/
    }

    public void HighScoreUpdate()
    {

        bool newScore = false; //by default, we don't have new high score

        for (int i = 0; i < highScoreNums.Count; i++)
        { //go through all the high scores
            if (highScoreNums[i] > instance.timer)
            { //if we have a time that is lower than one of the high scores
                highScoreNums.Insert(i, instance.timer); //insert this new score into the value list
                highScoreNames.Insert(i, "VAR"); //give it the name "VAR"
                print(highScoreNames); //print names
                newScore = true; //new high sccore
                break; //stop the for loop
            }
        }

        if (newScore)
        { //if we have a new high score
            highScoreNums.RemoveAt(highScoreNums.Count - 1); //stop high score from increasing more than 10
            highScoreNames.RemoveAt(10);
        }

        string HSString = " "; //create a new string to insert into the file

        for (int i = 0; i < highScoreNames.Count; i++)
        { //Go through all the high scores
            HSString += highScoreNames[i] + "" + highScoreNums[i] + "\n"; //build a string for all the high scores in the lists
        }

        File.WriteAllText(Application.dataPath + FILE_HS, HSString); //save the list to the file
    }

    //reset the important values when the game restarts
    public void Reset()
    {
        instance.timer = 30; //reset time to 30
        /*score = 0;
        PrizeScript.currentLevel = 0;*/
    }
}
