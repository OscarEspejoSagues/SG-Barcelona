using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCartas : MonoBehaviour
{
    public List<Card> MyDeck = new List<Card>(); 

    public TextAsset CardsDB;
    // Start is called before the first frame update
    void Start()
    {
        string[] data = CardsDB.text.Split(new char[] { '\n' }); //separa las linias y las guarda como strings
        Debug.Log(data.Length);

        for (int i = 1; i<data.Length; i++)
        {
            string[] row = data[i].Split(new char[] {','});
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
