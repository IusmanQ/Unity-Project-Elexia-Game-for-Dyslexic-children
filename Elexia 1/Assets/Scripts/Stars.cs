using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{

    private int starCount;
    public Vector3 starsPos;
    private float starsGap=0.5f;

    // Start is called before the first frame update
    void Start()
    {
        DetermineStarsBasedOnPercent();
        GenerateStars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DetermineStarsBasedOnPercent()
    {
        if (Game.scorePercent >= 90)
        {
            starCount = 3;
        }
        else if (Game.scorePercent >= 65 && Game.scorePercent < 90)
        {
            starCount = 2;
        }
        else if(Game.scorePercent < 65)
        {
            starCount = 1;
        }
    }

    void GenerateStars()
    {
        GameObject starPrefab = (GameObject)Resources.Load("Prefabs/Star");
        float starSize = starPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log("stars: " + starCount);
        Vector2 starsPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2.25f, Screen.height / 1.6f), 0);
        for (int i = 1; i <= starCount; i++)
        {
            Debug.Log("Star Generated");
            (GameObject.Instantiate(Resources.Load("Prefabs/Star"), starsPos, Quaternion.identity) as GameObject).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Stars/" + i);
            starsPos += new Vector2(starSize + starsGap, 0);
        }
    }
}
