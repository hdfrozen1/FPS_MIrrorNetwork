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
    private void Start()
    {

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        transform.name = "Player_" + netId;
        ManagePlayer.RegisterPlayer(netId, this);

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
        if (_currentHealth <= 0 && isServer)
        {
            Die();

        }
    }

    [ClientRpc]
    private void Die()
    {
        if (!isOwned)
        {
            gameObject.SetActive(false);
        }
        else if (isOwned)
        {
            StartCoroutine(HoiSinh(5));
        }

    }

    private void OnHealthChange(float _oldCurrentHealth, float _newCurrentHealth)
    {
        if (!isOwned)
        {
            Debug.Log("Hello from on health change");
            Debug.Log("curent health :" + _currentHealth);
            return;
        }
        _playerUI.ChangeCurrentHealth(_currentHealth);
    }
    private IEnumerator HoiSinh(int coolDown)
    {
        int a = coolDown;
          _playerUI.OnDieCalling();
        while (a >= 0)
        {
            _playerUI.SetRespawn(a);
            yield return new WaitForSecondsRealtime(1);
            a -= 1;
            
        }
        CmdActiveObject();
    }

    [Command(requiresAuthority = true)]
    private void CmdActiveObject(){
        _currentHealth = _maxHealth;
        Debug.Log("Hello from CmdActiveObject");
        ActiveObject();
    }

    [ClientRpc]
    private void ActiveObject()
    {
        if (!isOwned)
        {
            gameObject.SetActive(true);
        }
    }


}
