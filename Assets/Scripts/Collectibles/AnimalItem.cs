using UnityEngine;

/// <summary>
/// Script cho animal item có thể collect
/// </summary>
public class AnimalItem : MonoBehaviour
{
    [Header("Animal Settings")]
    [Tooltip("Loại động vật này")]
    [SerializeField] private AnimalType animalType = AnimalType.Cow;
    
    [Tooltip("Effect khi collect (particle, sound, etc.)")]
    [SerializeField] private GameObject collectEffect;
    
    [Tooltip("Sound name khi collect")]
    [SerializeField] private string collectSoundName = "animal_collect";
    
    private bool isCollected = false;
    
    public AnimalType AnimalType => animalType;
    
    /// <summary>
    /// Set animal type (dùng khi spawn động)
    /// </summary>
    public void SetAnimalType(AnimalType type)
    {
        animalType = type;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Chỉ player mới có thể collect
        if (collision.gameObject.CompareTag("Player") && !isCollected)
        {
            CollectAnimal();
        }
    }
    
    /// <summary>
    /// Collect animal này (có thể gọi từ bên ngoài, ví dụ từ PlayerController)
    /// </summary>
    public void CollectAnimal()
    {
        if (isCollected) return;
        
        isCollected = true;
        
        // Thông báo cho QuestManager
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnAnimalCollected(animalType);
        }
        
        // Play sound
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(collectSoundName))
        {
            AudioManager.Instance.PlaySound(collectSoundName);
        }
        
        // Spawn effect
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
        
        // Ẩn hoặc destroy animal
        gameObject.SetActive(false);
        // Hoặc có thể dùng: Destroy(gameObject);
    }
    
    /// <summary>
    /// Reset animal để có thể collect lại (dùng khi restart level)
    /// </summary>
    public void ResetAnimal()
    {
        isCollected = false;
        gameObject.SetActive(true);
    }
}

