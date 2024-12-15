using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Item_End: MonoBehaviour
{
    public GameObject video;

    private VideoPlayer _videoPlayer;
    private int readyCharacter = 0;

    [SerializeField] private ScaleManager player1;
    [SerializeField] private ScaleManager player2;
    [SerializeField] private GameObject line;

    [SerializeField] private int passLevel1 = 3;
    [SerializeField] private int passLevel2 = 3;

    private Animator anim;

    private void Awake()
    {
        _videoPlayer = video.GetComponent<VideoPlayer>();
        video.SetActive(false);
        _videoPlayer.loopPointReached += OnVideoFinished;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            readyCharacter++;
            print("readyCharacter = " + readyCharacter);
        }

        if(readyCharacter == 2 && (player1.level != passLevel1 && player1.level != passLevel2) || (player2.level != passLevel1 && player2.level != passLevel2))
        {
            anim.SetTrigger("Warning");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(readyCharacter == 2 && ((player1.level == passLevel1 && player2.level == passLevel2) || (player1.level == passLevel2 && player2.level == passLevel1)))
        {
            print("Next level");
            anim.SetTrigger("Entry");
            Destroy(player1.gameObject);
            Destroy(player2.gameObject);
            Destroy(line);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            readyCharacter--;
            print("readyCharacter = " + readyCharacter);
        }
    }

    public void LoadNextLevel() 
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        }
        else
        {
            Time.timeScale = 0;
            PlayVideo();
        }
    }
    
    public void PlayVideo()
    {
        video.SetActive(true);
        _videoPlayer.Play();
    }
    public void OnVideoFinished(VideoPlayer player)
    {
        video.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
