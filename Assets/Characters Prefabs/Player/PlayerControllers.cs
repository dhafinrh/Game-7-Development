using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

//Controller untuk pergerakan, animasi, dan serangan 
public class PlayerControllers : MonoBehaviour
{
    [SerializeField] SwordScript swordScript;
    [SerializeField] private float moveSpeed = 1f; // Kecepatan pergerakan karakter
    [SerializeField] private float moveDrag = 15f;
    [SerializeField] private float stopDrag = 25f;
    [SerializeField] private float idleFriction;
    [SerializeField] private float collisionOffset = 0.05f; // Jarak tambahan untuk menghindari tabrakan
    [SerializeField] private ContactFilter2D movementFilter; // Filter yang digunakan untuk mendeteksi tabrakan saat bergerak

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movementInput = Vector2.zero; // Nilai input untuk arah pergerakan
    private Rigidbody2D rb; // Referensi ke komponen Rigidbody2D
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>(); // List untuk menyimpan hasil tabrakan
    private bool canMove = true;

    private bool isMoving = false;
    public bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", value);

            if (isMoving == true)
                rb.drag = moveDrag;
            else
                rb.drag = stopDrag;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Mendapatkan komponen Rigidbody2D dari GameObject
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // private void Update()
    // {
    //     if (movementInput != Vector2.zero) // Memeriksa apakah ada input pergerakan
    //     {
    //         bool success = TryMove(movementInput); // Mencoba bergerak ke arah yang diinginkan

    //         if (!success)
    //         {
    //             success = TryMove(new Vector2(movementInput.x, 0)); // Jika pergerakan terhalang, coba bergerak secara horizontal
    //         }

    //         if (!success)
    //         {
    //             success = TryMove(new Vector2(0, movementInput.y)); // Jika pergerakan horizontal terhalang, coba bergerak secara vertikal
    //         }

    //         animator.SetBool("isMoving", success);
    //     }
    //     else
    //     {
    //         animator.SetBool("isMoving", false);
    //     }

    //     //Mengecek arah gerakan untuk disesuaikan dengan animasi jalan
    //     if (movementInput.x < 0)
    //     {
    //         spriteRenderer.flipX = true;
    //         swordScript.AttackLeft();
    //     }
    //     else if (movementInput.x > 0)
    //     {
    //         spriteRenderer.flipX = false;
    //         swordScript.AttackRight();
    //     }
    // }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero && canMove)
        {
            //rb.velocity = Vector2.ClampMagnitude(rb.velocity + (movementInput * moveSpeed * Time.deltaTime), maxSpeed);

            rb.AddForce(movementInput * moveSpeed * Time.deltaTime, ForceMode2D.Force);

            IsMoving = true;

            if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
                swordScript.AttackRight();
            }
            else if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
                swordScript.AttackLeft();
            }
        }
        else
        {
            IsMoving = false;
        }
    }

    // private bool TryMove(Vector2 direction)
    // {
    //     if (direction != Vector2.zero)
    //     {
    //         int count = rb.Cast(
    //                 direction,
    //                 movementFilter,
    //                 castCollisions,
    //                 moveSpeed * Time.fixedDeltaTime + collisionOffset // Mengecek tabrakan dengan menjalankan sinar dalam arah yang diberikan
    //             );

    //         if (count == 0) // Jika tidak ada tabrakan yang terdeteksi
    //         {
    //             rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime); // Bergerakkan Rigidbody2D ke arah yang diinginkan
    //             return true;
    //         }
    //         else
    //         {
    //             return false; // Tabrakan terdeteksi, pergerakan tidak berhasil
    //         }
    //     }
    //     else
    //     {
    //         //Return false kalau ada objek yang menghalangi pergerakan
    //         return false;
    //     }
    // }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>(); // Mendapatkan nilai input pergerakan dari Input System
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
}
