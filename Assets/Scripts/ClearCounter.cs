using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    private void Update()
    {

    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no kitchen object here
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else
            {
                // player ain't carrying something
            }
        } else
        {
            // ayyyyyy a kitchen object
            if (player.HasKitchenObject())
            {
                // player has something
            } else
            {
                // player ain't got anything jit
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
 