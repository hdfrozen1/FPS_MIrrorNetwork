using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CanvasController : MonoBehaviour
{
    public Canvas _viewCanvas;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _respawnText;
    private float _maxHealth;

    public void ChangeCurrentHealth(float currentHealth){
        _healthText.text = "Health :" + currentHealth + " / " + _maxHealth; 
    }

    public void SetMaxHealth(float maxHealth){
        _maxHealth = maxHealth;
    }
    public void SetRespawn(int a){
        _respawnText.text = "Respawn :" + a;
    }
    public void OnDieCalling(){
        _respawnText.gameObject.SetActive(true);
    }
}
