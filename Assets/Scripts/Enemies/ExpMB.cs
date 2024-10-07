using UnityEngine;

public class ExpMB : MonoBehaviour
{
    public float ExpValue = 1;

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
            return;
        var playerExp = collider.transform.parent.GetComponent<PlayerExpHolder>();
        if (playerExp == null)
            return;
        playerExp.AddExp(ExpValue);
        Destroy(gameObject);
    }
}
