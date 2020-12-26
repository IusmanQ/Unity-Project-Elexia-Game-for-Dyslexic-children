using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Narrative_Manager : MonoBehaviour
{
    //private string omissionGameScene = "Home";
    private string insertionGameScene = "Scene";
    private string substitutionGameScene = "sub";
    private string omissionGameScene = "Omission";
    public static Narrative_Manager inst;
    public GameObject fingerAvatar;
    

    public bool nextButtonBool = false;
    public float waitTime = 100F;

    public Canvas startCanvas;
    public Canvas playCanvas;
    public Canvas eatCanvas;
    public Canvas dieCanvas;
    public Canvas wizardCanvas1;
    public Canvas wizardCanvas2;
    public Canvas doorsCanvas;

    public Canvas insertionHint;
    public Canvas substitutionHint;
    public Canvas omissionHint;

    public float load_canvas_time = 2F;


    void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        LoadStartScene();
        if (nextButtonBool == false)
        {  
            Invoke("LoadPlayScene", 6f);
            Invoke("LoadEatScene", 12f);
            Invoke("LoadDieScene", 18f);
            Invoke("LoadWizard1Scene", 24f);
            Invoke("LoadWizard2Scene", 30f);
            Invoke("LoadDoorsScene", 36f);
        }

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (fingerAvatar.gameObject != null)
        {
            fingerAvatar.gameObject.transform.position = new Vector2(cursorPosition.x, cursorPosition.y);
        }
    }

    public void LoadInsertionScene()
    {
        insertionHint.gameObject.SetActive(true);
        StartCoroutine(waitForInsertion());
        
    }

    public void LoadSubstitutionScene()
    {
        substitutionHint.gameObject.SetActive(true);
        StartCoroutine(waitForSubstitution());
        
    }

    public void LoadOmissionScene()
    {
        omissionHint.gameObject.SetActive(true);
        StartCoroutine(waitForOmission());
        
    }
    void OnMouseDrag()
    {
        fingerAvatar.gameObject.SetActive(true);
        fingerAvatar.gameObject.GetComponent<Image>().transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("hit");
    }

    //void OnTriggerExit2D(Collider2D col)
    //{
    //    this.over = false;
    //}



    public void LoadStartScene()
    {
        startCanvas.gameObject.SetActive(true);
    }
    public void LoadPlayScene()
    {
        startCanvas.gameObject.SetActive(false);
        playCanvas.gameObject.SetActive(true);
    }

    public void LoadEatScene()
    {
        playCanvas.gameObject.SetActive(false);
        eatCanvas.gameObject.SetActive(true);
    }
    public void LoadDieScene()
    {
        eatCanvas.gameObject.SetActive(false);
        dieCanvas.gameObject.SetActive(true);
    }
    public void LoadWizard1Scene()
    {
        dieCanvas.gameObject.SetActive(false);
        wizardCanvas1.gameObject.SetActive(true);
    }
    public void LoadWizard2Scene()
    {
        wizardCanvas1.gameObject.SetActive(false);
        wizardCanvas2.gameObject.SetActive(true);
    }
    public void LoadDoorsScene()
    {
        wizardCanvas2.gameObject.SetActive(false);
        doorsCanvas.gameObject.SetActive(true);
    }

    public void LoadPlayButton()
    {
        nextButtonBool = true;
        startCanvas.gameObject.SetActive(false);
        playCanvas.gameObject.SetActive(true);
        nextButtonBool = false;
        CancelInvoke();
        Invoke("LoadEatScene", 6f);
    }

    public void LoadEatButton()
    {
        nextButtonBool = true;
        playCanvas.gameObject.SetActive(false);
        eatCanvas.gameObject.SetActive(true);
        nextButtonBool = false;
        CancelInvoke();
        Invoke("LoadDieScene", 6f);
    }
    public void LoadDieButton()
    {
        nextButtonBool = true;
        eatCanvas.gameObject.SetActive(false);
        dieCanvas.gameObject.SetActive(true);
        nextButtonBool = false;
        CancelInvoke();
        Invoke("LoadWizard1Scene", 6f);
    }
    public void LoadWizard1Button()
    {
        nextButtonBool = true;
        dieCanvas.gameObject.SetActive(false);
        wizardCanvas1.gameObject.SetActive(true);
        nextButtonBool = false;
        CancelInvoke();
        Invoke("LoadWizard2Scene", 6f);
    }
    public void LoadWizard2Button()
    {
        nextButtonBool = true;
        wizardCanvas1.gameObject.SetActive(false);
        wizardCanvas2.gameObject.SetActive(true);
        nextButtonBool = false;
        CancelInvoke();
        Invoke("LoadDoorsScene", 6f);
    }
    public void LoadDoorsButton()
    {
        nextButtonBool = true;
        wizardCanvas2.gameObject.SetActive(false);
        doorsCanvas.gameObject.SetActive(true);
        nextButtonBool = false;
        CancelInvoke();

    }

    public void SkipButton()
    {
        CancelInvoke();
        startCanvas.gameObject.SetActive(false);
        playCanvas.gameObject.SetActive(false);
        eatCanvas.gameObject.SetActive(false);
        dieCanvas.gameObject.SetActive(false);
        wizardCanvas1.gameObject.SetActive(false);
        wizardCanvas2.gameObject.SetActive(false);
   
        doorsCanvas.gameObject.SetActive(true);
    }

    IEnumerator waitForInsertion()
    {
        
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(insertionGameScene);

    }
    IEnumerator waitForSubstitution()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(substitutionGameScene);
    }
    IEnumerator waitForOmission()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(omissionGameScene);
    }
}
