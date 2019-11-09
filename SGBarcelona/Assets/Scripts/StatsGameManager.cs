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
    [SerializeField] Button Card;
    [SerializeField] Button Description;


    private List<Card> MyDeck = new List<Card>();
    private List<Card> MyEvents = new List<Card>();
    private List<int> EventQueued = new List<int>();

    private Slider _moneyIndicator;
    private Slider _happinessIndicator;
    private Slider _citystateIndicator;

    private Card _currentCardToShow;

    private int _counterCards; 

    //----UI
    private Text _cardDescription;
    private Image _cardImage;


    void Awake()
    {
        GenerateDeck();
        //----Counter Of Cards to trigger events
        _counterCards = 0;

        //----UI sliders
        _moneyIndicator = GameStats.transform.GetChild(0).GetComponent<Slider>();
        _happinessIndicator = GameStats.transform.GetChild(1).GetComponent<Slider>();
        _citystateIndicator = GameStats.transform.GetChild(2).GetComponent<Slider>();

        //----UI
        _cardDescription = GameCard.transform.GetChild(0).GetComponent<Text>(); //CARD DESCRIPTION
        _cardImage = GameCard.transform.GetChild(1).GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetValue();
        ButtonYes.onClick.AddListener(AcceptAction);
        ButtonNo.onClick.AddListener(DenyAction);
        Card.onClick.AddListener(CardDescription);
        Description.onClick.AddListener(CloseDescription);
        CardToUI(_currentCardToShow);
    }


    public void ResetValue()
    {
        _moneyIndicator.value = 0.5f;
        _happinessIndicator.value = 0.5f;
        _citystateIndicator.value = 0.5f;
    }

    public void AcceptAction()
    {
        ApplyCard(true);
    }

    public void DenyAction()
    {
        ApplyCard(false);
    }

    public void ApplyCard(bool Accept)
    {
        if (Accept)
        {
            _moneyIndicator.value = CalculateValueToSlider(_moneyIndicator.value, _currentCardToShow.MoneyY);
            _happinessIndicator.value = CalculateValueToSlider(_happinessIndicator.value, _currentCardToShow.HappyY);
            _citystateIndicator.value = CalculateValueToSlider(_citystateIndicator.value, _currentCardToShow.CityY);

            if (_currentCardToShow.EventY != 0)
            {
                EventQueued.Add(_currentCardToShow.EventY);
            }
            _currentCardToShow = ChangeToNextCard();
            CardToUI(_currentCardToShow);
            _counterCards++;
        }
        else
        {
            _moneyIndicator.value = CalculateValueToSlider(_moneyIndicator.value, _currentCardToShow.MoneyN);
            _happinessIndicator.value = CalculateValueToSlider(_happinessIndicator.value, _currentCardToShow.HappyN);
            _citystateIndicator.value = CalculateValueToSlider(_citystateIndicator.value, _currentCardToShow.CityN);

            if (_currentCardToShow.EventN != 0)
            {
                EventQueued.Add(_currentCardToShow.EventN);
            }
            _currentCardToShow = ChangeToNextCard();
            CardToUI(_currentCardToShow);
            _counterCards++;
        }
    }

    public float CalculateValueToSlider(float indicatorValue, float valueStat)
    {
        float aux = 0;
        if (valueStat == 0)
        {
            aux = indicatorValue;
        }
        if (valueStat == -1)
        {
            aux = indicatorValue - 0.050f;
        }
        if (valueStat == 1)
        {
            aux = 0.050f + indicatorValue;
        }
        return aux;
    }


    public Card ChangeToNextCard()
    {
        Card aux = new Card();
        if (_counterCards >= 2 && EventQueued.Count != 0)
        {
            Debug.Log("Event Triggered!");
            _counterCards = 0;
            aux = MyEvents.Find(x => x.Id == EventQueued[0]);
            EventQueued.Remove(EventQueued[0]);
            return aux;
        }
        else
        {
            int random = Random.Range(1, 10);
            aux = MyDeck.Find(x => x.Id == random);

            while (aux == null)
            {
                int random2 = Random.Range(1, 9);
                aux = MyDeck.Find(x => x.Id == random2);
            }
            Debug.Log(aux.Id);

            return aux;
        }
    }

    public void CardToUI(Card currentCard)//de la base de datos a mostrarla por pantalla
    {
        _cardDescription.text = currentCard.Description;
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
            int.TryParse(row[1], out ncard.isEvent);
            ncard.Description = row[2];
            //------
            int.TryParse(row[3], out ncard.MoneyY);
            int.TryParse(row[4], out ncard.HappyY);
            int.TryParse(row[5], out ncard.CityY);
            //------
            int.TryParse(row[6], out ncard.MoneyN);
            int.TryParse(row[7], out ncard.HappyN);
            int.TryParse(row[8], out ncard.CityN);
            //------
            int.TryParse(row[9], out ncard.SeaLvlY);
            int.TryParse(row[10], out ncard.SeaLvlN);
            //------
            int.TryParse(row[11], out ncard.EventY);
            int.TryParse(row[12], out ncard.EventN);

            if (ncard.isEvent == 1)
            {
                MyEvents.Add(ncard);
            }
            if (ncard.isEvent == 0)
            {
                MyDeck.Add(ncard);
            }

            if (ncard.Id == 1)
            {
                _currentCardToShow = new Card(ncard);
            }
        }
    }

    public void CardDescription()
    {
        Description.gameObject.SetActive(true);
        Card.gameObject.SetActive(false);
    }
    public void CloseDescription()
    {
        Description.gameObject.SetActive(false);
        Card.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
