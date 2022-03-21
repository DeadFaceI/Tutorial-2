using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;
    public Text score;
    public Text lives;
    public TextMeshProUGUI winText;
    public GameObject winTextObject;
    private int scoreValue = 0;
    private int livesValue = 3;
    private int levelSwitch = 0;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float  checkRadius;
    public LayerMask allGround;
    public Text hozText;
    public Text jumpText;
    // Start is called before the first frame update
    void Start()
    {
        winTextObject.SetActive(false);
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (scoreValue == 4 && levelSwitch == 0)
        {
            transform.position = new Vector3(60.0f, 0f, 0);
            levelSwitch += 1;
            if(scoreValue == 4 && levelSwitch == 1)
            {
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
            }
        }
        if (scoreValue >= 8)
        {
            winTextObject.SetActive(true);
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
            
        }
        else if ( livesValue == 0)
        {
            Destroy(this);
            winText.text = "You Lose";
            winTextObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (hozMovement > 0 && facingRight == true)
        {
            Debug.Log ("Facing Right");
            hozText.text = "Facing Right";
        }
        if (hozMovement < 0 && facingRight == false)
        {
            Debug.Log ("Facing Left");
            hozText.text = "Facing Left";
        }

        if (verMovement > 0 && isOnGround == false)
        {
            Debug.Log  ("Jumping");
            jumpText.text = "Jumping";
        }
        else if (verMovement == 0 && isOnGround == true)
        {
            Debug.Log ("Not Jumping");
            jumpText.text = "Not Jumping";
        }
    }

    void Flip()
   {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
   }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
            }
        }
    }
}
