using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public int columnLength, rowLength;
    public GameObject treasureChest;
    public float xStart, yStart;
    public float xSpace, ySpace;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonPressed(){
        for(int y = 0; y < columnLength * rowLength; y++){
            GameObject chestClone;
            chestClone = Instantiate(treasureChest, new Vector3(xStart + (xSpace*(y%columnLength)), yStart + (ySpace*(y/columnLength))), Quaternion.identity) as GameObject;
        }
    }
}
