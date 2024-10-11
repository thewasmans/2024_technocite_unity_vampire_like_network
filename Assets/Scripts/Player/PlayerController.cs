using System;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float SpeedMovement;
    public Transform Mesh;
    public GameReferencesVariables GameVariables;
    private Transform mTransform;
    public Transform WeaponsTransform;
    public NetworkVariable<Quaternion> nvPlayerRotation = new NetworkVariable<Quaternion>(
        Quaternion.identity,
        readPerm: NetworkVariableReadPermission.Owner,
        writePerm: NetworkVariableWritePermission.Owner
    );

    private void Start()
    {
        mTransform = transform;
        if (IsLocalPlayer)
        {
            Camera.main.transform.parent = transform;
        }
        nvPlayerRotation.OnValueChanged += UpdateRotation;
    }

    private void UpdateRotation(Quaternion previousValue, Quaternion newValue)
    {
        if (IsLocalPlayer)
            return;
        Mesh.rotation = newValue;
        WeaponsTransform.rotation = newValue;
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            var xAxis = Input.GetAxisRaw("Horizontal");
            var yAxis = Input.GetAxisRaw("Vertical");

            var direction = (Vector3.right * xAxis + Vector3.forward * yAxis).normalized;

            mTransform.Translate(direction * SpeedMovement * Time.deltaTime);
            Mesh.LookAt(mTransform.position + direction);
            WeaponsTransform.LookAt(mTransform.position + direction);
            nvPlayerRotation.Value = Mesh.rotation;
        }
    }

    public override void OnDestroy()
    {
        nvPlayerRotation.OnValueChanged -= UpdateRotation;
    }
}
