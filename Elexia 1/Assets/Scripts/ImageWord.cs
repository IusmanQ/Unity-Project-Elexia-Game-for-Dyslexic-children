using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageWord : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GenerateImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateImage()
    {
        GameObject imagePrefab = (GameObject)Resources.Load("Prefabs/Image");
        float imageSize = imagePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log("word(image): " + Game.word);
        Debug.Log("Images/" + Game.word.ToUpper());
        GameObject img = (GameObject.Instantiate(Resources.Load("Prefabs/Image"), transform.position, Quaternion.identity) as GameObject);
        img.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/" + Game.word.ToUpper());
    }
}
