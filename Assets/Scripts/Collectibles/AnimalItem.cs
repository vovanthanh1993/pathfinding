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
    private bool isPickedUp = false; // Đã được lượm nhưng chưa thả tại checkpoint
    
    public AnimalType AnimalType => animalType;
    public bool IsPickedUp => isPickedUp;
    public bool IsCollected => isCollected;
    
    /// <summary>
    /// Set animal type (dùng khi spawn động)
    /// </summary>
    public void SetAnimalType(AnimalType type)
    {
        animalType = type;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Chỉ player mới có thể lượm
        if (collision.gameObject.CompareTag("Player") && !isPickedUp && !isCollected)
        {
            // Kiểm tra xem player đã có animal chưa
            if (PlayerController.Instance != null && !PlayerController.Instance.HasCarriedAnimal())
            {
                // Lấy AnimalPoint từ PlayerController
                Transform animalPoint = PlayerController.Instance.GetAnimalPoint();
                
                // Lượm animal (chưa tính điểm)
                PickupAnimal(animalPoint);
                
                // Thông báo cho PlayerController
                PlayerController.Instance.PickupAnimal(this);
            }
        }
    }
    
    /// <summary>
    /// Lượm animal (chưa tính điểm, chỉ khi thả tại checkpoint mới tính)
    /// </summary>
    /// <param name="animalPoint">Điểm để hiển thị animal khi đã lượm</param>
    public void PickupAnimal(Transform animalPoint)
    {
        if (isPickedUp || isCollected) return;
        
        isPickedUp = true;
        
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
        
        // Nếu có animalPoint, di chuyển animal đến đó
        if (animalPoint != null)
        {
            // Tắt các component không cần thiết
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
            
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            
            // Tắt AI/NavMeshAgent nếu có
            UnityEngine.AI.NavMeshAgent navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent != null) navAgent.enabled = false;
            
            // Di chuyển animal đến animalPoint và set parent
            transform.SetParent(animalPoint);
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero; // Đặt ở vị trí 0 vì chỉ có 1 con
        }
        else
        {
            // Nếu không có animalPoint, ẩn animal
            gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Thả animal tại checkpoint và tính điểm
    /// </summary>
    /// <param name="checkpointPosition">Vị trí checkpoint để thả animal</param>
    public void DropAnimalAtCheckpoint(Transform checkpointPosition)
    {
        if (!isPickedUp) return;
        
        isCollected = true;
        isPickedUp = false;
        
        // Remove parent
        transform.SetParent(null);
        
        // Đặt animal tại checkpoint
        if (checkpointPosition != null)
        {
            transform.position = checkpointPosition.position;
            transform.rotation = checkpointPosition.rotation;
        }
        
        // Bật lại các component để animal có thể hiển thị tại checkpoint
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Giữ kinematic để không rơi
        }
        
        // Không bật lại NavMeshAgent vì animal đã được thả
    }
    
    /// <summary>
    /// Collect animal này (có thể gọi từ bên ngoài, ví dụ từ PlayerController)
    /// DEPRECATED: Dùng PickupAnimal và DropAnimalAtCheckpoint thay thế
    /// </summary>
    /// <param name="animalPoint">Điểm để hiển thị animal khi đã nhặt (nếu null sẽ ẩn animal)</param>
    public void CollectAnimal(Transform animalPoint = null)
    {
        PickupAnimal(animalPoint);
    }
    
    /// <summary>
    /// Reset animal để có thể collect lại (dùng khi restart level)
    /// </summary>
    public void ResetAnimal()
    {
        isCollected = false;
        isPickedUp = false;
        
        // Bật lại các component
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;
        
        // Bật lại AI/NavMeshAgent nếu có
        UnityEngine.AI.NavMeshAgent navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navAgent != null) navAgent.enabled = true;
        
        // Remove parent nếu đang là con của AnimalPoint
        if (transform.parent != null && transform.parent.name == "AnimalPoint")
        {
            transform.SetParent(null);
        }
        
        gameObject.SetActive(true);
    }
}

