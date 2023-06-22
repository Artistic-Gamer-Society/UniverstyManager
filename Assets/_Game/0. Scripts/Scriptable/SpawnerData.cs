using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerData", menuName = "ScriptableObjects/SpawnerData", order = 1)]
public class SpawnerData : ScriptableObject
{
    [SerializeField]
    private int baseRequiredMoney; // The initial required money

    [SerializeField]
    private float moneyMultiplier = 1.2f; // The multiplier to increase the required money


    [SerializeField]
    private int currentRequiredMoney; // The current required money

    public int GetCurrentRequiredMoney()
    {
        return currentRequiredMoney;
    }

    public int CalculateMoneyNeededToSpawn()
    {
        IncrementRequiredMoney();
        return currentRequiredMoney;
    }

    private void IncrementRequiredMoney()
    {
        currentRequiredMoney = Mathf.RoundToInt(currentRequiredMoney * moneyMultiplier);
        SaveRequiredMoney();
    }

    public void SaveRequiredMoney()
    {
        PlayerPrefs.SetInt("RequiredMoney", currentRequiredMoney);
        PlayerPrefs.Save();
    }

    public void LoadRequiredMoney()
    {
        if (PlayerPrefs.HasKey("RequiredMoney"))
        {
            currentRequiredMoney = PlayerPrefs.GetInt("RequiredMoney");
        }
        else
        {
            currentRequiredMoney = baseRequiredMoney;
        }
    }

}
