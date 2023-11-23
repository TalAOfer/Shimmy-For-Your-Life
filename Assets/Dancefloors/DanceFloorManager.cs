using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DanceFloorManager : MonoBehaviour
{
    public List<CustomTile> tiles;
    public int randomTileDropChance;
    [SerializeField] private int numberOfDancers;
    [SerializeField] private List<GameObject> npcDancers;

    private void Start()
    {
        for (var i = 0; i < npcDancers.Count; i++)
        {
            if (i < numberOfDancers)
            {
                npcDancers[i].transform.position = GetRandomTile().transform.position;
            }
            
            else
            {
                npcDancers[i].SetActive(false);
            }
        }
    }

    public void OnBeat()
    {
        if (Tools.didSucceed(randomTileDropChance))
        {
            CustomTile randomTile = GetRandomTile();
            randomTile.ApplyFall();
            tiles.Remove(randomTile);
        }
    }

    private CustomTile GetRandomTile()
    {
        var rand = Random.Range(0, tiles.Count);
        var randTile = tiles[rand];
        return randTile;
    }
}