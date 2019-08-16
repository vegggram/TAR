using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsPiece : MonoBehaviour
{
    // Start is called before the first frame update

    public string name;
    public int maxHealth;
    public int health;
    public int[] dodge;
    public int armor;
    public int attack;
    public int attackType;
    public bool surehit;
    public int movement;
    public int direction;
    public int[] location;
    public GameObject attackAnimation;
    public GameObject walkAnimation;
    public int cooldown;
    public GameObject damagePopup;

    private AudioSource audioSource;
    //public AudioClip audioClip;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void castAttack()
    {
        //Instantiate(attackAnimation, transform.position, Quaternion.identity);
        //play attack animation
    }

    public int receiveAttack(int receivedAttack, bool surehit, int directionOfAttack, bool heal)
    {
       
        float discount = armor / 100f;
        int damage = Mathf.RoundToInt((1.0f - discount) * receivedAttack);

        //Debug.Log(dodge[Mathf.Abs(direction - directionOfAttack)]);
        //Debug.Log(direction);
        //Debug.Log(directionOfAttack);
        //if ((surehit) || Random.Range(1, 100) > dodge[Mathf.Abs(direction - directionOfAttack)])
        if ((surehit) || Random.Range(1, 100) > dodge[Mathf.Abs(direction - directionOfAttack)])
        {
            if (heal)
            {
                if (health + receivedAttack > maxHealth)
                {
                    showFloatingText(maxHealth - health, true);
                    health = maxHealth + 0;
                    
                }
                else
                {
                    health += receivedAttack;
                    showFloatingText(receivedAttack, false);
                }
                
                
            }
            else
            {
                health -= damage;
                showFloatingText(damage, false);
            }        
            
            if (health <= 0)
            {
                StartCoroutine(die());
            }
            //play receive attack animation
            GetComponent<Animator>().SetTrigger("receiveAttack");
            return damage;
        }
        block();
        showFloatingText(0, false);
        return 0;
    }

    public void showFloatingText(int dmg, bool heal)
    {
        var display = Instantiate(damagePopup, transform.position + new Vector3 (0,1f,0), Quaternion.Euler(new Vector3(50, 250, 15)));
        if (dmg > 0)
        {
            display.GetComponent<TextMesh>().text = dmg.ToString();
        }
        else
        {
            if (heal)
            {
                display.GetComponent<TextMesh>().text = "Full HP";
            }
            else
            {
                display.GetComponent<TextMesh>().text = "Blocked";

            }
            
        }

    }

    public void showCoolDown()
    {
        var display = Instantiate(damagePopup, transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(new Vector3(50, 250, 15)));
        display.GetComponent<TextMesh>().text = "Cooldown: " + cooldown.ToString();
    }

    public void block()
    {
        //play block
    }

    public IEnumerator die()
    {
        //play death animation
        GetComponent<Animator>().SetTrigger("die");
        audioSource.Play();
        //Debug.Log(audioSource.enabled);
        float transRate = 0.0f;
        while (transRate < 2.4f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 0, Time.deltaTime * transRate);
            transRate += Time.deltaTime / 1.0f;
            yield return null;
        }
        GetComponent<Animator>().SetTrigger("idle");
        Destroy(gameObject);
    }

    //IEnumerator delay()
    //{
    //    yield return new WaitForSeconds(2);
    //}

    public void move()
    {
        //move animation
    }

    public void avoid()
    {
        //avoid animation
    }

    public List<Vector2Int> MoveLocations(Vector2Int grid)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        //locations.Add(grid);
        if (movement == 1)
        {
            locations.Add(new Vector2Int(grid.x + 1, grid.y));
            locations.Add(new Vector2Int(grid.x -1, grid.y));
            locations.Add(new Vector2Int(grid.x, grid.y+1));
            locations.Add(new Vector2Int(grid.x, grid.y-1));
        }

        else
        {
            if (movement == 3)
            {
                locations.Add(new Vector2Int(grid.x + 3, grid.y));
                locations.Add(new Vector2Int(grid.x + 2, grid.y));
                locations.Add(new Vector2Int(grid.x + 2, grid.y + 1));
                locations.Add(new Vector2Int(grid.x + 2, grid.y - 1));
                locations.Add(new Vector2Int(grid.x + 1, grid.y));
                locations.Add(new Vector2Int(grid.x + 1, grid.y+1));
                locations.Add(new Vector2Int(grid.x + 1, grid.y + 2));
                locations.Add(new Vector2Int(grid.x + 1, grid.y-1));
                locations.Add(new Vector2Int(grid.x + 1, grid.y -2));
                locations.Add(new Vector2Int(grid.x, grid.y + 3));
                locations.Add(new Vector2Int(grid.x, grid.y + 2));
                locations.Add(new Vector2Int(grid.x, grid.y + 1));
                locations.Add(new Vector2Int(grid.x, grid.y - 1));
                locations.Add(new Vector2Int(grid.x, grid.y - 2));
                locations.Add(new Vector2Int(grid.x, grid.y - 3));
                locations.Add(new Vector2Int(grid.x - 1, grid.y));
                locations.Add(new Vector2Int(grid.x - 1, grid.y +1 ));
                locations.Add(new Vector2Int(grid.x - 1, grid.y + 2));
                locations.Add(new Vector2Int(grid.x - 1, grid.y -1 ));
                locations.Add(new Vector2Int(grid.x - 1, grid.y - 2));
                locations.Add(new Vector2Int(grid.x - 2, grid.y));
                locations.Add(new Vector2Int(grid.x - 2, grid.y + 1));
                locations.Add(new Vector2Int(grid.x - 2, grid.y - 1));
                locations.Add(new Vector2Int(grid.x-3, grid.y ));
            }
            else
            {
                locations.Add(new Vector2Int(grid.x + 2, grid.y));
                locations.Add(new Vector2Int(grid.x + 1, grid.y));
                locations.Add(new Vector2Int(grid.x + 1, grid.y + 1));
                locations.Add(new Vector2Int(grid.x + 1, grid.y - 1));
                locations.Add(new Vector2Int(grid.x, grid.y + 2));
                locations.Add(new Vector2Int(grid.x, grid.y + 1));
                locations.Add(new Vector2Int(grid.x, grid.y - 1));
                locations.Add(new Vector2Int(grid.x, grid.y - 2));
                locations.Add(new Vector2Int(grid.x - 1, grid.y));
                locations.Add(new Vector2Int(grid.x - 1, grid.y + 1));
                locations.Add(new Vector2Int(grid.x - 1, grid.y - 1));
                locations.Add(new Vector2Int(grid.x - 2, grid.y));
            }
        }

        
        return locations;

    }

    public List<Vector2Int> AttackLocations(Vector2Int grid)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        if (attackType == 0)
        {
            locations.Add(new Vector2Int(grid.x + 1, grid.y));
            locations.Add(new Vector2Int(grid.x - 1, grid.y));
            locations.Add(new Vector2Int(grid.x, grid.y + 1));
            locations.Add(new Vector2Int(grid.x, grid.y - 1));
            cooldown += 2;
        }
        else
        {
            if (attackType == 1)
            {
                locations.Add(new Vector2Int(grid.x, grid.y + 1));
                locations.Add(new Vector2Int(grid.x, grid.y + 2));
                locations.Add(new Vector2Int(grid.x, grid.y + 3));
                locations.Add(new Vector2Int(grid.x, grid.y + 4));
                locations.Add(new Vector2Int(grid.x, grid.y + 5));
                locations.Add(new Vector2Int(grid.x, grid.y + 6));
                locations.Add(new Vector2Int(grid.x, grid.y - 1));
                locations.Add(new Vector2Int(grid.x, grid.y - 2));
                locations.Add(new Vector2Int(grid.x, grid.y - 3));
                locations.Add(new Vector2Int(grid.x, grid.y - 4));
                locations.Add(new Vector2Int(grid.x, grid.y - 5));
                locations.Add(new Vector2Int(grid.x, grid.y - 6));
                locations.Add(new Vector2Int(grid.x + 1, grid.y));
                locations.Add(new Vector2Int(grid.x + 2, grid.y));
                locations.Add(new Vector2Int(grid.x + 3, grid.y));
                locations.Add(new Vector2Int(grid.x + 4, grid.y));
                locations.Add(new Vector2Int(grid.x + 5, grid.y));
                locations.Add(new Vector2Int(grid.x + 6, grid.y));
                locations.Add(new Vector2Int(grid.x - 1, grid.y));
                locations.Add(new Vector2Int(grid.x - 2, grid.y));
                locations.Add(new Vector2Int(grid.x - 3, grid.y));
                locations.Add(new Vector2Int(grid.x - 4, grid.y));
                locations.Add(new Vector2Int(grid.x - 5, grid.y));
                locations.Add(new Vector2Int(grid.x - 6, grid.y));
                cooldown += 3;
            }
            else
            {
                if (attackType == 2)
                {
                    locations.Add(new Vector2Int(grid.x + 3, grid.y));
                    locations.Add(new Vector2Int(grid.x + 2, grid.y));
                    locations.Add(new Vector2Int(grid.x + 2, grid.y + 1));
                    locations.Add(new Vector2Int(grid.x + 2, grid.y - 1));
                    locations.Add(new Vector2Int(grid.x + 1, grid.y));
                    locations.Add(new Vector2Int(grid.x + 1, grid.y + 1));
                    locations.Add(new Vector2Int(grid.x + 1, grid.y + 2));
                    locations.Add(new Vector2Int(grid.x + 1, grid.y - 1));
                    locations.Add(new Vector2Int(grid.x + 1, grid.y - 2));
                    locations.Add(new Vector2Int(grid.x, grid.y + 3));
                    locations.Add(new Vector2Int(grid.x, grid.y + 2));
                    locations.Add(new Vector2Int(grid.x, grid.y + 1));
                    locations.Add(new Vector2Int(grid.x, grid.y - 1));
                    locations.Add(new Vector2Int(grid.x, grid.y - 2));
                    locations.Add(new Vector2Int(grid.x, grid.y - 3));
                    locations.Add(new Vector2Int(grid.x - 1, grid.y));
                    locations.Add(new Vector2Int(grid.x - 1, grid.y + 1));
                    locations.Add(new Vector2Int(grid.x - 1, grid.y + 2));
                    locations.Add(new Vector2Int(grid.x - 1, grid.y - 1));
                    locations.Add(new Vector2Int(grid.x - 1, grid.y - 2));
                    locations.Add(new Vector2Int(grid.x - 2, grid.y));
                    locations.Add(new Vector2Int(grid.x - 2, grid.y + 1));
                    locations.Add(new Vector2Int(grid.x - 2, grid.y - 1));
                    locations.Add(new Vector2Int(grid.x - 3, grid.y));
                    cooldown += 4;
                }
                else
                {
                    if (attackType == 3)
                    {
                        cooldown += 5;
                    }
                }
            }
        }

        return locations;
    }

    public List<Vector2Int> FaceDirections(Vector2Int grid)
    {
        List<Vector2Int> locations = new List<Vector2Int>
        {
            new Vector2Int(grid.x + 1, grid.y),
            new Vector2Int(grid.x - 1, grid.y),
            new Vector2Int(grid.x, grid.y + 1),
            new Vector2Int(grid.x, grid.y - 1)
        };
        return locations;
    }


}
