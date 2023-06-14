using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerControllers : MonoBehaviour
{
    [Header("Moving Properties")]
    [SerializeField] private HealthManager healthManager;
    [SerializeField] SwordScript swordScript;
    [SerializeField] private float moveSpeed = 1f; // Kecepatan pergerakan karakter
    [SerializeField] private float moveDrag = 15f;
    [SerializeField] private float stopDrag = 25f;
    [SerializeField] private float idleFriction;
    [SerializeField] private float collisionOffset = 0.05f; // Jarak tambahan untuk menghindari tabrakan
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    private Vector2 movementInput = Vector2.zero; // Nilai input untuk arah pergerakan
    private Vector2 lastMovementDirection = Vector2.right;
    [SerializeField] private Rigidbody2D rb; // Referensi ke komponen Rigidbody2D
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

    [Header("Grab Properties")]
    [SerializeField] private Transform RayPoint;
    [SerializeField] private Transform GrabPoint;
    [SerializeField] private float RayDistance;
    [SerializeField] private UnityEvent<TrashType> OnTrashCollected;
    private GameObject grabbedObject = null;
    private Canvas canvasPickUp;
    private int layerIndex;
    private TrashScript currentTrashType;
    public GameObject GrabbedObject { get => grabbedObject; set { grabbedObject = value; } }
    public TrashScript CurrentTrashType { get => currentTrashType; }

    [Header("Throw Properties")]
    [SerializeField] private List<GameObject> throwablePrefabs;
    [SerializeField] private List<GameObject> activeThrowable;
    [SerializeField] private int maxThrowables;
    [SerializeField] private float throwPower = 10f;
    [SerializeField] private float throwRange = 10f;
    [SerializeField] Vector2 throwDirection = new Vector2(1, 0);
    [SerializeField] Image arrowAim;
    [SerializeField] private float throwCoolDown;
    [SerializeField] private UnityEvent<float> OnThrowCoolDown;
    private int currentThrowablesLeft;
    private int currentThrowableIndex = 0;
    private GameObject currentThrowable;
    bool isAiming = false;
    bool isThrowCoolDown;
    float throwTimer;

    [Header("Other Properties")]
    [SerializeField] private int maxHeal;
    [SerializeField] private float healAmount;
    [SerializeField] private float healCoolDown;
    [SerializeField] private UnityEvent<float> onHealCoolDown;
    [SerializeField] private UnityEvent<int> HealLeftUpdate;
    private int currentHealLeft;

    [Header("Other Properties")]
    [SerializeField] private float slipperyDuration = 1f;
    private bool isSlippery = false;
    private float slipperyTimer = 1f;
    private float healTimer = 0f;
    private bool hasHealed = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        layerIndex = LayerMask.NameToLayer(layerName: "GrabAble");

        currentThrowablesLeft = maxThrowables;
        currentHealLeft = maxHeal;
    }

    private void Update()
    {
        if (healTimer > 0)
        {
            healTimer -= Time.deltaTime;
            if (healTimer <= 0f)
            {
                hasHealed = false;
            }
        }

        if (throwTimer > 0)
        {
            throwTimer -= Time.deltaTime;
            if (throwTimer <= 0f)
            {
                isThrowCoolDown = false;
            }
        }

        //Choosing Throwable
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SetCurrentThrowableIndex(0);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SetCurrentThrowableIndex(1);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SetCurrentThrowableIndex(2);
        }
        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            if (hasHealed == false)
                HealUp(healAmount);
        }

        //Throw
        if (!isThrowCoolDown)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame && !isAiming)
            {
                StartAim();
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame && isAiming)
            {
                CancelAim();
            }
            else if (Mouse.current.rightButton.wasReleasedThisFrame && isAiming)
            {
                Throw();
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!success)
            {
                success = TryMove(new Vector2(0, movementInput.y));
            }

            IsMoving = success;

            if (movementInput.x > 0)
            {
                //GrabPoint.localPosition = new Vector2(0.1f, 0f);
                spriteRenderer.flipX = false;
                throwDirection = new Vector2(1, 0f);
                swordScript.AttackRight();
            }
            else if (movementInput.x < 0)
            {
                //GrabPoint.localPosition = new Vector2(-0.1f, 0f);
                spriteRenderer.flipX = true;
                throwDirection = new Vector2(-1, 0f);
                swordScript.AttackLeft();
            }

            if (movementInput != Vector2.zero)
            {
                lastMovementDirection = movementInput.normalized;
            }
        }
        else
        {
            IsMoving = false;
        }

        //Aim Arrow
        if (Mouse.current.rightButton.isPressed && isAiming)
        {
            UpdateAim();
        }

        //Grab
        Collider2D[] colliders = Physics2D.OverlapCircleAll(RayPoint.position, RayDistance);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == layerIndex)
            {
                canvasPickUp = collider.gameObject.GetComponentInChildren<Canvas>();
                if (canvasPickUp != null && grabbedObject == null)
                    canvasPickUp.enabled = true;

                if (Keyboard.current.spaceKey.wasReleasedThisFrame && grabbedObject == null)
                {
                    grabbedObject = collider.gameObject;
                    GameObject playerParent = transform.parent.gameObject;
                    grabbedObject.transform.SetParent(playerParent.transform);
                    grabbedObject.transform.localPosition = GrabPoint.localPosition;
                    currentTrashType = grabbedObject.GetComponent<TrashScript>();
                }
            }
            else if (collider.gameObject.layer != layerIndex && canvasPickUp != null)
            {
                canvasPickUp.enabled = false;
            }
        }

        if ((Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }


        if (isSlippery)
        {
            slipperyTimer -= Time.fixedDeltaTime;
            if (slipperyTimer <= 0f)
            {
                RemoveSlipperyEffect();
            }
        }
    }

    public void HealUp(float healAmount)
    {
        if (healthManager.Health < healthManager.maxHealth)
        {
            if (currentHealLeft > 0)
            {
                currentHealLeft--;

                HealLeftUpdate.Invoke(currentHealLeft);


                healthManager.Health += healAmount;

                if (healthManager.Health > healthManager.maxHealth)
                    healthManager.Health = healthManager.maxHealth;

                healTimer = healCoolDown;
                onHealCoolDown.Invoke(healCoolDown);
                healthManager.OnHitUpdateUI();
                hasHealed = true;
            }
        }
    }

    private void SetCurrentThrowableIndex(int index)
    {
        if (index >= 0 && index < throwablePrefabs.Count)
        {
            currentThrowableIndex = index;
            UpdateThrowableUI();
        }
    }

    private void UpdateThrowableUI()
    {
        foreach (GameObject active in activeThrowable)
        {
            if (active != null)
                active.SetActive(false);
        }
        for (int i = 0; i < activeThrowable.Count; i++)
        {
            if (activeThrowable[i] != null)
            {
                currentThrowable = activeThrowable[i].gameObject;
                currentThrowable.SetActive(i == currentThrowableIndex);
            }
        }
    }

    private void StartAim()
    {
        isAiming = true;
        arrowAim.enabled = true;
    }

    private void UpdateAim()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Calculate the direction from the player to the mouse position
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Find the closest direction from the eight possible directions
        Vector2 closestDirection = Vector2.up;
        float closestAngle = Vector2.Angle(Vector2.up, direction);
        float[] angles = { 45f, 90f, 135f, 180f, -45f, -90f, -135f, -180f };
        foreach (float angleDiff in angles)
        {
            Vector2 dir = Quaternion.Euler(0f, 0f, angleDiff) * Vector2.up;
            float angle = Vector2.Angle(dir, direction);
            if (angle < closestAngle)
            {
                closestDirection = dir;
                closestAngle = angle;
            }
        }

        //Defining the actual throwing direction
        Vector2 arrowAimDirection = closestDirection;

        // Calculate the position and rotation of the aim arrow based on the throwDirection
        Vector2 arrowEndPosition = (Vector2)transform.position + arrowAimDirection * throwRange;
        arrowAim.transform.position = new Vector2(arrowEndPosition.x, arrowEndPosition.y - 0.1f);

        float throwAngle = Mathf.Atan2(arrowAimDirection.y, arrowAimDirection.x) * Mathf.Rad2Deg;
        arrowAim.transform.rotation = Quaternion.AngleAxis(throwAngle, Vector3.forward);
    }

    private void Throw()
    {
        CancelAim();
        if (currentThrowablesLeft > 0 && !isThrowCoolDown)
        {
            GameObject chosenThrowable = null;
            Vector2 throwDirection = arrowAim.transform.right;

            // Use a switch statement to check the value of currentThrowableIndex
            switch (currentThrowableIndex)
            {
                case 0:
                    // Instantiate throwable of type 0
                    chosenThrowable = Instantiate(throwablePrefabs[0], transform.position, Quaternion.identity);

                    Rigidbody2D throwableRb = chosenThrowable.GetComponent<Rigidbody2D>();
                    throwableRb.AddForce(throwDirection * throwPower, ForceMode2D.Impulse);
                    break;
                case 1:
                    // Instantiate multiple throwables of type 1
                    int numThrowables = 8; // Number of throwables to instantiate
                    float angleBetweenThrowables = 360f / numThrowables; // Angle between each throwable in degrees

                    for (int i = 0; i < numThrowables; i++)
                    {
                        chosenThrowable = Instantiate(throwablePrefabs[1], transform.position, Quaternion.identity);
                        activeThrowable.Add(chosenThrowable);

                        // Set the direction and force of the throw
                        float angle = i * angleBetweenThrowables;
                        throwDirection = Quaternion.Euler(0, 0, angle) * Vector2.right;
                        Rigidbody2D throwableRb2 = chosenThrowable.GetComponent<Rigidbody2D>();
                        throwableRb2.AddForce(throwDirection * throwPower, ForceMode2D.Impulse);
                    }
                    break;
                    // Add more cases here for other types of throwables
            }

            if (chosenThrowable != null)
            {
                currentThrowablesLeft--;
                isThrowCoolDown = true;
                OnThrowCoolDown.Invoke(throwCoolDown);
                throwTimer = throwCoolDown;
            }
        }
    }

    private void CancelAim()
    {
        arrowAim.enabled = false;
        isAiming = false;
    }

    public void TrashCollected()
    {
        if (grabbedObject != null)
        {
            TrashScript trashScript = grabbedObject.GetComponent<TrashScript>();
            OnTrashCollected.Invoke(trashScript.TrashType);
            Invoke("DestroyTrash", 0.1f);
        }
    }

    private void DestroyTrash()
    {
        Destroy(grabbedObject);
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            Vector2 targetPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

            // Collider2D[] colliders = Physics2D.OverlapBoxAll(targetPosition, boxCollider.size + new Vector2(0.1f, 0.1f), 0f);

            if (isSlippery)
            {
                targetPosition = rb.position + direction * moveSpeed * 2.5f * Time.fixedDeltaTime;
            }

            // else
            // {
            //     foreach (Collider2D collider in colliders)
            //     {
            //         if (collider.gameObject.layer == LayerMask.NameToLayer("TileMap") && !collider.isTrigger)
            //         {
            //             Vector2 safeDirection = Vector2.zero;

            //             if (direction.x > 0f)
            //                 safeDirection = Vector2.left;
            //             else if (direction.x < 0f)
            //                 safeDirection = Vector2.right;
            //             else if (direction.y > 0f)
            //                 safeDirection = Vector2.down;
            //             else if (direction.y < 0f)
            //                 safeDirection = Vector2.up;

            //             targetPosition = rb.position + safeDirection * moveSpeed * Time.fixedDeltaTime;

            //             break;
            //         }
            //     }
            // }

            rb.MovePosition(targetPosition); // Move the character to the safe target position

            return true;
        }
        else
        {
            // Return false if there is no movement direction
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>(); // Mendapatkan nilai input pergerakan dari Input System
    }

    void OnAttack()
    {
        animator.SetTrigger("swordAttack");

        if (isAiming)
            CancelAim();
    }

    public void ApplySlipperyEffect()
    {
        isSlippery = true;
        slipperyTimer = slipperyDuration;
    }

    private void RemoveSlipperyEffect()
    {
        isSlippery = false;
    }
}
