using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// this class is used to manage and display player's data
public class PlayerDataController : MonoBehaviour
{
    public int initialMoney;
    public int playerHealth = 100;
    public int turretNumber = 0;
    private int playerMoney;
    private int waveCount = 0;
    public Text healthText;
    public Text moenyText;
    public Text waveText;
    public Text turretText;
    public GameObject loseWindow;

    public static PlayerDataController PlayerData;
    void Awake()
    {
        this.playerMoney = initialMoney;
        moenyText.text = "$ " + playerMoney;
        waveText.text = "" + waveCount+"/7";
        healthText.text = "" + playerHealth + "/100";
        turretText.text = "" + turretNumber;
        PlayerData = this;
    }


    public int getCurrentMoney()
    {
        return this.playerMoney;
    }
    public void costMoney(int money)
    {
        this.playerMoney -= money;
        moenyText.text = "$" + playerMoney;
    }

    public void addMoney(int money)
    {
        this.playerMoney += money;
        moenyText.text = "$" + playerMoney;
    }
    public void loseHealth(int damage)
    {
        this.playerHealth -= damage;
        if (playerHealth <= 0)
        {
            loseWindow.SetActive(true);
        }
        healthText.text = playerHealth + "/100";
    }

    public void addTurretNumber()
    {
        turretNumber++;
        turretText.text = "" + turretNumber;
    }
    public void decreaseTurretNumber()
    {
        turretNumber--;
        turretText.text = "" + turretNumber;
    }
    public void addWave()
    {
        waveCount++;
        waveText.text = "" + waveCount + "/7";
    }
    public void reLoad()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void quit()
    {
        Application.Quit();
    }
}
