using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Text LState;
    public Text LInfo;
    public Button LSkip;
    public Text RState;
    public Text RInfo;
    public Button RSkip;
    public bool turn = true;
    public Image LPanel;
    public Image RPanel;
    private string currentState = "";
    public Text Timer;
    private int time = 30;
    public RectTransform GameOver;
    public Text TxtGameOver;
    public Button Surrender;
    public Image SurrenderScreen;
    public Button YesSurrenderButton;
    public Button NoSurrenderButton;

    public AudioSource audioSource;
    public AudioClip audioClip;

    public void Start()
    {
        RPanel.color = UnityEngine.Color.gray;
        RSkip.gameObject.SetActive(false);
        StartCoroutine(Countdown());
        GameOver.gameObject.SetActive(false);
        TxtGameOver.gameObject.SetActive(false);
        SurrenderScreen.gameObject.SetActive(false);
        YesSurrenderButton.gameObject.SetActive(false);
        NoSurrenderButton.gameObject.SetActive(false);
        audioSource.clip = audioClip;
    }

    public void DisplayButton()
    {
        if (!turn)
        {
            LSkip.gameObject.SetActive(true);
            RSkip.gameObject.SetActive(false);

        }
        else
        {
            LSkip.gameObject.SetActive(false);
            RSkip.gameObject.SetActive(true);
        }
    }

    public void ChangeButtonText(string state)
    {
        string newText;
        if (state == "")
        {
            newText = "End Turn";
        }
        else
        {
            newText = "Next Phase";
        }

        if (!turn)
        {
            LSkip.transform.Find("Text").GetComponent<Text>().text = newText;
        }
        else
        {
            RSkip.transform.Find("Text").GetComponent<Text>().text = newText;
        }
    }

    public void UpdateState(string input)
    {
        if (!turn)
        {
            LState.text = input;           
        }
        else
        {
            RState.text = input;
        }
        currentState = input;
        ChangeButtonText(input);
    } 

    public void UpdatePanel()
    {
        if (RPanel.color == UnityEngine.Color.gray)
        {
            RPanel.color = UnityEngine.Color.red;
            LPanel.color = UnityEngine.Color.gray;
            LInfo.text = "";
        }
        else
        {
            RPanel.color = UnityEngine.Color.gray;
            LPanel.color = UnityEngine.Color.blue;
            RInfo.text = "";
        }
    }

    public void ShowDescription(TacticsPiece piece, bool belongToSelf)
    {

        string text = "Character: " + piece.name + "\n" + "HP: " + piece.health + "/" + piece.maxHealth + "\n";

        if (piece.name == "Priest")
        {
            text = text + "Healing: " + piece.attack + "\n" + "Armor: " + piece.armor + "% reduction in damage" + "\n" + "Cooldown: " + piece.cooldown;
        }
        else
        {
            text = text + "Damage: " + piece.attack + "\n" + "Armor: " + piece.armor + "% reduction in damage" + "\n" + "Cooldown: " + piece.cooldown;
        }

        if (!belongToSelf)
        {
            text = "Enemy " + text;
        }


        if (!turn)
        {
            LInfo.text = text;
        }
        else
        {
            RInfo.text = text;
        }

        
    }

    public void EndTurn()
    {
        audioSource.Play();
        if (currentState == "")
        {
            GameManager.instance.NextPlayer();
        }
        else
        {
            if (currentState == "Move")
            {
                var moveSelector = GameObject.Find("Board").GetComponent<MoveSelector>();
                moveSelector.ExitMovementState();
            }
            else
            {
                if (currentState == "Attack")
                {
                    var attackSelector = GameObject.Find("Board").GetComponent<AttackSelector>();
                    attackSelector.ExitAttackState();
                }
                else
                {
                    var directionSelector = GameObject.Find("Board").GetComponent<NewDirectionSelector>();
                    directionSelector.ExitDirectionState();
                }
            }
        }        
    }

    private void ForceEndTurn()
    {
        if (currentState == "")
        {
            GameManager.instance.NextPlayer();
        }
        else
        {
            while (currentState != "")
            {
                EndTurn();
            }
        }
    }

    IEnumerator Countdown()
    {
        bool thisTurn = turn;
        while (time > 0)
        {
            if (thisTurn != turn)
            {
                time = 30;
                thisTurn = turn;
            }
            Timer.text = time.ToString();
            time--;
            yield return new WaitForSeconds(1.0f);

            if (time == 0)
            {
                time = 30;
                ForceEndTurn();
            }
        }
    }

    public void DisplayEndScreen(string name)
    {
        GameOver.gameObject.SetActive(true);
        TxtGameOver.gameObject.SetActive(true);
        TxtGameOver.text = name.ToUpper() + " PLAYER WINS";
    }

    public void OnClickQuit()
    {
        audioSource.Play();
        SurrenderScreen.gameObject.SetActive(true);
        YesSurrenderButton.gameObject.SetActive(true);
        NoSurrenderButton.gameObject.SetActive(true);
        Surrender.gameObject.SetActive(false);
    }

    public void OnClickYes()
    {
        audioSource.Play();
        SurrenderScreen.gameObject.SetActive(false);
        YesSurrenderButton.gameObject.SetActive(false);
        NoSurrenderButton.gameObject.SetActive(false);
        GameManager.instance.EndGame(true);
    }

    public void OnClickNo()
    {
        audioSource.Play();
        SurrenderScreen.gameObject.SetActive(false);
        YesSurrenderButton.gameObject.SetActive(false);
        NoSurrenderButton.gameObject.SetActive(false);
        Surrender.gameObject.SetActive(true);
    }

}
