using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using maxstAR;
using UnityEngine.UI;

public class InchkievGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startUIMenu;
    [SerializeField]
    private GameObject arGameUIMenu;
    [SerializeField]
    private GameObject playPause_btn;
    [SerializeField]
    private GameObject soundOnOff_btn;
    [SerializeField]
    private GameObject startGame_btn;
    [SerializeField]
    private GameObject restartGame_btn;
    [SerializeField]
    private GameObject endGame_btn;
    [SerializeField]
    private GameObject score_btn;
    [SerializeField]
    private GameObject timer_btn;
    [SerializeField]
    private GameObject allSounds;
    [SerializeField]
    private GameObject stars;
    [SerializeField]
    private Sprite play_img_btn;
    [SerializeField]
    private Sprite pause_img_btn;
    [SerializeField]
    private Sprite soundOff_img_btn;
    [SerializeField]
    private Sprite soundOn_img_btn;
    [SerializeField]
    private AudioSource background_sound;
    [SerializeField]
    private AudioSource game_end_sound;
    [SerializeField]
    private AudioSource hit_star_sound;
    [SerializeField]
    private AudioSource tap_btn_sound;

    [Space]
    [SerializeField]
    bool playPauseBtnChanger;
    [SerializeField]
    bool soundOnOffBtnChanger = true;
    [SerializeField]
    bool playGame = true;
    [SerializeField]
    bool restartGame;
    [SerializeField]
    bool workCounterGoBack = true;

    [Space]
    [SerializeField]
    private Text counter;
    [SerializeField]
    private Text end_score;
    [SerializeField]
    private Text scoreText;

    [Space]
    [SerializeField]
    private List<GameObject> allArGameUIMenu;
    [Space]

    [SerializeField]
    private int starCounter;

    [SerializeField]
    private List <StarsMoves> starsMoves;

    float myTime = 30;

    private void OnEnable()
    {
        ImageTrackableBehaviour.OnTrackSuccessFailEvent += ImageTrackableBehaviour_OnTrackSuccessFailEvent;
        StarsMoves.OnClickToStar += StarsMoves_OnClickToStar;
    }

    private void OnDisable()
    {
        ImageTrackableBehaviour.OnTrackSuccessFailEvent -= ImageTrackableBehaviour_OnTrackSuccessFailEvent;
        StarsMoves.OnClickToStar -= StarsMoves_OnClickToStar;
    }

    private void StarsMoves_OnClickToStar(GameObject obj)
    {
        starCounter++;
        scoreText.text = starCounter.ToString();
        StartCoroutine(WaitForStartVisibleStar(obj));
    }

    private void ImageTrackableBehaviour_OnTrackSuccessFailEvent(bool obj)
    {
        if (obj)
        {
            startUIMenu.SetActive(false);
            arGameUIMenu.SetActive(true);

            if (soundOnOffBtnChanger && !background_sound.isPlaying)
            {
                background_sound.Play();
            }
            PlayPauseBtnChanger();
        }
        else
        {
            arGameUIMenu.SetActive(false);
            startUIMenu.SetActive(true);
            if (playPauseBtnChanger == false)
            {
                PlayPauseBtnChanger();
            }  
        }
    }

    public void PlayPauseBtnChanger()
    {
        if(!startGame_btn.activeSelf)
        {
            playPauseBtnChanger = !playPauseBtnChanger;
            if (playPauseBtnChanger)
            {
                playPause_btn.GetComponent<Image>().sprite = play_img_btn;
                for(int i = 0; i < starsMoves.Count; i++)
                {
                    starsMoves[i].setPause = true;
                    starsMoves[i].move = false;
                }
            }
            else
            {
                playPause_btn.GetComponent<Image>().sprite = pause_img_btn;
                for (int i = 0; i < starsMoves.Count; i++)
                {
                    starsMoves[i].setPause = false;
                    starsMoves[i].move = true;
                }
            }
            if (soundOnOffBtnChanger && !tap_btn_sound.isPlaying)
            {
                tap_btn_sound.Play();
            }
            playGame = !playGame; //допомагає ставити гру на паузу при натисканні кнопки
        }

    }
    public void SoundOnOffBtnChanger()
    {
        soundOnOffBtnChanger = !soundOnOffBtnChanger;
        if (soundOnOffBtnChanger)
        {
            soundOnOff_btn.GetComponent<Image>().sprite = soundOn_img_btn;
            allSounds.SetActive(true);
            background_sound.Play();
        }
        else
        {
            soundOnOff_btn.GetComponent<Image>().sprite = soundOff_img_btn;
            allSounds.SetActive(false);
        }
    }

    public void StartGame()
    {
        playPause_btn.GetComponent<Image>().sprite = pause_img_btn;
        for(int i = 0; i < allArGameUIMenu.Count; i++)
        {
            if(allArGameUIMenu[i].Equals(startGame_btn) || allArGameUIMenu[i].Equals(endGame_btn))
            {
                allArGameUIMenu[i].SetActive(false);
            }
            else
            {
                allArGameUIMenu[i].SetActive(true);
            }
        }
        stars.SetActive(true);
        StartCoroutine(CounterGoBack());
    }

    public void RestartGame()
    {
        playPause_btn.GetComponent<Image>().sprite = pause_img_btn;
        for (int i = 0; i < allArGameUIMenu.Count; i++)
        {
            if (allArGameUIMenu[i].Equals(startGame_btn) || allArGameUIMenu[i].Equals(endGame_btn))
            {
                allArGameUIMenu[i].SetActive(false);
            }
            else
            {
                allArGameUIMenu[i].SetActive(true);
            }
        }
        restartGame = true;
        starCounter = 0;
        scoreText.text = starCounter.ToString();
        playPause_btn.GetComponent<Image>().sprite = pause_img_btn;
        stars.SetActive(true);
        for (int i = 0; i < starsMoves.Count; i++)
        {
            starsMoves[i].setPause = false;
            starsMoves[i].move = true;
        }
        if (workCounterGoBack == false)
        {
            StartCoroutine(CounterGoBack());
            workCounterGoBack = true;
        }
        else
        {
            myTime = 30;
            playGame = true;
        }
    }

    IEnumerator CounterGoBack()
    {

        myTime = 30;
        while (myTime > 0)
        {
            if (restartGame)
            {
                myTime = 30;
                restartGame = false;
                counter.text = "0:" + myTime.ToString();
            }

            yield return new WaitForSeconds(1);
            while (playGame == false)
            {
                yield return null;
            }
            myTime = myTime - 1;
            if(myTime > 9)
            {
                counter.text = "0:" + myTime.ToString();
            }
            else
            {
                counter.text = "0:0" + myTime.ToString();
            }
        }
        endGame_btn.SetActive(true);
        end_score.text = starCounter.ToString();
        game_end_sound.Play();
        stars.SetActive(false);
        workCounterGoBack = false;
        playPause_btn.SetActive(false);
        score_btn.SetActive(false);
        timer_btn.SetActive(false);
    }

    IEnumerator WaitForStartVisibleStar(GameObject star)
    {
        star.SetActive(false);
        star.transform.position = star.GetComponent<StarsMoves>().starsTargetPoints[Random.Range(0, star.GetComponent<StarsMoves>().starsTargetPoints.Count)].transform.position;
        yield return new WaitForSeconds(2);
        star.SetActive(true);
    }
}
