using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public ManagerScript manSc;
    public SpriteRenderer sr;
    public Sprite hasMoney;
    public Sprite isEmpty;
    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        manSc = GameObject.Find("Manager").GetComponent<ManagerScript>();
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown(){
        sr.sprite = hasMoney;
        if(isOpen == false){
            manSc.theScore.openNextChest();
            isOpen = true;
        }else if(isOpen == true){
            //do nothing
        }

        if(manSc.theScore.isPooper()){
            sr.sprite = isEmpty;
            manSc.theScore.endRound();
            manSc.DestroyAllBoxes();
        }

        manSc.SetTheText();
        }
    }
