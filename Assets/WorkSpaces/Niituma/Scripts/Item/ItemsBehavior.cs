using UnityEngine;

public class ItemsBehavior : MonoBehaviour
{
    [SerializeField] private ItemGauge gau; // GameSceneUIManager.Instance.ItemGauge
    [SerializeField] private ItemType itemType;
    [SerializeField] private float value;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gau.ItemGaugeUpdate(itemType, value);
            gameObject.SetActive(false);
        }
    }
}
