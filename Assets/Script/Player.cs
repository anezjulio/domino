using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera playerCamera;
    public List<GameObject> tiles = new List<GameObject>();

    public void addTiles(List<GameObject> tiles){
        this.tiles = tiles;
    }



}
