using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed = 2f;
    public int health = MAX_HEALTH;
    private float deathTimer = 5f; // seconds
    private float attackTimer = 0f;
    public float moveSpeed = 5f;
    public GameObject cameraAttachment;
    public float cameraMinAngle = 10, cameraMaxAngle = 40;
    public LevelBuilder levelBuilder;
    public uint levelsCleared;
    Animator animator;
    private uint timesTricked;
    public IndicatorText indicator;
    public int damage = 1;
    public bool hasFeather;
    public int gameMode; // 1 for story 2 for delve

    private bool isDead = false;
    private float currentTurn = 0;
    private bool isGuarding = false;
    private bool hasKey = false;
    private float defaultCameraAngle;
    private Vector3 spawner;
    private float blockTimer = 0;
    private float distToGround;

    public AudioClip attackSound, eSound, rSound, fSound;
    public AudioClip blockSound, hurtSound;
    public AudioClip[] secretSounds;
    private AudioSource audioSource;

    public const int MAX_HEALTH = 100;
    public const int POTION_HEAL_VALUE = 20;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name.CompareTo("Level1") == 0)
        { // if level1 is loaded that means we are doing story
            gameMode = 1;
            hasFeather = false;
        } else
        {
            gameMode = 2;
            hasFeather = true;
        }

        animator = GetComponent<Animator>();
        defaultCameraAngle = cameraAttachment.transform.localEulerAngles.x;
        levelsCleared = 0;
        timesTricked = 0;
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            CheckDeath();
            StopAttack();
            CheckAttack();

            if (Input.GetKeyDown("space") && hasFeather && isGrounded()) 
            { // jumping
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 5, 0);
                animator.SetBool("Jumping", true);
            }

            // move character
            float moveStrafe = Input.GetAxis("Horizontal");
            float moveForward = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveStrafe, 0, moveForward);
            transform.Translate(movement * Time.deltaTime * moveSpeed);
            transform.Rotate(0, Input.GetAxis("Mouse X") * turnSpeed, 0);

            // animate the movement
            animator.SetFloat("Forward", moveForward);
            animator.SetFloat("Turn", currentTurn);
            animator.SetFloat("Strafe", moveStrafe);

            cameraAttachment.transform.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);
            // Nudge the camera back in bounds
            float cameraAngle = cameraAttachment.transform.localEulerAngles.x;
            if (cameraAngle >= 180)
            {
                cameraAngle -= 360;
            }
            if (cameraAngle < cameraMinAngle || cameraAngle > cameraMaxAngle)
            {
                float delta = defaultCameraAngle - cameraAngle;
                cameraAttachment.transform.Rotate(Time.deltaTime * delta, 0, 0);
            }
        }
        else if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                DestroyOtherwisePersistentObjects();
                MainMenuController.DisplayMenu(Menu.Lose);
            }
        }
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void CheckDeath()
    {
        if (health <= 0)
        {
            isDead = true;
            animator.SetBool("isDead", true);
        }
    }
    void StopAttack()
    {
        animator.SetBool("Attacking", false);
        animator.SetBool("E Ability", false);
        animator.SetBool("F Ability", false);
        animator.SetBool("R Ability", false);
        animator.SetBool("Guarding", false);
        animator.SetBool("isHit", false);
        animator.SetBool("Jumping", false);
        if (blockTimer <= 0)
        {
            isGuarding = false;
        }
        blockTimer -= Time.deltaTime;
    }

    bool CheckAttack()
    {
        if (Input.GetMouseButtonDown(0))
        { // if player is normal attacking
            audioSource.PlayOneShot(attackSound);
            animator.SetBool("Attacking", true);
            animator.SetBool("Guarding", false);
            return true;
        }
        else if (Input.GetKeyDown("e"))
        { // if player is using e ability
            audioSource.PlayOneShot(eSound);
            animator.SetBool("E Ability", true);
            animator.SetBool("Guarding", false);
            return true;
        }
        else if (Input.GetKeyDown("r"))
        { // if player is using r ability
            audioSource.PlayOneShot(rSound);
            animator.SetBool("R Ability", true);
            animator.SetBool("Guarding", false);
            return true;
        }
        else if (Input.GetKeyDown("f"))
        { // if player is using f ability
            audioSource.PlayOneShot(fSound);
            animator.SetBool("F Ability", true);
            animator.SetBool("Guarding", false);
            return true;
        }
        else if (Input.GetMouseButtonDown(1))
        { // if player is blocking
            animator.SetBool("Guarding", true);
            isGuarding = true;
            blockTimer = 1f;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void takeBlockableDamageFromEnemy(int amount)
    {
        if (isGuarding)
        {
            audioSource.PlayOneShot(blockSound);
        }
        else
        {
            audioSource.PlayOneShot(hurtSound);
            health -= amount;
            animator.SetBool("isHit", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy 1"))
        {
            takeBlockableDamageFromEnemy(damage);
        }

        if (other.gameObject.CompareTag("Enemy 2"))
        {
            takeBlockableDamageFromEnemy(damage * 2);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            takeBlockableDamageFromEnemy(damage * 5);
        }

        if (other.gameObject.CompareTag("Key Chest"))
        {
            GameObject.FindGameObjectWithTag("Floor Door").GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<Animation>().Play("Chest Opening");
            hasKey = true;
        }

        if (other.gameObject.CompareTag("Floor Door") && hasKey)
        {
            audioSource.PlayOneShot(secretSounds[Random.Range(0, secretSounds.Length)]);
            other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (other.gameObject.CompareTag("Stairs"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (other.gameObject.CompareTag("Wind Switch"))
        {
            audioSource.PlayOneShot(secretSounds[Random.Range(0, secretSounds.Length)]);
            GameObject.FindGameObjectWithTag("Wind Wall").SetActive(false);
        }

        if (other.gameObject.CompareTag("Death"))
        {
            health = 0;
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            transform.position = new Vector3(-15, 4, 33);
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            timesTricked++;
            indicator.Tricked(timesTricked);
        }

        if (other.gameObject.CompareTag("Next Level"))
        {
            if (gameMode == 1)
            {
                Destroy(this.levelBuilder.gameObject);
                SceneManager.LoadScene("Boss");
                spawner = new Vector3(52.05f, 0.1f, 13);
            }
            else
            {
                levelsCleared++;
                transform.position = new Vector3(-15, 20, 30);
                transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                levelBuilder.levelFinished = true;
                timesTricked = 0;
                indicator.LevelCleared(levelsCleared);
            }
        }

        if (other.gameObject.CompareTag("Spawner"))
        {
            spawner = other.transform.position;
        }

        if (other.gameObject.CompareTag("Fall"))
        {
            transform.position = spawner;
            health -= 3;
        }

        if (other.gameObject.CompareTag("Win Game"))
        {
            transform.position = new Vector3(2, .3f, 5);
            transform.rotation = new Quaternion(0, 90, 0, 0);
            SceneManager.LoadScene("Win");
            MainMenuController.DisplayMenu(Menu.Win);
        }
    }

    internal void DestroyOtherwisePersistentObjects()
    {
        GameController.UngrabCursor();
        // Destoy objects marked as DontDestroy, so that they don't persist into the menu.
        Destroy(this.gameObject);
        Destroy(this.levelBuilder.gameObject);
    }

    private IEnumerator Die(Animation animation)
    {
        animation.Play("Trap Door");
        yield return new WaitForSeconds(animation["Trap Door"].length * animation["Trap Door"].speed);
    }
}
