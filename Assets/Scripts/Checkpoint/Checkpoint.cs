using UnityEngine;

/// <summary>
/// Script cho checkpoint - nơi player thả animal để tính điểm
/// </summary>
public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [Tooltip("Vị trí để đặt animal khi thả xuống")]
    [SerializeField] private Transform animalDropPoint;
    
    [Tooltip("Effect khi thả animal thành công")]
    [SerializeField] private GameObject dropEffect;
    
    [Tooltip("Sound name khi thả animal")]
    [SerializeField] private string dropSoundName = "animal_drop";
    
    private void Start()
    {
        // Tự động tìm drop point nếu chưa được assign
        if (animalDropPoint == null)
        {
            animalDropPoint = transform.Find("AnimalDropPoint");
            if (animalDropPoint == null)
            {
                // Nếu không tìm thấy, tạo một điểm tại vị trí checkpoint
                GameObject dropPointObj = new GameObject("AnimalDropPoint");
                dropPointObj.transform.SetParent(transform);
                dropPointObj.transform.localPosition = Vector3.zero;
                animalDropPoint = dropPointObj.transform;
            }
        }
    }
    
    /// <summary>
    /// Được gọi khi player vào checkpoint
    /// </summary>
    public void OnPlayerEnter(PlayerController player)
    {
        if (player == null) return;
        
        // Kiểm tra xem player có đang mang animal không
        if (player.HasCarriedAnimal())
        {
            // Thả animal tại checkpoint
            player.DropAnimalAtCheckpoint(animalDropPoint);
            
            // Play sound
            if (AudioManager.Instance != null && !string.IsNullOrEmpty(dropSoundName))
            {
                AudioManager.Instance.PlaySound(dropSoundName);
            }
            
            // Spawn effect
            if (dropEffect != null)
            {
                Instantiate(dropEffect, animalDropPoint.position, Quaternion.identity);
            }
            
            Debug.Log("Đã thả animal tại checkpoint!");
        }
        else
        {
            Debug.Log("Bạn cần mang animal để thả tại checkpoint!");
        }
    }
    
    /// <summary>
    /// Lấy vị trí drop point
    /// </summary>
    public Transform GetDropPoint()
    {
        return animalDropPoint;
    }
}

