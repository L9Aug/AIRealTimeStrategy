using UnityEngine;
using System.Collections;

/// <summary>
/// Like an Argos ticket but better.
/// </summary>
public class KalamataTicket
{
    public Products Product;
    public BaseBuilding ProductOwner;
    public HexTransform ProductPosition;
    public float ReservationID;

    public KalamataTicket(Products product, BaseBuilding productOwner, float reservationID)
    {
        Product = product;
        ProductOwner = productOwner;
        ProductPosition = ProductOwner.hexTransform;
        ReservationID = reservationID;
    }
}
