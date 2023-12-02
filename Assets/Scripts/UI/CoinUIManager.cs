using System.Collections;
using TMPro;
using UnityEngine;

public class CoinUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text coinsEffectText;
    [SerializeField] private PlayerCoins playerCoins;

    private int updateCoins = Animator.StringToHash("Update");
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        playerCoins.OnRecievedCoins += (int total) => OnCoinsUpdated(true, total);
        playerCoins.OnSpendCoins += (int total) => OnCoinsUpdated(false, total);
    }

    private void OnCoinsUpdated(bool isRecieved, int total)
    {
        StopAllCoroutines();
        animator.SetTrigger(updateCoins);
        int prevCoins = isRecieved ? playerCoins.CurrentCoins - total : playerCoins.CurrentCoins + total;
        StartCoroutine(SlowUpdateCoins(prevCoins, playerCoins.CurrentCoins));
        coinsEffectText.text = $"{(isRecieved ? "+" : "-")}{total}";
        coinsEffectText.color = isRecieved ? Color.green : Color.red;
    }
    
    public IEnumerator SlowUpdateCoins(int start, int end)
    {
        float elapsed = 0f;

        while (elapsed < 2f)
        {
            elapsed += Time.deltaTime;

            int coins = (int)Mathf.Lerp(start, end, elapsed / 2f);

            coinsText.text = coins.ToString();

            yield return null;
        }

        coinsText.text = end.ToString();
    }
}
