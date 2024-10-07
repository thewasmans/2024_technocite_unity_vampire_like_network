using UnityEngine;

public class ExpMB : MonoBehaviour
{
    public float ExpValue = 1;

    void OnTriggerEnter(Collider collider)
    {
        var playerExp = collider.transform.parent.parent.GetComponent<PlayerExpHolder>();
        if (playerExp == null)
            return;
        playerExp.AddExp(ExpValue);
        Destroy(gameObject);
    }
}
