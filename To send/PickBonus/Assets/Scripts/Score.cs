using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Terminiology
    Round = the user hits play, keeps openiing chests till they reach the pooper
    Pooper = last chest opened with no money, ends round
    ChestAmount = amount in a chest
    Multiplier = from WinProbability, the mulitiplier selected for the current round. See 
    "amountToWin" for detaiils

    Storing as float internally to make math easier
    Using Float externally for ease of use in Unity
*/

public class Score
{
    /*
         Holds the amount the user starts with. 
         When the user hits play, balance =  balance - amount bet
         When the user hits the pooper, balance = balance + currentGameWin
    */
    private float balance ;

    /*
        At the start of the turn, set to zero
        During the round, each time the user opens a chest, the chestAount is
        added to this
    */
    private float currentGameWin ;

    /*
        The amount out of the current balance that the user is going to bet
        Cannot be > current balance
    */
    private float amountToBet ;


    /*
        This holds the master list of win probabilities across multiple rounds. It has 100 values to start with.
        Each time the user plays, we randomly pick one from the list, then remove it from the list
        When the list gets to zero, we reset it
    */
    private List<int> winProbability ;


    /*
        Caculated at the start of the round as the value of amount to bet * multiplier
    */
    private float totaltoWin ;

    /* 
        During the round, we will be opening a number of chests till we hit the pooper. This stores the "index" in
        the "boxVal" list for the next chest
        At the start of the round, we will be setting it to "0". Each time a chest is opened, we increment it
    */
    private int nextBoxIndex ;


    /*
        This stores the amount that we are going to win in each box. The last box will have zero, so we know we hit the pooper
    */
    private List<float> boxVal ;

    /*
        Convenience variable that holds the amount from the last chest
    */
    private float lastChestAmount ;


    private void populateWinProbabilty(){
        // The distribution of multipliers here is as per the required win frequencies:
        // 50% = 0
        // 30% = 1 to 10
        // 15% = 12, 16, 24, 32, 48, 64
        // 5% = 100, 200, 300, 400, 500
        //declaring array of probabilities
        winProbability = new List<int> {0, 0, 0, 0, 0 ,0 ,0, 0 ,0 ,0
            , 0, 0, 0, 0, 0 ,0 ,0, 0 ,0 ,0
            , 0, 0, 0, 0, 0 ,0 ,0, 0 ,0 ,0
            , 0, 0, 0, 0, 0 ,0 ,0, 0 ,0 ,0
            , 0, 0, 0, 0, 0 ,0 ,0, 0 ,0 ,0
            , 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            , 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            , 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            , 12, 16, 24, 32, 48, 64
            , 12, 16, 24, 32, 48, 64
            , 12, 16, 24
            , 100, 200, 300, 400, 500};
    }

    public Score(float startingBalance = 10.0f ) {
        populateWinProbabilty() ;
        balance = startingBalance ; 
    }


    public float getBalance(){
        return balance ;
    }

    public float getAmountToBet(){
        return amountToBet;
    }

    public float getCurrentGameWin(){
        return currentGameWin;
    }
    
    public void addToAmountToBet( float diff ){
        amountToBet += diff ;
    } 

    public void startPlay(){
        balance -= amountToBet;

        // reset to the first chest we are going to open whenever the play button is pressed
        nextBoxIndex = 0 ;


        // Get the index of the multiplier from the winProbablity list
        int chosenMultIndex = UnityEngine.Random.Range(0, winProbability.Count);

        // Get the multiplier for this round from the winProbability array
        int multiplier  = winProbability[ chosenMultIndex ];

        // Remove this multiplier from the llist 
        winProbability.RemoveAt(chosenMultIndex);   

        Debug.Log("Chosen Multiplier is: " + multiplier);
    
        // if there is nothing left in the winProbablity list, then we have to repopulate it
        if( winProbability.Count == 0){
            populateWinProbabilty();
        } 


        // If the amount we won is "zero" (i.e multiplier is zero, dont waste more time)
        if( multiplier == 0 ){
            // First box is going to be a pooper
            boxVal = new List<float>(){ 0.0f };

            // not going to win anything
            totaltoWin = 0 ;
        }
        //Below is code for what happens if the amount is not 0

        // The amount we are going to win is the amount bet * multiplier
        totaltoWin = amountToBet * multiplier ;
        Debug.Log("You could possibly win " + totaltoWin);

        // Ensure we are staying with 2 decimal places
        totaltoWin =  (float)System.Math.Round(totaltoWin,2);
        /* We now have to distribute this between the chests
 
        minimum in a chest is 0.05. If the total we are going to win is less than that, we are going to put it in the first chest

        */
        if( totaltoWin < 0.05 ){
            boxVal = new List<float>(){ totaltoWin, 0 } ; 
            /*
            First chest is classified as ITEM 0 In Array of box values
            Every time a total to win is less than 0.5, that amount always goes to the first chest (which is item 0 in list), ensures that amount is
            going to be the pooper, this way, if you get a win multiplier of 0, because that is the lowest, you get the pooper first
            THIS IS THE POOPER THAT WILL HAPPEN DURING FIRST ROUND, in my older script, this was set to if(totalWin == 0){pooper == true}

            NOTE: condition says that the total to win doesn't have to be 0 because:
            1. no multiplier goes below 1 except 0
            2. chests can only have amounts of 0.05 at least
            */

            return ;
        }
        // we now have to distribute the remaining amount across the boxes randomly, with each box having > 0.05 cents
        /* 
            how many boxes are going to have money? Lets get a number between 1 and 8 (at least one box has money, and the last box 
            must be the pooper
        */

        /*
        Gives us the amount of boxes we open BEFORE we get the pooper, if we only get 1, we only get one pick before the pooper & the round ends,
        if we get 8, we get 8 picks before the pooper and the round ends
        Even though there are 9 boxes, the last one picked will ALWAYS be the pooper, the only exception is if the total to win is 0
        (or in this case less than 0.5)
        */
        int boxesToOpen = UnityEngine.Random.Range(1,8);

        /*
            Lets put in a random amount in each box, between 1 and the totalToWin, here is where things start getting tricky.
            So you start off by declaring a list of variables for the amounts in the box
            You have to put these random amounts in each box individually, in order to target each box and run it, instead of finding each box
            individually, since we already know how many boxes have to be opened overall, we use boxesToOpen in a forloop instead.  This way, it'll run only
            the same amount of time as there are boxes to open. I.E: if there are 8 boxes to open, the for loop runs till it finds those 8 boxes and in
            the 8 boxes, it runs the code in the loop for each one.
            .
            .
            .
            The code in the for loop is run per each box.  First, we declare a float which is the amount in the box, this amount can be anywhere from 
            0.05 cents to the total amount that the player wins.  We then add that amount in the box to the list.
            
            One small problem, the sum of what's in the boxes, according to this logic, will end up being more than the total amount won.

            We have to fix this SO...
            (Go to line 200)
        */
        boxVal = new List<float>() ;
        float sum = 0 ;
        for( int i = 0 ; i < boxesToOpen ; i++ ){
            float amountInBox = UnityEngine.Random.Range(0.05f, totaltoWin) ;
            boxVal.Add( amountInBox );
            sum += amountInBox ;
        }

        /*
            We have to bring the total sum down.  We start this by dividing the sum of what's in the boxes by the total amount we're actually supposed 
            to win.  This gives us our value ratio
        */
        float ratio = totaltoWin / sum ;

        /*
            Now that we brought our value down, we have to apply that to the boxes.
            Therefore for each of the boxes, we multiply the original number by the ratio. 
            For any single box, if the value is < 0.05, then we need to set it to 0.05
        */


        /*
        Now that we have our value down, we run through another for loop, which first gets the existing value in A box
        */
        sum = 0 ;
        for( int i=0 ; i < boxesToOpen ; i++ ){
            float amountInBox = boxVal[ i ];//gets existing value in A box
            amountInBox = amountInBox * ratio ; //replaces the value in the box with this equation
            amountInBox = (float)Math.Round(amountInBox,2); // new value only goes up to 2 decimal places 
            amountInBox = (amountInBox < 0.05f )? 0.05f : amountInBox  ; // and must be be higher than 0.05
            sum = sum + amountInBox ; //Now that amount in the box is added to the sum
            boxVal[i] = amountInBox ; //Store the new amount back in the box
            Debug.Log("Amount per box is " + amountInBox);
        }

        /*
            Small problem though, the sum IS STILL not going to be perfect because of rounding errors, so, we run the below line of code, which...
        */
        float diff = totaltoWin - sum ; //Gets the difference of the total to win and the sum
        boxVal[ boxesToOpen - 1 ] =  boxVal[ boxesToOpen - 1 ] + diff; //takes that "diff" and adds it to the last box (in theory, should make value exact)*
        //* [boxesToOpen - 1] refers to the 2nd to last box in the array

        /*
            Add the pooper box, which is 0
        */
        boxVal.Add(0); //amount in box is 0

        for( int i=0; i < boxVal.Count; i++){
            Debug.Log("["+ i + "]="+boxVal[i]);
        }

    }


    public void endRound() {
        balance += currentGameWin;
        currentGameWin = 0 ;
        amountToBet = 0 ;
    }

    //Activates when oopening the next chest
    public void openNextChest(){
        lastChestAmount = boxVal[ nextBoxIndex ]; //sets last chest amount to the last value in box index
        nextBoxIndex ++ ; //increments next box index when you open the next chest
        currentGameWin += lastChestAmount;
    }

    public bool isPooper(){
        return ( lastChestAmount == 0) ;

    }

    public float amountInChest(){
        return lastChestAmount ;
    }
}
