using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class StatsGameManager : MonoBehaviour
{

    public GameObject GameStats;
    public Button ButtonYes;
    public Button ButtonNo;


    private Slider _moneyIndicator;
    private Slider _happinessIndicator;
    private Slider _citystateIndicator;


    void Awake()
    {
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


    // Update is called once per frame
    void Update()
    {
       
    }
}
