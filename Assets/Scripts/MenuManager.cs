using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public bool GameState;
    public GameObject menuElement;
    private void Awake()
    {
        instance = this;
        GameState = false;
    }
    public void StartTheGame()
    {
        GameState = true;
        menuElement.SetActive(false);
        GameObject.FindWithTag("air").GetComponent<ParticleSystem>().Play();
    }
}
