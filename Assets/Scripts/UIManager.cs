using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public GameObject video;

    private VideoPlayer _videoPlayer;

    public GameObject aboutUs;
    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = video.GetComponent<VideoPlayer>();
        video.SetActive(false);
        _videoPlayer.loopPointReached += OnVideoFinished;
        aboutUs.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JpToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void JpToLevel1()
    {
        SceneManager.LoadScene("level1");
    }
    
    public void JpToLevel2()
    {
        SceneManager.LoadScene("level2");
    }
    
    public void JpToLevel3()
    {
        SceneManager.LoadScene("level3");
    }

    public void PlayVideo()
    {
        video.SetActive(true);
        _videoPlayer.Play();
    }

    public void OnVideoFinished(VideoPlayer player)
    {
        video.SetActive(false);
    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1;
    }

    public void AboutUsIn()
    {
        aboutUs.SetActive(true);
    }
    public void AboutUsBack()
    {
        aboutUs.SetActive(false);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
