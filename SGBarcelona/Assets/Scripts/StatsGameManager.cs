using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class StatsGameManager : MonoBehaviour
{
    public TextAsset CardsDB;
    public GameObject GameCard;
    public GameObject GameStats;
    public Button ButtonYes;
    public Button ButtonNo;


    private List<Card> MyDeck = new List<Card>();
    private Slider _moneyIndicator;
    private Slider _happinessIndicator;
    private Slider _citystateIndicator;


    void Awake()
    {
        GenerateDeck();
        _moneyIndicator = GameStats.transform.GetChild(0).GetComponent<Slider>();
        _happinessIndicator = GameStats.transform.GetChild(1).GetComponent<Slider>();
        _citystateIndicator = GameStats.transform.GetChild(2).GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetValue();
        ButtonYes.onClick.AddListener(AcceptAction);
        ButtonNo.onClick.AddListener(DenyAction);
    }


    public void ResetValue()
    {
        _moneyIndicator.value = 0.5f;
        _happinessIndicator.value = 0.5f;
        _citystateIndicator.value = 0.5f;
    }

    public void AcceptAction()
    {
        Debug.Log("ACCEPT ACTION");
    }

    public void DenyAction()
    {
        Debug.Log("DENY ACTION");
    }

    public void GenerateDeck()
    {
        string[] data = CardsDB.text.Split(new char[] { '\n' }); //separa las linias y las guarda como strings
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            Card ncard = new Card();
            //------
            int.TryParse(row[0], out ncard.Id);
            ncard.Description = row[1];
            //------
            int.TryParse(row[2], out ncard.MoneyY);
            int.TryParse(row[3], out ncard.HappyY);
            int.TryParse(row[4], out ncard.CityY);
            //------
            int.TryParse(row[5], out ncard.MoneyN);
            int.TryParse(row[6], out ncard.HappyN);
            int.TryParse(row[7], out ncard.CityN);
            //------
            int.TryParse(row[8], out ncard.SeaLvlY);
            int.TryParse(row[9], out ncard.SeaLvlN);
            //------
            int.TryParse(row[10], out ncard.EventY);
            int.TryParse(row[11], out ncard.EventN);

            MyDeck.Add(ncard);
        }
        Debug.Log("THE NUMBER OF CARDS IS: " + MyDeck.Count);
    }


    // Update is called once per frame
    void Update()
    {
       
    }
}
