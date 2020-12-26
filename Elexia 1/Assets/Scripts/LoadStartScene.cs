using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStartScene : MonoBehaviour
{

    public string sceneName = "Home";
    public float load_scene_time = 2F;

    [SerializeField]
    private Image loading_bar_progress;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSomeScene());
    }

    // Update is called once per frame
    void Update()
    {
        fill_Loading_Bar();
    }

    public void Load_Scene(string sreenName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //function that fills the loading bar
    public void fill_Loading_Bar()
    {
        loading_bar_progress.fillAmount += 1.0f / load_scene_time * Time.deltaTime;
    }

    IEnumerator LoadSomeScene()
    {
        yield return new WaitForSeconds(load_scene_time);
        Load_Scene(sceneName);
    }

   
}
