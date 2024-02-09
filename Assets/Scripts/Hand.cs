using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField] Player player;
    Collider2D hitBox;

    [SerializeField] float xOffset, yOffset;
    float handDirectionOffset { get; set; }
    

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        hitBox = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.parent.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle + handDirectionOffset);
        //Debug.DrawLine(transform.position, mousePosition);
        //Debug.DrawLine(transform.position, transform.localPosition.);

        if (hitBox.enabled)
        {
            if (Vector2.Distance(transform.position, transform.parent.TransformPoint(new Vector2(xOffset, yOffset))) < 0.3f)
            {
                if(handDirectionOffset == -90f)
                    transform.localPosition += transform.up * Time.deltaTime * 8f;
                else
                    transform.localPosition += transform.right * Time.deltaTime * 8f;
            }
            else
                hitBox.enabled = false;
        }
        else
            transform.position = Vector2.Lerp(transform.position, transform.parent.TransformPoint(new Vector2(xOffset, yOffset)), Time.deltaTime * 5f);
    }

    public void UpdateHandDirectionOffset(float offset)
    {
        handDirectionOffset = offset;
    }

    public void Hit()
    {
        hitBox.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Entity>())
        {
            player.PlayerInteractWith(collision.GetComponent<Entity>());
        }    
        
    }
}
