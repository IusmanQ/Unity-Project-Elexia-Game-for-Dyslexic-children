using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;

public class Omi_Game : MonoBehaviour
{
    List<string> words = new List<string>();
    public static string word;
    private float slotsGap = 0.2f;
    private float lettersRadius = 1.5f;
    private int countdownTime;
    private bool isPaused = false;

    public static string replayWord;
    public static string wordImage;
    public static bool isReplay = false;
    public GameObject letterPrefab;
    public static AudioSource letterPickAudio;
    public GameObject pauseCanvas;
    public Sprite[] imagesOfWords;
    public Sprite[] omissionImages;
    public static Sprite replayImage;

    public Omi_Letter[] letters;
    public Omi_slot[] slots;
    public Omi_slot[] updatedslots;

    public Omi_slot tmp_slot;
    public List<Transform> positions;
    public static int score;
    public static int totalScoreOmi;
    public static int target;

    public static float scorePercent;
    public static int gameLevelOmission;
    public static bool IncrementCoinsGems;
    public static int coins;
    public static int gems;

    public Omi_Letter[] newLetters;
    List<string> alphabets;
    public List<string> wordList;
    public string randomAlphabet;
    public int randomWordNumber;
    public string status;

    private int level;
    public Text countDownText;

    public static bool omissionActive = false;
    public Image imageGameObject;

    public Image omissionImageObj;

    MongoClient client = new MongoClient("mongodb+srv://usman:usman123@cluster0.qzcrb.mongodb.net/<dbname>?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    BsonDocument Document;

    public List<Vector2> substitutePositionsList;

    //public Text coinsText;
    //public Text gemsText;
    //public Text countDownText;

    //Scenes
    public string voiceScreen = "Voice";
    public string scoreSceen = "Score_Screen";
    public string levelFailedScreen = "LevelFailedScreen";

    public static Omi_Game inst;
    void Awake()
    {
        inst = this;
        level = 1;
        letterPickAudio = GetComponent<AudioSource>();
        letterPickAudio.Stop();
    }
    //void OnGUI()
    //{
    //    GUILayout.Label("temp : " + tmp_slot.over);
    //    for (int i = 0; i < slots.Length; i++)
    //        GUILayout.Label("slots" + i + " : " + slots[i].over);
    //    for (int i = 0; i < letters.Length; i++)
    //        GUILayout.Label("letters" + i + " : " + letters[i].selected);
    //}

    void Start()
    {

        database = client.GetDatabase("PlayersData");
        collection = database.GetCollection<BsonDocument>("Scores Information");

        omissionActive = true;
        Game.InsertionActive = false;
        Sub_Game.substitutionActive = false;

        Player.Load();
        //Player.Delete();

        //if game is omission and total score is good, change the background image.


        IncrementCoinsGems = true;
        wordList = new List<string>();
        alphabets = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        if (isReplay == true) //If player wants to replay a word
        {
            if(ScoresUIManager.rWord != null)
            {
                word = ScoresUIManager.rWord;
                replayImage = ScoresUIManager.replayImage;
            } 
            if(FailedScreenManager.rWord != null)
            {
                word = FailedScreenManager.rWord;
            }
            Player.playerOmissionLevel = ScoresUIManager.levelOmission;
            IncrementCoinsGems = false;
            isReplay = false;
        }
        else
        {
            ReadFile();
            randomWordNumber = Random.Range(0, words.Count);
            word = words[randomWordNumber];  //Gets the random word from a range 

            //for (int i = 0; i < wordList.Count; i++)
            //{
            //    print("wordlist: " + wordList[i]);
            //}
            replayImage = imagesOfWords[randomWordNumber];

            replayWord = word;
            gameLevelOmission = Player.playerOmissionLevel;

        }
        imageGameObject.gameObject.GetComponent<Image>().sprite = replayImage;
        for (int i = 0; i < word.Length; i++)
        {
            wordList.Add(word[i].ToString().ToUpper());
        }
        Debug.Log("word word: " + word);
        letters = new Omi_Letter[word.Length - 1];  //Get all the letters

        if (Player.playerOmissionLevel == 1)
        {
            omissionImageObj.gameObject.GetComponent<Image>().sprite = omissionImages[1];
        }

        else if (Player.playerSubstitutionLevel == 2)
        {
            omissionImageObj.gameObject.GetComponent<Image>().sprite = omissionImages[2];
        }

        GenerateOmiWord(level);
        GenerateOmiSlots(level);
        updatedslots = new Omi_slot[slots.Length - 1];
        target = updatedslots.Length;
        AssignLetters();
        
        countdownTime = 20;  //set the countdown timer
        //assigning after slots and letter created
    }

    void ReadFile()
    {
        TextAsset wordStr = Resources.Load("Words") as TextAsset; //Resources.Load<TextAsset>("Words")
        string[] arr = wordStr.text.Split('\n');
        words.AddRange(arr);
        words.RemoveAt(words.Count - 1);
    }

    //substitutes given letter from the word and displays it
    void GenerateOmiWord(int substituteWords = 0)
    {
        int randomIndex = Random.Range(0, letters.Length + 1);
        newLetters = new Omi_Letter[letters.Length + level];
        print("total alphabets: " + alphabets.Count);
        int randomAlphabetIndex = Random.Range(0, alphabets.Count - 1);
        randomAlphabet = alphabets[randomAlphabetIndex];
        print("alphabet: " + randomAlphabet);
        GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Slot");
        float slotSize = slotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        int checkIndex = 0;

        Vector2 substitutePos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 4)) - new Vector3((slots.Length / 2f) * slotsGap / 2 - ((slotsGap - letters.Length) / (slotsGap * 10f)), 0, 0);

        for (int i = 0; i< newLetters.Length; i++)
        {
            if (i != randomIndex)
            {
                newLetters[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Omi_Letter"), substitutePos, Quaternion.Euler(0, 0, 0))).GetComponent<Omi_Letter>(); //GameObject.Instantiate(letterPrefab);
                newLetters[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Alphabets/" + word.ToUpper()[i - checkIndex]);
                print("word that is not printing:" + word.ToUpper()[i]);
            }
            else
            {
                print("random index is: " + randomIndex);
                newLetters[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Omi_Letter"), substitutePos, Quaternion.Euler(0, 0, 0))).GetComponent<Omi_Letter>();
                newLetters[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Alphabets/" + randomAlphabet);
                print("random letter: " + newLetters[i].gameObject.GetComponent<SpriteRenderer>().sprite.name);
                if (i >= 0)
                {
                    checkIndex++;
                }
            }
            substitutePos += new Vector2(slotsGap + slotSize, 0);

        }
        print(newLetters.Length);
        print(letters.Length);
    }       



    bool checkletterSubtitute(int index, int[] subtituteIndexs)
    {
        for (int i = 0; i < subtituteIndexs.Length; i++)
        {
            if (index == subtituteIndexs[i])
                return true;
        }
        return false;
    }

    void GenerateOmiSlots(int levelOmission)
    {
        GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Omi_slot");
        float slotSize = slotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        slots = new Omi_slot[letters.Length + level];
        print("length of slots: " + slots.Length);
        Vector2 position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 4)) - new Vector3(slotSize * (slots.Length / 2f) - slotSize / 2 - ((slotsGap - 1) / 2f), 0, 0);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Omi_slot"), position, Quaternion.identity)).GetComponent<Omi_slot>(); //GameObject.Instantiate(letterPrefab);
            position += new Vector2(slotSize + slotsGap, 0);
            if (!newLetters[i].GetComponent<BoxCollider2D>().enabled)
                slots[i].GetComponent<BoxCollider2D>().enabled = false;
            slots[i].name = "Slot" + i;
        }
        Vector2 tmp_position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2)) - new Vector3(0, 0, 0);
        //slots[slots.Length] = ((GameObject)Instantiate(Resources.Load("Prefabs/Sub_slot"), tmp_position, Quaternion.identity)).GetComponent<Sub_slot>();
        tmp_slot = ((GameObject)Instantiate(Resources.Load("Prefabs/Omi_temp_slot"), tmp_position, Quaternion.identity)).GetComponent<Omi_slot>();
    }

    public void AssignLetters()
    {
        for (int i = 0; i < newLetters.Length; i++)
            newLetters[i].AssignSlot(slots[i]);
    }

    Vector2 GetRandomPosition()
    {
        Vector2 extent = letterPrefab.GetComponent<SpriteRenderer>().bounds.extents;
        Vector2 position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        return new Vector2(Random.Range((-position.x + extent.x) * 10, (position.x - extent.x) * 10) / 10f, Random.Range(-position.y * 10 * .7f, position.y * 10 * .7f) / 10f);
    }

    float second;
    void Update()
    {
        if (Time.time > second)
        {
            second = Time.time + 1;
            countdownTime--;
            if (countdownTime >= 0)
                displayCounterText();
            else
                checkGameCondition();
        }
    }

    void displayCounterText()
    {
        countDownText.text = countdownTime.ToString("00");
    }

    public void checkGameCondition()
    {
        score = 0;
        totalScoreOmi = 0;
        for (int i = 0; i < updatedslots.Length; i++)
        {
            if (updatedslots[i] != null && updatedslots[i].letter.GetComponent<SpriteRenderer>().sprite.name == wordList[i])
            {
                score++;
                print("win");
            }
        }
        scorePercent = 100f * score / target;
        //Debug.Log("end score = " + score + " " + scorePercent);
        if (score == updatedslots.Length)
        {
            coins = (Player.playerOmissionLevel + 1) * (int)scorePercent;
            gems = (Player.playerOmissionLevel + 1) * (int)score;
            totalScoreOmi = totalScoreOmi + 1;
            status = "Correct";
            Debug.Log("Total score: " + totalScoreOmi);
            //SceneManager.LoadScene(voiceScreen);
            SceneManager.LoadScene(scoreSceen);
        }
        else
        {
            // Debug.Log("You lost");
            coins = (Player.playerOmissionLevel + 1) * (int)scorePercent;
            gems = (Player.playerOmissionLevel + 1) * (int)score;
            status = "Incorrect";
            SceneManager.LoadScene(levelFailedScreen);
        }
        Player.Save(word, gems, coins, score, Game.totalScore, Sub_Game.totalScoreSub ,totalScoreOmi, 20 - countdownTime, "Omission");
        Document = new BsonDocument {
            { Player.playerId, new BsonArray { new BsonDocument {
            { "word", word }, { "score", score }, { "time", 20 - countdownTime }, { "GameType", "Omission" }, { "status", status }
                                    }
                                                                 }
            }
            };
        //collection.InsertOne(Document);

        
    }

    //public void DisplayTotalCoinsGems()
    //{
    //    if (coinsText != null)
    //    {
    //        coinsText.text = Player.playerTotalCoins.ToString("0000");
    //    }
    //    if (gemsText != null)
    //    {
    //        gemsText.text = Player.playerTotalGems.ToString("0000");
    //    }
    //}

    //public void pauseButton()
    //{
    //    if (isPaused)
    //    {
    //        Time.timeScale = 1;
    //        isPaused = false;
    //    }
    //    else
    //    {
    //        Time.timeScale = 0;
    //        isPaused = true;
    //        if (pauseCanvas != null)
    //        {
    //            //pauseCanvas.SetActive(true);
    //        }
    //    }
    //}
}
