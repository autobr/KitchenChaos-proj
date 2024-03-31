using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DeliveryResultUI : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Image bkgrndImage;
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;
    [SerializeField] private Sprite goodJob;
    [SerializeField] private Sprite badJob;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryMngr_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryMngr_OnRecipeFailed;
        gameObject.SetActive(false);
    }

    private void DeliveryMngr_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        StartCoroutine(popup());
        bkgrndImage.color = failColor;
        iconImage.sprite = badJob;
        messageText.text = "DELIVERY\nFAILED";

    }

    private void DeliveryMngr_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        StartCoroutine(popup());
        bkgrndImage.color = successColor;
        iconImage.sprite = goodJob;
        messageText.text = "DELIVERY\nSUCCESS";
    }

    private IEnumerator popup()
    {
        yield return new WaitForSecondsRealtime(2f);
        gameObject.SetActive(false);
    }
}
