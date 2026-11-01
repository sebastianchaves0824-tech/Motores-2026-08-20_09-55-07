using UnityEngine;
using TMPro;

public class CoinCollect : MonoBehaviour
{

    private int coin = 0;

    public TextMeshProUGUI coinText;
    

    private void OnTriggerEnter(Collider other) 
    { 
      if(other.transform.tag == "Coin") 
        { 
          coin++;
            coinText.text = "Coins: " + coin.ToString();
            Debug.Log(coin);
            Destroy(other.gameObject);
            SoundFXManager.Instance.Play3DSound("CoinClip", transform.position, volume: 1f);

        }
    }
}
