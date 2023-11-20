using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float limitSpeed = 10f;
    public float maxJumpHeight = 10f;
    public float acceleration = 10f;
    public GameObject playerCamera;
    public GameObject tutorialTextHolder;
    public string nextSceneName; 

    private bool isGrounded;
    private Vector3 respawnPoint;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private float horizontal;
    private bool isStill;
    private GridController gridController;
    private TextMeshProUGUI tutorialText;
    private AudioSource audioSource;

    private AsyncOperation asyncLoad;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        isGrounded = false;
        respawnPoint = this.transform.position;
        gridController = gameObject.GetComponent<GridController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int currentLevelInt = int.Parse(sceneName.Substring(sceneName.Length - 1));
        if (currentLevelInt > PlayerPrefs.GetInt("latestLevel"))
        {
            PlayerPrefs.SetInt("latestLevel", currentLevelInt);
        }
        tutorialText = tutorialTextHolder.GetComponent<TextMeshProUGUI>();
        tutorialTextHolder = tutorialTextHolder.transform.parent.gameObject;
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && isStill && isGrounded && transform.parent != null && !tutorialTextHolder.activeInHierarchy)
        {
            this.enabled = false;
            gridController.enabled = true;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StartCoroutine(LoadLevel("Menu"));
        }

        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            die();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * acceleration, rb.velocity.y);

        if (Math.Abs(rb.velocity.x) > limitSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x < 0 ? -limitSpeed : limitSpeed, rb.velocity.y);
        }

        isStill = rb.velocity.x == 0;
        
        // work out the player location/if they're grounded
        Bounds colliderBounds = bc.bounds;
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, 0f, 0f);

        Collider2D colliders = Physics2D.OverlapBox(groundCheckPos, new Vector3(colliderBounds.size.x * 0.9f, 0.01f, 0f), 0.0f, LayerMask.GetMask("Ground"));//3 is set to ground
        //check if player main collider is in the list of overlapping colliders
        if (colliders != null && !tutorialTextHolder.activeInHierarchy)
        {
            Debug.Log(colliders);
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            this.transform.parent = other.transform;
        }

        if (other.gameObject.layer == 6)
        {
            die();
        }

        if (other.gameObject.tag == "Finish")
        {
            //rb.velocity = new Vector2(0, 0);
            //this.transform.Translate(other.gameObject.transform.position - this.transform.position + (new Vector3(0, -0.5f, -0.1f)));
            StartCoroutine(LoadLevel(nextSceneName));
            asyncLoad.allowSceneActivation = true;
        }

        if (other.gameObject.tag == "Tutorial")
        {
            tutorialTextHolder.SetActive(true);
            tutorialText.text = other.gameObject.GetComponent<TextMeshProUGUI>().text;
            Destroy(other.gameObject);
            isGrounded = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            this.transform.parent = null;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (this.enabled == true && !tutorialTextHolder.activeInHierarchy)
        {
            horizontal = context.ReadValue<Vector2>().x;
        }
        else
        {
            horizontal = 0f;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!tutorialTextHolder.activeInHierarchy)
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && this.enabled == true)
                {
                    audioSource.Play();
                    rb.velocity = new Vector2(rb.velocity.x, maxJumpHeight);
                    isGrounded = false;
                }
            }
            else
            {
                tutorialTextHolder.SetActive(false);
            }
        }
        /**
        if (Input.GetKeyDown("space") && isGrounded)
        {
            isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, maxJumpHeight);
        }
        **/
    }


    //moves the player to a designated respawn point
    public void die()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    public IEnumerator LoadLevel(string nextSceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
    }

    void OnEnable()
    {
        playerCamera.SetActive(true);
    }

    void OnDisable()
    {
        if (playerCamera != null)
        {
            playerCamera.SetActive(false);
        }
    }

    //TODO:Add a mechanism preventing cells to be moved
    //Look into a save mechanism so one can continue at a specific level
    //Make a level selection screen
}
