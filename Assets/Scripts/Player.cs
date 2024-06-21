using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    private float horizontal; // horizontal input (left and right arrow keys)
    private float vertical; // vertical input (up and down arrow keys)
    public float speed = 6f; // speed of the player, can change from the editor
    private PhotonView view; // The player's network view
    private joinChannelVideo jcv; // agora settings

    [SerializeField] private Rigidbody2D rb; // player rigidbody

    private void OnEnable()
    {
        jcv = GameObject.Find("Canvas").GetComponent<joinChannelVideo>();
    }
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    // Update is called once per frame
    void Update()
    {
        // update the input only of the active screen
        if (view.IsMine)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
    }
    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            // change the velocity based on the input
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if collided with another player, join the video chat
        if (collision.CompareTag("Player"))
        {
            jcv.Join();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // leave the video chat on exiting the other player
        jcv.Leave();
    }
}
