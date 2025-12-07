using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public string speedParameter = "Speed";
    public string shootTriggerParameter = "Shoot";
    public string aimParameter = "Aiming";
    public string dieTriggerParameter = "Die";

    /// <summary>
    /// Được gọi từ PlayerController để cập nhật animation khi di chuyển
    /// </summary>
    /// <param name="isMoving">Nhân vật có đang di chuyển không</param>
    /// <param name="speed">Tốc độ di chuyển (0-1)</param>
    public void SetMovement(bool isMoving, float speed = 0f)
    {
        if (animator == null) return;
        
        if (!string.IsNullOrEmpty(speedParameter))
        {
            animator.SetFloat(speedParameter, speed);
        }
    }

    /// <summary>
    /// Được gọi từ PlayerController để trigger animation bắn
    /// </summary>
    public void SetShoot()
    {
        if (animator == null) return;

        if (!string.IsNullOrEmpty(shootTriggerParameter))
        {
            animator.SetTrigger(shootTriggerParameter);
        }
    }

    /// <summary>
    /// Được gọi từ PlayerController để set animation aim
    /// </summary>
    /// <param name="isAiming">Nhân vật có đang aim không</param>
    public void SetAim(bool isAiming)
    {
        if (animator == null) return;

        if (!string.IsNullOrEmpty(aimParameter))
        {
            animator.SetBool(aimParameter, isAiming);
        }
    }

    /// <summary>
    /// Được gọi từ PlayerHealth để trigger animation chết
    /// </summary>
    public void SetDie()
    {
        if (animator == null) return;

        if (!string.IsNullOrEmpty(dieTriggerParameter))
        {
            animator.SetTrigger(dieTriggerParameter);
        }
    }
}
