using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StatsGameManager : MonoBehaviour
{
    public TextAsset CardsDB;
    public GameObject GameCard;
    public GameObject GameStats;
    public Button ButtonYes;
    public Button ButtonNo;
    public Button ButtonOk;
    public GameObject SeaLevel;
    [SerializeField] Button Card;

    private List<Card> MyDeck = new List<Card>();
    private List<Card> MyEvents = new List<Card>();
    private List<int> EventQueued = new List<int>();

    private Slider _moneyIndicator;
    private Slider _happinessIndicator;
    private Slider _citystateIndicator;

    private Card _currentCardToShow;

    private int _counterCards;
    private bool _showEvent;
    private bool _isgameended;

    //----UI
    private Text _cardDescription;
    private Text _consequenceTitle;
    private Text _cardBackground;
    private bool showBackground = false;

    //----Sea Level
    private Slider _seaIndicator;


    void Awake()
    {
        GenerateDeck();
        //----Counter Of Cards to trigger events
        _counterCards = 0;

        //----Bold for Event
        _showEvent = false;

        //----UI sliders
        _moneyIndicator = GameStats.transform.GetChild(0).GetComponent<Slider>();
        _happinessIndicator = GameStats.transform.GetChild(1).GetComponent<Slider>();
        _citystateIndicator = GameStats.transform.GetChild(2).GetComponent<Slider>();

        //----UI
        _cardDescription = GameCard.transform.GetChild(0).GetComponent<Text>(); //CARD DESCRIPTION
        _consequenceTitle = GameCard.transform.GetChild(1).GetComponent<Text>();

        //----Sea Level
        _seaIndicator = SeaLevel.transform.GetComponent<Slider>();

        _isgameended = false;


    }

    // Start is called before the first frame update
    void Start()
    {
        ResetValue();

        ButtonYes.onClick.AddListener(AcceptAction);
        ButtonNo.onClick.AddListener(DenyAction);
        ButtonOk.onClick.AddListener(OkEvent);

        Card.onClick.AddListener(CardDescription);
        CardToUI(_currentCardToShow);
        
    }


    public void ResetValue()
    {
        _moneyIndicator.value = 0.5f;
        _happinessIndicator.value = 0.5f;
        _citystateIndicator.value = 0.5f;
        _seaIndicator.value = 0.0f;

    }

    public void OkEvent()
    {
        ButtonYes.gameObject.SetActive(true);
        ButtonNo.gameObject.SetActive(true);
        ButtonOk.gameObject.SetActive(false);
        _consequenceTitle.gameObject.SetActive(false);

        if (_isgameended)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            AcceptAction();
        }
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
        if (_currentCardToShow == null)
        {
            Debug.Log("ACABA EL JUEGO");
            _cardDescription.text = "WIN\nHas conseguido evitar la catástrofe.";
            ButtonYes.enabled = false;
            ButtonNo.enabled = false;
            Card.enabled = false;
        }
        else
        {
            if (Accept)
            {
                GameCard.transform.GetChild(2).gameObject.SetActive(false);
                GameCard.transform.GetChild(0).gameObject.SetActive(true);
                _moneyIndicator.value = CalculateValueToSlider(_moneyIndicator.value, _currentCardToShow.MoneyY);
                _happinessIndicator.value = CalculateValueToSlider(_happinessIndicator.value, _currentCardToShow.HappyY);
                _citystateIndicator.value = CalculateValueToSlider(_citystateIndicator.value, _currentCardToShow.CityY);

                if(_currentCardToShow.SeaLvlY == 1)
                {
                    _seaIndicator.value = CalculateValueToSlider(_seaIndicator.value + 0.25f, _currentCardToShow.SeaLvlY);
                }
                else if(_currentCardToShow.SeaLvlY == -1)
                {
                    _seaIndicator.value = CalculateValueToSlider(_seaIndicator.value - 0.25f, _currentCardToShow.SeaLvlY);
                }
                else
                {
                    _seaIndicator.value = CalculateValueToSlider(_seaIndicator.value, _currentCardToShow.SeaLvlY);
                }

                if (_currentCardToShow.EventY != 0)
                {
                    EventQueued.Add(_currentCardToShow.EventY);
                }
                if (!CheckIfAllSeen())
                {
                    _currentCardToShow = ChangeToNextCard();
                    CardToUI(_currentCardToShow);
                    _counterCards++;
                }
                else
                {
                    Debug.Log("ACABA EL JUEGO");
                    _cardDescription.text = "WIN\nHas conseguido evitar la catástrofe.";
                    ButtonYes.enabled = false;
                    ButtonNo.enabled = false;
                    Card.enabled = false;
                }
                EndGame();

            }
            else
            {
                GameCard.transform.GetChild(2).gameObject.SetActive(false);
                GameCard.transform.GetChild(0).gameObject.SetActive(true);
                _moneyIndicator.value = CalculateValueToSlider(_moneyIndicator.value, _currentCardToShow.MoneyN);
                _happinessIndicator.value = CalculateValueToSlider(_happinessIndicator.value, _currentCardToShow.HappyN);
                _citystateIndicator.value = CalculateValueToSlider(_citystateIndicator.value, _currentCardToShow.CityN);

                if (_currentCardToShow.SeaLvlN == 1)
                {
                    _seaIndicator.value = CalculateValueToSlider(_seaIndicator.value + 0.25f, _currentCardToShow.SeaLvlN);
                }
                else if (_currentCardToShow.SeaLvlN == -1)
                {
                    _seaIndicator.value = CalculateValueToSlider(_seaIndicator.value - 0.25f, _currentCardToShow.SeaLvlN);
                }
                else
                {
                    _seaIndicator.value = CalculateValueToSlider(_seaIndicator.value, _currentCardToShow.SeaLvlN);
                }

                if (_currentCardToShow.EventN != 0)
                {
                    EventQueued.Add(_currentCardToShow.EventN);
                }
                if (!CheckIfAllSeen())
                {
                    _currentCardToShow = ChangeToNextCard();
                    CardToUI(_currentCardToShow);
                    _counterCards++;
                }
                else
                {
                    Debug.Log("ACABA EL JUEGO");
                    _cardDescription.text = "WIN\nHas conseguido evitar la catástrofe.";
                    ButtonYes.enabled = false;
                    ButtonNo.enabled = false;
                    Card.enabled = false;
                }

                EndGame();

            }
        }
    }

    public void EraseCard(Card cardToErase)
    {
        if (MyDeck.Count != 0)
        {
            MyDeck.Remove(cardToErase);
            Debug.Log("Number of cards in a deck: " + MyDeck.Count);
        }
        else
        {
            Debug.Log("The deck has already empty");
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
        if (valueStat == -2)
        {
            aux = indicatorValue - 0.150f;
        }
        if (valueStat == 2)
        {
            aux = 0.150f + indicatorValue;
        }
        return aux;
    }


    public Card ChangeToNextCard()
    {

        Card aux = new Card();
        if (_counterCards % 2 == 0 && EventQueued.Count != 0)
        {
            aux = MyEvents.Find(x => x.Id == EventQueued[0]);
            EventQueued.Remove(EventQueued[0]);
            _showEvent = true;
            return aux;
        }
        else
        {
            int random = Random.Range(1, MyDeck.Count);
            aux = MyDeck.Find(x => x.Id == random);

            do
            {
                int random2 = Random.Range(1, MyDeck.Count);
                aux = MyDeck.Find(x => x.Id == random2);

            } while (aux == null);

            if (aux.Seen)
            {
                for (int i=0; i < MyDeck.Count; i++)
                {
                    if (!MyDeck[i].Seen)
                    {
                        aux = MyDeck[i];
                        break;
                    }
                }
            }
            if (aux.Seen)
            {
                Debug.Log("ACABA EL JUEGO");
                _cardDescription.text = "WIN\nHas conseguido evitar la catástrofe.";
                ButtonYes.enabled = false;
                ButtonNo.enabled = false;
                Card.enabled = false;
            }

            aux.Seen = true;
            Debug.Log(aux.Id);

            return aux;
        }

    }

    public void CardToUI(Card currentCard)//de la base de datos a mostrarla por pantalla
    {
        if (_showEvent)
        {
            _cardDescription.fontStyle = FontStyle.Bold;

            _showEvent = false;
            ButtonYes.gameObject.SetActive(false);
            ButtonNo.gameObject.SetActive(false);
            ButtonOk.gameObject.SetActive(true);
            GameCard.transform.GetChild(2).gameObject.SetActive(false);
            GameCard.transform.GetChild(0).gameObject.SetActive(true);
            _consequenceTitle.gameObject.SetActive(true);

        }
        else
        {
            _cardDescription.fontStyle = FontStyle.Normal;
        }
        if (currentCard != null)
        {
            _cardDescription.text = currentCard.Description;
        }
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
            ncard.Background = row[13];

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
                ncard.Seen = true;
                _currentCardToShow = new Card(ncard);
            }
        }
       // Debug.Log("NUMERO CARTAS "+ MyDeck.Count);
    }

    public void CardDescription()
    {
        // Card.gameObject.SetActive(false);
        if (showBackground)
        {
            GameCard.transform.GetChild(0).gameObject.SetActive(false);
            GameCard.transform.GetChild(2).gameObject.SetActive(true);
            GameCard.transform.GetChild(2).GetComponent<Text>().text = _currentCardToShow.Background;
            showBackground = false;
        }
           
        else
        {
            GameCard.transform.GetChild(0).gameObject.SetActive(true);
            GameCard.transform.GetChild(2).gameObject.SetActive(false);
            GameCard.transform.GetChild(2).GetComponent<Text>().text = _currentCardToShow.Description;
            showBackground = true;
        }
            
      
    }
    
    public bool CheckIfAllSeen()
    {
        int counter = 0;
        for (int i=0; i < MyDeck.Count; i++)
        {
            if (MyDeck[i].Seen)
            {
                counter++;
            }
        }
        if (counter == MyDeck.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EndGame()
    {

        if(_moneyIndicator.value == 0)
        {
            Debug.Log("ACABA EL JUEGO");
            _cardDescription.text = "END GAME\nTe has quedado sin fondos para poder continuar.";
            ButtonYes.gameObject.SetActive(false);
            ButtonNo.gameObject.SetActive(false);
            ButtonOk.gameObject.SetActive(true);
            _isgameended = true;
            Card.enabled = false;
        }
        if(_happinessIndicator.value == 0)
        {
            Debug.Log("ACABA EL JUEGO");
            _cardDescription.text = "END GAME\nTus ciudadanos estan muy enfadados y te han destituido del cargo.";
            ButtonYes.gameObject.SetActive(false);
            ButtonNo.gameObject.SetActive(false);
            ButtonOk.gameObject.SetActive(true);
            _isgameended = true;
            Card.enabled = false;
        }
        if (_citystateIndicator.value == 0)
        {
            Debug.Log("ACABA EL JUEGO");
            _cardDescription.text = "END GAME\nLa ciudad está en muy mal estado, así no hay quien viva.";
            ButtonYes.gameObject.SetActive(false);
            ButtonNo.gameObject.SetActive(false);
            ButtonOk.gameObject.SetActive(true);
            _isgameended = true;
            Card.enabled = false;
        }
        if (_seaIndicator.value == 1)
        {
            Debug.Log("ACABA EL JUEGO");
            _cardDescription.text = "END GAME\nLa ciudad ha sido engullida por el mar.";
            ButtonYes.gameObject.SetActive(false);
            ButtonNo.gameObject.SetActive(false);
            ButtonOk.gameObject.SetActive(true);
            _isgameended = true;
            Card.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
