using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawnor : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;
    private HealthSystem healthsystem;

    private void Awake() {

        healthsystem = GetComponent<HealthSystem>();
        healthsystem.onDead += HealthSystem_onDead;
    }

    private void HealthSystem_onDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = 
        Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
