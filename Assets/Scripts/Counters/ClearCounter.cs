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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                } else
                {
                    // player is not carrying plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // counter has plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // check that player's obj is an ingredient, place on plate
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else
            {
                // player ain't got anything jit
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
 