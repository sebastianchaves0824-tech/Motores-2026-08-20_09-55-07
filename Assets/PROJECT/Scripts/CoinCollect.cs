using UnityEngine;

public class CoinCollect : MonoBehaviour
{

    private int coin = 0;


    private void OnTriggerEnter(Collider other) 
    { 
      if(other.transform.tag == "Coin") 
        { 
          coin++;
            Debug.Log(coin);
            Destroy(other.gameObject);
        }
    }
}
