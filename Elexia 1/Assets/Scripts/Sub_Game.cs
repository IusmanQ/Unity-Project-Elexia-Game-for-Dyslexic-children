using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using MongoDB.Driver;
using MongoDB.Bson;

public class Sub_Game : MonoBehaviour
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
    public Sprite[] substitutionImages;
    public static Sprite replayImage;

    public Sub_Letter[] letters;
    public Sub_slot[] slots;
    public Sub_slot tmp_slot;
    public List<Transform> positions;
    public static int score;
    public static int totalScoreSub;
    public static int target;
    public int randomWordNumber;

    public static float scorePercent;
    public static int gameLevelSubstitution;
    public static bool IncrementCoinsGems;
    public static int coins;
    public static int gems;
    public Text countDownText;
    public string status;

    public static bool substitutionActive = false;
    public Image imageGameObject;

    public Image substitutionImageObj;

    public List<Vector2> substitutePositionsList;

    MongoClient client = new MongoClient("mongodb+srv://usman:usman123@cluster0.qzcrb.mongodb.net/<dbname>?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    BsonDocument Document;

    //public Text coinsText;
    //public Text gemsText;
    //public Text countDownText;

    //Scenes
    public string voiceScreen = "Voice";
    public string scoreSceen = "Score_Screen";
    public string levelFailedScreen = "LevelFailedScreen";


    public static Sub_Game inst;
    void Awake()
    {
        inst = this;
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

        substitutionActive = true;
        Game.InsertionActive = false;
        Omi_Game.omissionActive = false;

        Player.Load();
        //Player.Delete();

        //if game is substitution and total score is good, change the background image.

        IncrementCoinsGems = true;
        if (isReplay == true) //If player wants to replay a word
        {
            if (ScoresUIManager.rWord != null)
            {
                word = ScoresUIManager.rWord;
                replayImage = ScoresUIManager.replayImage;
            }
            if (FailedScreenManager.rWord != null)
            {
                word = FailedScreenManager.rWord;
            }

            Player.playerSubstitutionLevel = ScoresUIManager.levelSubstitution;
            IncrementCoinsGems = false;
            isReplay = false;
        }
        else
        {
            ReadFile();
            randomWordNumber = Random.Range(0, words.Count);
            word = words[randomWordNumber];  //Gets the random word from a range
            replayImage = imagesOfWords[randomWordNumber];
            replayWord = word;
            gameLevelSubstitution = Player.playerSubstitutionLevel;

        }
        imageGameObject.gameObject.GetComponent<Image>().sprite = replayImage;
        Debug.Log("word word: " + word);
        letters = new Sub_Letter[word.Length - 1];  //Get all the letters

        target = 1;
        if (Player.playerSubstitutionLevel == 1)
        {
            substitutionImageObj.gameObject.GetComponent<Image>().sprite = substitutionImages[1];
        }

        else if (Player.playerSubstitutionLevel == 2)
        {
            substitutionImageObj.gameObject.GetComponent<Image>().sprite = substitutionImages[2];
        }
        GenerateSubWord(2);
        GenerateSlots();
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
    void GenerateSubWord(int substituteWords = 0)
    {
        GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Slot");
        float slotSize = slotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        Vector2 substitutePos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2.1f, Screen.height / 4)) - new Vector3((slots.Length / 2f) * slotsGap  / 2 - ((slotsGap - letters.Length) / (slotsGap * 10f)), 0, 0);
       
        for(int i = 0; i < letters.Length; i++)
        {
            letters[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Sub_Letter"), substitutePos, Quaternion.Euler(0, 0, 0))).GetComponent<Sub_Letter>(); //GameObject.Instantiate(letterPrefab);
            letters[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Alphabets/" + word.ToUpper()[i]);
            //letters[i].GetComponent<BoxCollider2D>().enabled = false;
        }
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

    void GenerateSlots()
    {
        GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Sub_slot");
        float slotSize = slotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        slots = new Sub_slot[letters.Length];
        print("length of slots: " + slots.Length);
        Vector2 position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f , Screen.height / 4)) - new Vector3(slotSize * (slots.Length / 2f) - slotSize / 2 - ((slotsGap-1) / 2f), 0, 0);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Sub_slot"), position, Quaternion.identity)).GetComponent<Sub_slot>(); //GameObject.Instantiate(letterPrefab);
            position += new Vector2(slotSize + slotsGap, 0);
            if (!letters[i].GetComponent<BoxCollider2D>().enabled)
                slots[i].GetComponent<BoxCollider2D>().enabled = false;
            slots[i].name = "Slot" + i;
        }
        Vector2 tmp_position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2)) - new Vector3(0, 0, 0);
        //slots[slots.Length] = ((GameObject)Instantiate(Resources.Load("Prefabs/Sub_slot"), tmp_position, Quaternion.identity)).GetComponent<Sub_slot>();
        tmp_slot = ((GameObject)Instantiate(Resources.Load("Prefabs/temp_slot"), tmp_position, Quaternion.identity)).GetComponent<Sub_slot>();
    }

    void AssignLetters ()
    {
        List<int> randomList = new List<int>();   //  Declare list
        for (int n = 0; n < letters.Length; n++)    //  Populate list
            randomList.Add(n);

        for (int n = 0; n < 5; n++)  {  //  5 = shuffle 5 times
            int rand = Random.Range(0, letters.Length);
            int temp = randomList [rand]; // temporary save for adding at last
            randomList.RemoveAt(rand); // rempve from anywhere
            randomList.Add(temp); // add at end
        }

        for (int i = 0; i < letters.Length; i++)
            letters[i].AssignSlot(slots[randomList[i]]);
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
        totalScoreSub = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].letter != null && slots[i].letter.GetComponent<SpriteRenderer>().sprite.name == letters[i].GetComponent<SpriteRenderer>().sprite.name)
                score++;
        }
        scorePercent = 100f * score / target;
        //Debug.Log("end score = " + score + " " + scorePercent);
        if (scorePercent >= 65)
        {
            coins = (Player.playerSubstitutionLevel + 1) * (int)scorePercent;
            gems = (Player.playerSubstitutionLevel + 1) * (int)score;
            totalScoreSub = totalScoreSub + 1;
            Debug.Log("Total score: " + totalScoreSub);
            status = "Correct";
            SceneManager.LoadScene(scoreSceen);
            //SceneManager.LoadScene(voiceScreen);
        }
        else
        {
            // Debug.Log("You lost");
            coins = (Player.playerSubstitutionLevel + 1) * (int)scorePercent;
            gems = (Player.playerSubstitutionLevel + 1) * (int)score;
            status = "Incorrect";
            SceneManager.LoadScene(levelFailedScreen);
        }
        Player.Save(word, gems, coins, score, Game.totalScore, totalScoreSub, Omi_Game.totalScoreOmi, 20 - countdownTime, "Substitution");
        Document = new BsonDocument {
            { Player.playerId, new BsonArray { new BsonDocument {
            { "word", word }, { "score", score }, { "time", 20 - countdownTime }, { "GameType", "Substitution" }, { "status", status }
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

    public void pauseButton()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            if (pauseCanvas != null)
            {
                //pauseCanvas.SetActive(true);
            }
        }
    }
}
