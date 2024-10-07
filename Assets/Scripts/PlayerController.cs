using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float SpeedMovement;
    public GameObject Mesh;
    public GameReferencesVariables GameVariables;

    private void Awake()
    {
        GameVariables.PlayerTransforms.Add(transform);
    }

    void Update()
    {
        var xAxis = Input.GetAxisRaw("Horizontal");
        var yAxis = Input.GetAxisRaw("Vertical");

        var direction = (Vector3.right * xAxis + Vector3.forward * yAxis).normalized;

        transform.Translate(direction * SpeedMovement * Time.deltaTime);
        Mesh.transform.LookAt(transform.position + direction);
    }

    private void OnDestroy()
    {
        GameVariables.PlayerTransforms.Remove(transform);
    }
}
