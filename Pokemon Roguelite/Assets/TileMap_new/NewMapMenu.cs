using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapMenu : MonoBehaviour
{
    public SquareGrid squareGrid;
    public SquareMapGenerator mapGenerator;
    int x = 20;
    int z = 15;

    private void Awake()
    {
        EventHandler.current.onGenerateMap += CreateMap;
    }


    public void CreateMap()
    {
        mapGenerator.GenerateMap(x, z);
    }


}
