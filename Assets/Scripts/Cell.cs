using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Cell : MonoBehaviour
{
    public bool alive;
    public bool next_state_alive;

    SpriteRenderer spriteRenderer;

    public Sprite alive_cell;
    public Sprite newborn_cell;
    public Sprite dead_cell;
    public Sprite small_dead_cell;

    private void Start()
    {
        next_state_alive = alive;
    }


    public void CreateCells()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = alive;
    }

    public void EnableCell()
    {
        spriteRenderer.enabled = alive;
    }

    public void UpdateStatus()
    {
        if (spriteRenderer.sprite == alive_cell)
        {
            GameOfLife.Instance.big_cells++;
        }

        if (alive && next_state_alive)
        {
            spriteRenderer.enabled = alive;
            ChangeSprite(1); 
        }

        else if (!alive && next_state_alive)
        {
            ChangeSprite(3);
            spriteRenderer.enabled = next_state_alive;
        }

        else if (!next_state_alive && spriteRenderer.sprite == newborn_cell)
        {
            ChangeSprite(4);
        }

        else if (!alive && !next_state_alive && spriteRenderer.sprite == dead_cell)
        {
            ChangeSprite(4);
        }

        else if (alive && !next_state_alive)
        {
            ChangeSprite(2);
        }

        else if (!alive && !next_state_alive)
        {
            spriteRenderer.enabled = alive;
        }

        alive = next_state_alive;
    }

    public void ChangeSprite(int i)
    {
        if (i == 1)
        {
            spriteRenderer.sprite = alive_cell;
            
        }
        else if (i == 2)
        {
            spriteRenderer.sprite = dead_cell;
        }
        else if (i == 3)
        {
            spriteRenderer.sprite = newborn_cell;
        }
        else if (i == 4)
        {
            spriteRenderer.sprite = small_dead_cell;
        }
        else return;
    }
}