using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LoadScene(int level)
    {
        SceneManager.LoadScene("SceneLvl1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
