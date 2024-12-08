using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HealthController : NetworkBehaviour
{
    [SerializeField] private float _maxHealth;
    
    [SyncVar(hook = nameof(OnHealthChange))]
    private float _currentHealth;
    private CanvasController _playerUI;
    private void Start() {
        if(!isOwned){
            enabled =false;
        }
    }
    public override void OnStartAuthority()
    {
        _playerUI = GameObject.Find("Player UI").GetComponent<CanvasController>();
        _playerUI._viewCanvas.gameObject.SetActive(true);
        _playerUI.SetMaxHealth(_maxHealth);
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
    [Command(requiresAuthority = false)]
    private void CmdDie(NetworkConnectionToClient sender = null)
    {
        Debug.Log("sender :" + sender.connectionId);
        Die();
    }
    [ClientRpc]
    private void Die()
    {
        if(!isOwned){
            gameObject.SetActive(false);
        }
        
    }

    private void OnHealthChange(float _oldCurrentHealth,float _newCurrentHealth){
        if(!isOwned){
            Debug.Log("Hello from on health change");
            Debug.Log("curent health :" + _currentHealth);
            return;
        }
        _playerUI.ChangeCurrentHealth(_currentHealth);
    }


}
