using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public bool legSwrd;
    public bool legShield;
    public int healthPots;
    public GameObject legendarySword;
    public GameObject normalSword;
    public GameObject legendaryShield;
    public GameObject normalShield;
    public PlayerController player;

    public GameObject pauseMenu;
    public TextMeshProUGUI inventoryText;
    public Button usePotionButton;

    // Start is called before the first frame update
    void Start()
    {
        healthPots = 0;
        legSwrd = false;
        legShield = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeSelf)
            {
                ShowPauseScreen();
            }
            else
            {
                HidePauseScreen();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chest"))
        {
            other.GetComponent<Animation>().Play("Chest Opening");
            if (!legSwrd)
            {
                legSwrd = true;
                Destroy(normalSword);
                legendarySword.SetActive(true);
            }
            else if (!legShield)
            {
                legShield = true;
                Destroy(normalShield);
                legendaryShield.SetActive(true);
            }
            else
            {
                healthPots++;
            }
            other.GetComponent<BoxCollider>().isTrigger = false;
        }

        if (other.gameObject.CompareTag("Wind Chest"))
        {
            player.hasFeather = true;
            other.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    public void ShowPauseScreen()
    {
        GameController.UngrabCursor();
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        RebuildText();
    }

    public void HidePauseScreen()
    {
        GameController.GrabCursor();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitClicked()
    {
        Time.timeScale = 1;
        player.DestroyOtherwisePersistentObjects();
        MainMenuController.DisplayMenu(Menu.Start);
    }

    const string TEXT_TEMPLATE = @"Inventory

Potions: {0}
Sword: {1}
Shield: {2}
Feather: {3}";

    public void RebuildText()
    {
        inventoryText.text = string.Format(TEXT_TEMPLATE, healthPots, legSwrd ? "Legendary" : "Normal", legSwrd ? "Legendary" : "Normal", player.hasFeather ? "Yes" : "No");
        usePotionButton.interactable = (healthPots > 0);
    }

    public void UsePotion()
    {
        if (healthPots > 0)
        {
            healthPots--;
            player.health = Mathf.Clamp(player.health + PlayerController.POTION_HEAL_VALUE, 0, PlayerController.MAX_HEALTH);
            RebuildText();
        }
    }
}
