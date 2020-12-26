using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.ComponentModel.Design;
using System.Reflection;
using MongoDB.Driver;
using MongoDB.Bson;

public class Game : MonoBehaviour
{
    List<string> words = new List<string>();
    public static string word;
    private float slotsGap = 0.2f;
    private float lettersRadius = 1.7f;
    private int countdownTime;
    private bool isPaused = false;
    public Sprite[] imagesOfWords;
    public Sprite[] insertionImages;
    public static Sprite replayImage;

    public static string replayWord;
    public static string wordImage;
    public static bool isReplay=false;
    public GameObject letterPrefab;
    public static AudioSource letterPickAudio;
    public GameObject pauseCanvas;

    public Letter[] letters;
    public Slot[] slots;
    public List<Transform> positions;
    public static int score;
    public static int totalScore;
    public static int target;
  
    public static float scorePercent;
    public static int gameLevelInsertion;
    public static bool IncrementCoinsGems;
    public static int coins;
    public static int gems;
    public string status;

    int[] subtituteIndexs;

    public Text coinsText;
    public Text gemsText;
    public Text countDownText;

    public static bool InsertionActive = false;

    public Image imageGameObject;
    public int randomWordNumber;

    public Image insertionImageObj;

    //Scenes
    public string voiceScreen = "Voice";
    public string scoreScreen = "Score_Screen";
    public string levelFailedScreen = "LevelFailedScreen";

    [SerializeField]
    public GameObject Settings_Menu;

    [SerializeField]
    Toggle VolumeToggle;

    [SerializeField]
    Toggle MusicToggle;

    [SerializeField]
    public Image Volume_handler;

    [SerializeField]
    public Image Music_handler;

    BsonDocument Document;
    public AudioSource Audio;

    MongoClient client = new MongoClient("mongodb+srv://usman:usman123@cluster0.qzcrb.mongodb.net/<dbname>?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    public static Game inst;
    void Awake()
    {
        inst = this;
        letterPickAudio = GetComponent<AudioSource>();
        letterPickAudio.Stop();
    }

    void Start() {

        database = client.GetDatabase("PlayersData");
        collection = database.GetCollection<BsonDocument>("Scores Information");
        print("chair image name: "+imagesOfWords[0].name);
        Audio = GetComponent<AudioSource>();

        InsertionActive = true;
        Sub_Game.substitutionActive = false;
        Omi_Game.omissionActive = false;

        Player.Load();
        //Player.Delete();

        //if game is insertion and total score is good, change the background image.

        IncrementCoinsGems = true;
        if (isReplay == true) //If player wants to replay a word
        {
            if (ScoresUIManager.rWord != null)
            {
                word = ScoresUIManager.rWord;
                replayImage = ScoresUIManager.replayImage;
                Player.playerInsertionLevel = ScoresUIManager.levelInsertion;
            }
            if (FailedScreenManager.rWord != null)
            {
                word = FailedScreenManager.rWord;
            }
            
            IncrementCoinsGems = false;
            isReplay = false;
        }
        else
        {
            ReadFile();
            randomWordNumber = Random.Range(0, words.Count);
            word = words[randomWordNumber];  //Gets the random word from a range
            replayWord = word;
            replayImage = imagesOfWords[randomWordNumber];
            gameLevelInsertion = Player.playerInsertionLevel;

        }
        imageGameObject.gameObject.GetComponent<Image>().sprite = replayImage;
        Debug.Log("word word: " + word);
        wordImage = word;
        DisplayTotalCoinsGems();   
        letters = new Letter[word.Length - 1];  //Get all the letters

        if (Player.playerInsertionLevel == 0)
        {
            target = 1;
            GenerateWordCircular(letters.Length - 1);
        }
        else if(Player.playerInsertionLevel == 1)
        {
            target = 2;
            insertionImageObj.gameObject.GetComponent<Image>().sprite = insertionImages[1];
            GenerateWordCircular(letters.Length - 2);
        }
        else if (Player.playerInsertionLevel == 2)
        {
            target = letters.Length;
            insertionImageObj.gameObject.GetComponent<Image>().sprite = insertionImages[2];
            GenerateWordCircular();
        }
        GenerateSlots();  //Generate the slots
        AssignLetters();
        countdownTime = 20;  //set the countdown timer
        isReplay = false;
    }

    void ReadFile () {
        TextAsset wordStr = Resources.Load("Words") as TextAsset; //Resources.Load<TextAsset>("Words")
        string[] arr = wordStr.text.Split('\n');
        words.AddRange(arr);
        words.RemoveAt(words.Count - 1);
       // for (int i = 0; i < words.Count; i++)
           // print(words[i]);
    }

    //substitutes given letter from the word and displays it
    void GenerateWordCircular(int substituteWords = 0)
    {
        List<int> randomList = new List<int>();   //  Declare list
        for (int n = 0; n < word.Length; n++)    //  Populate list    if issue, write 'word.Length-1'
        {
            randomList.Add(n);
        }

        subtituteIndexs = new int[substituteWords];
        for (int i = 0; i < substituteWords; i++) { 
            int i1 = Random.Range(0, randomList.Count - 1);    //  Pick random element from the list
            subtituteIndexs[i] = randomList[i1];    //  i = the number that was randomly picked
            randomList.RemoveAt(i1);   //  Remove chosen element
        }

        GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Slot");
        float slotSize = slotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        Vector2 substitutePos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 4)) - new Vector3((slots.Length / 2f) * slotsGap - 0.2f / 2 - ((slotsGap - letters.Length) / (slotsGap * 10f)), 0, 0);
        for (int i = 0; i < letters.Length; i++)
        {
            if (checkletterSubtitute (i, subtituteIndexs))
            {

                letters[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Letter"), substitutePos, Quaternion.Euler(0, 0, 0))).GetComponent<Letter>(); //GameObject.Instantiate(letterPrefab);
                letters[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Alphabets/" + word.ToUpper()[i]);
                letters[i].GetComponent<BoxCollider2D>().enabled = false;
                substitutePos += new Vector2(slotsGap + slotSize, 0);
            }
            else
            {
                float angle = (360 / letters.Length) * i;
                letters[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Letter"), Math.GetDirectionFromTheta(angle) * lettersRadius + Vector3.up, Quaternion.Euler(0, 0, 0))).GetComponent<Letter>(); //GameObject.Instantiate(letterPrefab);
                letters[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Alphabets/" + word.ToUpper()[i]);
                substitutePos += new Vector2(slotsGap + slotSize, 0);
            }
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

    void GenerateSlots() {
        GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Slot");
        float slotSize = slotPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        slots = new Slot[letters.Length];
        Vector2 position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/4)) - new Vector3 (slotSize * (slots.Length / 2f) - slotSize / 2 - ((slotsGap - 1) / 2f), 0, 0);
        for (int i = 0; i < slots.Length; i++) {
            slots[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Slot"), position, Quaternion.identity)).GetComponent<Slot>(); //GameObject.Instantiate(letterPrefab);
            position += new Vector2(slotSize + slotsGap, 0);
            if (!letters[i].GetComponent<BoxCollider2D>().enabled)
                slots[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void GenerateImage()
    {

    }

  

    void GenerateWordRandom() {
        word = words[Random.Range(0, words.Count)];
        print(word + " " + word.Length);
        letters = new Letter[word.Length - 1];
        for (int i = 0; i < letters.Length; i++) {
            int rand = Random.Range(0, positions.Count); // for get rand position
            letters[i] = ((GameObject)Instantiate(Resources.Load("Prefabs/Letter"), GetRandomPosition(), Quaternion.Euler(0, 0, 0))).GetComponent<Letter>(); //GameObject.Instantiate(letterPrefab);
            letters[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Alphabets/" + word.ToUpper()[i]);
            positions.RemoveAt(rand);
        }
    }

    public void AssignLetters()
    {
        for (int i = 0; i < letters.Length; i++)
        {
            if (checkletterSubtitute(i, subtituteIndexs))
            {
                letters[i].AssignSlot(slots[i]);
            }
        }
            
    }

    Vector2 GetRandomPosition () {
        Vector2 extent = letterPrefab.GetComponent<SpriteRenderer>().bounds.extents;
        Vector2 position = Camera.main.ScreenToWorldPoint(new Vector2 (Screen.width, Screen.height));
        return new Vector2(Random.Range((-position.x + extent.x) * 10, (position.x - extent.x) * 10) / 10f, Random.Range(-position.y * 10 * .7f, position.y * 10 * .7f) / 10f);
    }

    float second;
    void Update()
    {
        if (Time.time > second) {
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
        totalScore = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].letter != null && slots[i].letter.GetComponent<SpriteRenderer>().sprite.name == letters[i].GetComponent<SpriteRenderer>().sprite.name)
                score++;
        }
        scorePercent = 100f * score / target;
        //Debug.Log("end score = " + score + " " + scorePercent);
        if (scorePercent >= 65)
        {
            coins = (Player.playerInsertionLevel + 1) * (int)scorePercent;
            gems = (Player.playerInsertionLevel + 1) * (int)score;
            totalScore = totalScore + 1;
            status = "Correct";
            Debug.Log("Total score: " + totalScore);
            //SceneManager.LoadScene(voiceScreen);
            SceneManager.LoadScene(scoreScreen);
        }
        else
        {
            // Debug.Log("You lost");
            status = "Incorrect";
            coins = (Player.playerInsertionLevel + 1) * (int)scorePercent;
            gems = (Player.playerInsertionLevel + 1) * (int)score;
            SceneManager.LoadScene(levelFailedScreen);
        }
        Player.Save(word, gems, coins, score, totalScore, Sub_Game.totalScoreSub, Omi_Game.totalScoreOmi, 20 - countdownTime, "Insertion");
        Document = new BsonDocument {
            { Player.playerId, new BsonArray { new BsonDocument {
            { "word", word }, { "score", score }, { "time", 20 - countdownTime }, { "GameType", "Insertion" }, { "status", status }
                                    }
                                                                 }
            }
            };
        //collection.InsertOne(Document);

    }

    public void DisplayTotalCoinsGems()
    {
        if (coinsText != null)
        {
            coinsText.text = Player.playerTotalCoins.ToString("0000");
        }
        if(gemsText != null)
        {
            gemsText.text = Player.playerTotalGems.ToString("0000");
        }
    }
    
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
                //pauseCanvas.GetComponent<Canvas>().sortingOrder = 10;
            }
        }
    }

    public void SettingsButton()
    {
        Settings_Menu.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    public void VolumeOnOff_Button()
    {
        if (VolumeToggle.isOn)
        {
            //VOLUME IS ON
            Volume_handler.transform.localPosition = new Vector2(100.0f, -2.6f);
            Audio.Play();

        }
        else if (VolumeToggle.isOn == false)
        {
            //VOLUME IS OFF
            Volume_handler.transform.localPosition = new Vector2(12.0f, -2.6f);
            Audio.Pause();
        }
    }

    public void MusicOnOff_Button()
    {
        if (MusicToggle.isOn)
        {
            //Music IS ON
            Music_handler.transform.localPosition = new Vector2(100.0f, 0.24725f);
            Audio.Play();
        }
        else if (MusicToggle.isOn == false)
        {
            //Music IS OFF
            Music_handler.transform.localPosition = new Vector2(12.0f, 0.24725f);
            Audio.Pause();
        }
    }
}
