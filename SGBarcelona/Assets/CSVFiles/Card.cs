using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public int Id;
    public int isEvent;
    public string Description; 
    //---------
    public int MoneyY;
    public int HappyY;
    public int CityY;
    //---------
    public int MoneyN;
    public int HappyN;
    public int CityN;
    //---------
    public int SeaLvlY;
    public int SeaLvlN;
    //---------
    public int EventY;
    public int EventN;
    //---------
    public string Background;


    public Card()
    {

    }

    public Card(Card ncard) //constructor por copia
    {
        Id = ncard.Id;
        Description = ncard.Description;
        Background = ncard.Background;


        MoneyY = ncard.MoneyY;
        HappyY = ncard.HappyY;
        CityY = ncard.CityY;

        MoneyN = ncard.MoneyN;
        HappyN = ncard.HappyN;
        CityN = ncard.CityN;

        SeaLvlY = ncard.SeaLvlY;
        SeaLvlN = ncard.SeaLvlN;

        EventY = ncard.EventY;
        EventN = ncard.EventN;
    }

}
