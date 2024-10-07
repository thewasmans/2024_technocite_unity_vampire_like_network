using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyMover : NetworkBehaviour
{
    private float mTimer;
    private Transform mTransform;
    private float mUpdateTargetTimer;
    private NetworkVariable<Vector3> mTarget;
    public float Speed = 2;
    public List<Transform> PlayerTransforms;

    void Awake()
    {
        mTarget = new NetworkVariable<Vector3>(
            Vector3.zero,
            readPerm: NetworkVariableReadPermission.Everyone,
            writePerm: NetworkVariableWritePermission.Server
        );
        mUpdateTargetTimer = 1;
        mTimer = 0;
        mTransform = transform;
    }

    void Update()
    {
        if (IsOwner)
        {
            mTimer -= Time.deltaTime;
            if (mTimer < 0)
                UpdateTarget();
        }

        Move();
    }

    private void UpdateTarget()
    {
        mTimer = mUpdateTargetTimer;
        GetNewTargetRpc();
    }

    [Rpc(SendTo.Server)]
    private void GetNewTargetRpc()
    {
        mTarget.Value = GetNearestPlayerPosition();
    }

    private void Move()
    {
        var dir = (mTarget.Value - mTransform.position).normalized;
        mTransform.localPosition += dir * Speed * Time.deltaTime;
    }

    private Vector3 GetNearestPlayerPosition()
    {
        return PlayerTransforms
            .OrderBy(t => Vector3.SqrMagnitude(t.transform.position - mTransform.position))
            .First()
            .position;
    }
}
