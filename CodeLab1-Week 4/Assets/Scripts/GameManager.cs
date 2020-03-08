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
        { //instance hasn't been set yet
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
        SceneManager.LoadScene(stringname);
    }

    public void TimerReset()
    {
        //instance.TimerText.enabled = true;
        instance.timer = 30;
        instance.TimerText.text = "Time: " + (int)timer;
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
        }

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

    public void UpdateHighScores()
    {

        bool newScore = false; //by default, we don't have new high score

        for (int i = 0; i < highScoreNums.Count; i++)
        { //go through all the high scores
            if (highScoreNums[i] > instance.timer)
            { //if we have a time that is lower than one of the high scores
                highScoreNums.Insert(i, instance.timer); //insert this new score into the value list
                highScoreNames.Insert(i, "VAR"); //give it the name "VAR"
                print(highScoreNames);
                newScore = true; //new high sccore
                break; //stop the for loop
            }
        }

        if (newScore)
        { //if we have a new high score
            highScoreNums.RemoveAt(highScoreNums.Count - 1); //remove the final high score value so we are back down to 10
            highScoreNames.RemoveAt(10);
        }

        string fileContents = ""; //create a new string to insert into the file

        for (int i = 0; i < highScoreNames.Count; i++)
        { //loop through all the high scores
            fileContents += highScoreNames[i] + " " + highScoreNums[i] + "\n"; //build a string for all the high scores in the lists
        }

        File.WriteAllText(Application.dataPath + FILE_HS, fileContents); //save the list to the file
    }

    //reset the important values when the game restarts
    public void Reset()
    {
        instance.timer = 30;
        /*score = 0;
        PrizeScript.currentLevel = 0;*/
    }
}
