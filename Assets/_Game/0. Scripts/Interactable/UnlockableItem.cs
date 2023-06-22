using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class UnlockableItem : MonoBehaviour
{
    public int itemCost = 100;
    public TextMeshPro costText;
    public string itemKey; // Unique key to identify the unlockable item

    [SerializeField] GameObject itemToUnlock;
    public GameObject nextToUnlock;
    public static event Action<Vector3> OnUnlockItem;

    private void Start()
    {
        costText.text = "$" + itemCost.ToString();
        CheckUnlockStatus();
    }

    private void OnMouseDown()
    {
        Currency currency = Currency.GetInstance(); // To Unlock Item You Need Money So Here It Is Dependent.
        if (currency.playerMoney >= itemCost)
        {
            OnUnlockItem?.Invoke(transform.position);
            currency.SubtractMoney(itemCost);
            UnlockItem();
            SaveUnlockStatus();
        }
        else
        {
            Debug.Log("Insufficient funds!");
        }
    }

    private void UnlockItem()
    {
        StartCoroutine(TextSmoothUpdater.UpdateMoneyTextSmoothly("$", costText, itemCost, 0,TextEffect.None));
        itemCost = 0;
        var _scale = itemToUnlock.transform.localScale;
        itemToUnlock.transform.localScale = Vector3.zero;
        itemToUnlock.SetActive(true);
        itemToUnlock.transform.DOScale(_scale, 0.2f).SetEase(Ease.InOutBack);
        gameObject.SetActive(false);
        if (nextToUnlock != null)
        {
            nextToUnlock.SetActive(true);
        }
    }

    private void CheckUnlockStatus()
    {
        if (PlayerPrefs.HasKey(itemKey))
        {
            // Item is already unlocked, update the UI or apply any necessary changes
            UnlockItem();
        }
    }
    private void SaveUnlockStatus()
    {
        // Save the unlock status of the item
        PlayerPrefs.SetInt(itemKey, 1);
        PlayerPrefs.Save();
    }
}
