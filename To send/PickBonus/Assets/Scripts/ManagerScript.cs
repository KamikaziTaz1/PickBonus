using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{

    // Private properties
    protected Button PlayBut;
    [SerializeField]protected List<Button> intervalButtons;    
    // public properties
    public Score theScore = new Score();
    // References to controls that do not disappear
    public GameObject GameOverButton;  
    public Text currentBalanceTxt;
    public Text lastGameWinTxt;
    public Text amountToBetTxt;
    public GameObject GameOverTxt;

    void Start(){
        PlayBut = GameObject.Find("PlayButton").GetComponent<Button>(); //get button component

        intervalButtons = new List<Button>();

        foreach( GameObject obj in GameObject.FindGameObjectsWithTag("IntervalButton") ){
            intervalButtons.Add( obj.GetComponent<Button>());
        }

        setIntervalButtonStatus(true);

        theScore = new Score();//score reference
        SetTheText();

        GameOverTxt.SetActive(false);
        GameOverButton.SetActive(false);
   
    }

    // Update is called once per frame
    void Update(){
  
    }

    private void setIntervalButtonStatus(bool newStatus){
        foreach(Button btn in intervalButtons){
            btn.interactable = newStatus;
        }
    }

    public void addToBet( float diff ){
        theScore.addToAmountToBet( diff );
        SetTheText();
        if(theScore.getBalance() < theScore.getAmountToBet()){
            PlayBut.interactable = false;
        }else if(theScore.getBalance() >= theScore.getAmountToBet()){
            PlayBut.interactable = true; 
        }
    }

    public void startPlay(){
        theScore.startPlay();
        currentBalanceTxt.text = "Current Balance: " + theScore.getBalance().ToString("00.00");
        lastGameWinTxt.text = "Last Game Win: " + theScore.getCurrentGameWin().ToString("00.00");
        amountToBetTxt.text = "Amount To Bet: " + theScore.getAmountToBet().ToString("00.00");
        PlayBut.interactable = false;
        
        PlayBut.interactable = false;
        setIntervalButtonStatus(false);
    }

    public void SetTheText()
    {
        currentBalanceTxt.text = "Current Balance: " + theScore.getBalance().ToString();
        lastGameWinTxt.text = "Last Game Win: " + theScore.getCurrentGameWin().ToString();
        amountToBetTxt.text = "Amount To Bet: " + theScore.getAmountToBet().ToString();
    }

    public void DestroyAllBoxes(){
        foreach(var gameObj in GameObject.FindGameObjectsWithTag("chestOfTreasure")){
            Destroy(gameObj, .9f);
            GameOverTxt.SetActive(true);
            GameOverButton.SetActive(true);
        }
    }

    public void StartNewRound(){
        GameOverTxt.SetActive(false);
        GameOverButton.SetActive(false);
        PlayBut.interactable = true;
        setIntervalButtonStatus(true);
    }
}