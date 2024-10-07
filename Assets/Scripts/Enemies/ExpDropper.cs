using UnityEngine;

public class ExpDropper : MonoBehaviour
{
    public GameObject ExpPrefab;
    void OnDestroy()
    {
        DropExp();
    }

    private void DropExp()
    {
        var go = Instantiate(ExpPrefab, transform.position, Quaternion.identity);
    }
}
