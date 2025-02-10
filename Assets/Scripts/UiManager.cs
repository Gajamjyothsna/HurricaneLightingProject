using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region Private variables
    [Header("UI Components")]
    [SerializeField] private GameObject contentToastUI;
    [SerializeField] private Text contentToastTMP;
    [SerializeField] private Button protectButton;

    [SerializeField] private ThirdPersonController player;
    [SerializeField] private Transform vehiclePosition;
    [SerializeField] private Canvas canvas;

    [SerializeField] private Transform vehicleCovertransform;
    [SerializeField] private GameObject vehicleCover;


    private bool isInitiated;
    private Animator playerAnimator;
    private CharacterController characterController;
    private float moveSpeed = 5f; // Adjust speed
    private float rotationSpeed = 10f; // Speed to rotate towards vehicle
    private bool isVehiceInstructionprovided;

    #endregion

    public static UiManager Instance { get; private set; }

    #region MonoBehaviour Methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        characterController = player.GetComponent<CharacterController>();
        playerAnimator = player.GetComponent<Animator>();

        if (playerAnimator == null)
        {
            Debug.LogError("Animator is missing on the player!");
        }

        StartCoroutine(InitiateUIContent());
    }

    private void Update()
    {
        if (isInitiated && Input.GetKeyDown(KeyCode.J))
        {
            MovePlayerTowardsVehicle();
        }
        else if(isVehiceInstructionprovided && Input.GetKeyDown(KeyCode.J))
        {
            CoverVehicle();
        }
    }

    private void CoverVehicle()
    {
        vehicleCover.SetActive(true);
        isVehiceInstructionprovided = false;
    }

    

    #endregion

    #region Button Actions
    public void MovePlayerTowardsVehicle()
    {
        Debug.Log("ProtectButtonClick");
        StartCoroutine(MovePlayerToVehicle());
    }
    private IEnumerator MovePlayerToVehicle()
    {
        if (characterController == null || playerAnimator == null)
        {
            Debug.LogError("CharacterController or Animator is missing!");
            yield break;
        }

        // Start running animation
        playerAnimator.SetBool("run", true);

        while (Vector3.Distance(player.transform.position, vehiclePosition.position) > 0.1f) // Keep moving closer
        {
            // Calculate direction
            Vector3 direction = (vehiclePosition.position - player.transform.position).normalized;
            direction.y = 0; // Prevent unwanted vertical movement

            // Rotate towards target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards the target
            characterController.Move(direction * moveSpeed * Time.deltaTime);

            yield return null;
        }

        // Ensure exact positioning
        player.transform.position = vehiclePosition.position;

        // Small delay for smooth transition
        yield return new WaitForSeconds(0.3f);

        // Stop running animation
        playerAnimator.SetBool("run", false);

        // Show message after reaching vehicle
        yield return new WaitForSeconds(0.5f);  // Small delay to avoid abrupt transition
        
    }



    public void NextCommand()
    {
        contentToastTMP.text = "Protect the Car from Hurricane. Click J button to cover the Car.";
        isVehiceInstructionprovided = true;
        isInitiated = false;
    }


    #endregion

    #region Private Methods
    private IEnumerator InitiateUIContent()
    {
        yield return new WaitForSeconds(5f);
        contentToastUI.SetActive(true);
        isInitiated = true;
    }
    #endregion
}
