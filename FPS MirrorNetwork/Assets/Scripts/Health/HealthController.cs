using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HealthController : NetworkBehaviour
{
    [SerializeField] private float _maxHealth;
    [SyncVar] private float _currentHealth;
    public override void OnStartAuthority()
    {

        SetCurrentHealth();
    }
    [Command(requiresAuthority = true)]
    private void SetCurrentHealth()
    {
        _currentHealth = _maxHealth;
    }
    public void TakeDame(float dame)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - dame, 0, _maxHealth);
        if(_currentHealth <= 0){
            CmdDie();
        }
    }
    [Command]
    private void CmdDie()
    {
        Die();
    }
    [ClientRpc]
    private void Die()
    {
        gameObject.SetActive(false);
    }


}
