using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public bool isDead = false;

    void Start() {
        maxHealth = PlayerDataManager.Instance.playerData.health;
        currentHealth = maxHealth;
        isDead = false;
        GUIPanel.Instance.SetHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage) {
        if (isDead) return;
        
        currentHealth -= damage;
        AudioManager.Instance.PlayHurtSound();
        
        if (currentHealth <= 0) {
            currentHealth = 0;
            GUIPanel.Instance.SetHealthBar(currentHealth, maxHealth);
            Die();
            return;
        }
        GUIPanel.Instance.SetHealthBar(currentHealth, maxHealth);
    }

    private void Die() {
        if (isDead) return;
        isDead = true;
        
        // Trigger animation chết
        if (PlayerController.Instance != null && PlayerController.Instance.playerAnimation != null) {
            PlayerController.Instance.playerAnimation.SetDie();
        }
        
        // Dừng player controller
        if (PlayerController.Instance != null) {
            PlayerController.Instance.SetDisable(true);
        }
        
        // Hiển thị lose panel sau 1 giây
        StartCoroutine(ShowLosePanelDelayed());
    }
    
    private IEnumerator ShowLosePanelDelayed() {
        yield return new WaitForSeconds(1f);
        
        if (UIManager.Instance != null && UIManager.Instance.gamePlayPanel != null) {
            UIManager.Instance.gamePlayPanel.ShowLosePanel(true);
            Time.timeScale = 0f;
        }
    }
}
