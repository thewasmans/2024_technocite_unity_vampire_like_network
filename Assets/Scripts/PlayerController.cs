using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float SpeedMovement;
    public GameObject Mesh;
    public GameReferencesVariables GameVariables;

    public override void OnNetworkSpawn()
    {
        
    }

    private void Start()
    {
        if(IsLocalPlayer)
        {
            Camera.main.transform.parent = transform;
        }
    }

    void Update()
    {
        if(IsLocalPlayer)
        { 
            var xAxis = Input.GetAxisRaw("Horizontal");
            var yAxis = Input.GetAxisRaw("Vertical");

            var direction = (Vector3.right * xAxis + Vector3.forward * yAxis).normalized;

            transform.Translate(direction * SpeedMovement * Time.deltaTime);
            Mesh.transform.LookAt(transform.position + direction);
        }
    }
}
