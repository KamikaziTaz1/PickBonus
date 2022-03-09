using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Increments : MonoBehaviour
{
    //INCREMENTS FOR DENOMINATIONS
    public Button PlayBut;
    public GameObject Manager;
    public ManagerScript managerScript;
    public Score theScore = new Score();
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("Manager");
        managerScript = Manager.GetComponent<ManagerScript>();
        PlayBut = GameObject.Find("PlayButton").GetComponent<Button>(); //get button component

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseBy25(){
        managerScript.addToBet(0.25f);
    }

    public void IncreaseBy50(){
        managerScript.addToBet(0.50f);
    }

    public void IncreaseBy100(){
        managerScript.addToBet(1f);
    
    }

    public void IncreaseBy500(){
        managerScript.addToBet(5f);
    }

    public void DecreaseBy25(){
        managerScript.addToBet(-0.25f);
    }

    public void DecreaseBy50(){
        managerScript.addToBet(-0.50f);
    }

    public void DecreaseBy100(){
        managerScript.addToBet(-1f);
    }

    public void DecreaseBy500(){
        managerScript.addToBet(-5f);
    }
    
}
