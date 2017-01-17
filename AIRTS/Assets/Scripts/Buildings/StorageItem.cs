using UnityEngine;
using System.Collections;

public class StorageItem
{
    public Products Product;
    public bool Reserved = false;
    private float ReserveID = 0;

    public StorageItem(Products product)
    {
        Product = product;
    }

    public float ReserveProduct()
    {
        return (!Reserved) ? Time.realtimeSinceStartup : 0;
    }

    /// <summary>
    /// Generates a unique ID using the time from the start of the game.
    /// </summary>
    /// <returns></returns>
    private float GenerateID()
    {
        return Time.realtimeSinceStartup;
    }

    /// <summary>
    /// Un-Reserves the product if it has this ID.
    /// </summary>
    /// <param name="ID"></param>
    public void UnreserveProduct(float ID)
    {
        if (ReserveID == ID)
        {
            Reserved = false;
            ReserveID = 0;
        }
    }

}
