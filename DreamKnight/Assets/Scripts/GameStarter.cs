using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class GameStarter : MonoBehaviour
{
    public GameObject Hero;

    // Start is called before the first frame update
    void Start()
    {
        References.MainController = new GameController();
        References.Hero = Hero;
    }
    
}
