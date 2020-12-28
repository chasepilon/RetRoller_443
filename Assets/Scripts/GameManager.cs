using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ClickableObject[] clickables;
    public int cassetteDurability = 2;
    public int tapeLength = 10;
    public int maxRewindCharge = 100;
    public int maxObjectsOnScreen = 15;
    public float waitTime = 3.0f;
    public float minXForce = 0.0f;
    public float maxXForce = 0.0f;
    public float minYForce = 0.0f;
    public float maxYForce = 0.0f;
    public float rightStart = 13.0f;
    public float leftStart = -13.0f;
    public float topStart = 9.0f;
    public float bottomStart = -6.5f;
    public float maxDistance = 10.0f;
    public string nextLevel = "";
    public string levelGoalText = "";
    public GameObject canvasHintText;
    public GameObject rewindIcon;
    public GameObject progressBar;
    public GameObject[] cassetteHealthBar;
    public GameObject audioCassetteObject;
    public AudioSource musicSource;
    public AudioSource abilityReadyAudio;
    public AudioSource abilityUsed;
    public AudioSource[] soundFX;
    public GameObject[] burstFX;
    public Sprite enabledSprite;
    public Sprite disabledSprite;
    public Animator audioCassette;
    

    Queue<ClickableObject> objectsOnScreen = new Queue<ClickableObject>();
    int currentRewindCharge;
    int gravityToggle;
    int randomStartingSide;
    float randomXPos;
    float randomYPos;
    float xForce;
    float yForce;
    float distance;
    float startDelay;
    float animationDuration;
    float animCounter;
    bool rewindAbilityReady;
    bool gameOver;
    bool levelComplete;
    bool isPaused;
    bool backToMain;
    bool blinkToggle;
    bool playCassetteAnimation;
    Slider slider;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        gameOver = false;
        levelComplete = false;
        isPaused = false;
        rewindAbilityReady = false;
        backToMain = false;
        blinkToggle = true;
        startDelay = waitTime;
        animationDuration = 2.0f;
        animCounter = animationDuration;

        currentRewindCharge = 0;
        ClickableObjectPool.Instance.SetPool(clickables, maxObjectsOnScreen);
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        canvasHintText.GetComponent<Text>().text = levelGoalText;

        rewindIcon.SetActive(false);
        playCassetteAnimation = false;
        audioCassette.SetBool("cassetteIsPlaying", playCassetteAnimation);
        slider = progressBar.GetComponent<Slider>();
        slider.maxValue = tapeLength;
        slider.value = tapeLength;
    }

    void Update()
    {
        if(startDelay < 1.0f && startDelay > 0.0f && blinkToggle)
            ToggleStartingUI();

        if (startDelay < 0.0f)
        {
            if (maxRewindCharge > 0)
            {
                rewindIcon.SetActive(true);

                ToggleIcon(rewindAbilityReady);
            }

            if (gameOver)
            {
                StartCoroutine(StartFade(musicSource, 0.5f, 0));
                SceneManager.LoadScene("GameOver");
            }

            if (backToMain)
            {
                StartCoroutine(StartFade(musicSource, 0.5f, 0));
                SceneManager.LoadScene("Title");
            }

            if(playCassetteAnimation)
            {
                if(animCounter <= 0.0f)
                {
                    playCassetteAnimation = false;
                    animCounter = animationDuration;
                    audioCassette.SetBool("cassetteIsPlaying", playCassetteAnimation);
                }
                else
                {
                    animCounter = animCounter - Time.deltaTime;
                }
            }
            else if (levelComplete)
            {
                PlayerPrefs.SetString("NextScene", nextLevel);
                PlayerPrefs.SetInt("CassettesUnlocked", PlayerPrefs.GetInt("CassettesUnlocked") + 1);
                StartCoroutine(StartFade(musicSource, 0.5f, 0));
                SceneManager.LoadScene("Complete");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                TogglePause();

            if (Input.GetMouseButtonDown(1))
            {
                if (isRewindAbilityReady())
                {
                    abilityUsed.Play();
                }
            }

            var item = ClickableObjectPool.Instance.Get();

            if (item != null)
            {
                randomStartingSide = Random.Range(0, 4);
                distance = Random.Range(0, maxDistance);

                switch (randomStartingSide)
                {
                    case 0:     //Bottom launch
                        randomXPos = Random.Range(leftStart, rightStart);
                        item.SetStartPosition(randomXPos, bottomStart, distance);
                        if (randomXPos <= 0)
                        {
                            xForce = Random.Range(minXForce, maxXForce);
                            yForce = Random.Range(minYForce, maxYForce);
                        }
                        else
                        {
                            xForce = Random.Range(-maxXForce, -minXForce);
                            yForce = Random.Range(minYForce, maxYForce);
                        }
                        break;
                    case 1:     //Top launch
                        randomXPos = Random.Range(leftStart, rightStart);
                        item.SetStartPosition(randomXPos, topStart, distance);
                        if (randomXPos <= 0)
                        {
                            xForce = Random.Range(minXForce, maxXForce);
                            yForce = Random.Range(-maxYForce, -minYForce);
                        }
                        else
                        {
                            xForce = Random.Range(-maxXForce, minXForce);
                            yForce = Random.Range(-maxYForce, minYForce);
                        }
                        break;
                    case 2:     //Left launch
                        randomYPos = Random.Range(bottomStart, topStart);
                        item.SetStartPosition(leftStart, randomYPos, distance);
                        if (randomYPos <= 0)
                        {
                            xForce = Random.Range(minXForce, maxXForce);
                            yForce = Random.Range(minYForce, maxYForce);
                        }
                        else
                        {
                            xForce = Random.Range(minXForce, maxXForce);
                            yForce = Random.Range(-maxYForce, -minYForce);
                        }
                        break;
                    case 3:     //Right launch
                        randomYPos = Random.Range(bottomStart, topStart);
                        item.SetStartPosition(rightStart, randomYPos, distance);
                        if (randomYPos <= 0)
                        {
                            xForce = Random.Range(-maxXForce, -minXForce);
                            yForce = Random.Range(minYForce, maxYForce);
                        }
                        else
                        {
                            xForce = Random.Range(-maxXForce, -minXForce);
                            yForce = Random.Range(-maxYForce, -minYForce);
                        }
                        break;
                };

                //Enable and launch
                item.SetActive(true);
                item.Launch(xForce, yForce, 0, ForceMode.Impulse);
            }
        }
        else
            startDelay = startDelay - Time.deltaTime;
    }

    void ToggleGravity(ClickableObject item)
    {
        gravityToggle = Random.Range(0, 2);
        if (gravityToggle == 1)
            item.enableGravity();
        else
            item.disableGravity();
    }

    public void RewindTape(int amount)
    {
        tapeLength -= amount;
        slider.value = tapeLength;
        playCassetteAnimation = true;
        audioCassette.SetBool("cassetteIsPlaying", playCassetteAnimation);
        if (tapeLength <= 0)
        {
            tapeLength = 0;
            levelComplete = true;
        }
    }

    public void DamageCassette(int amount)
    {
        cassetteDurability -= amount;

        //Update Cassette Health Bar
        for (int i = 0; i < cassetteHealthBar.Length; i++)
        {
            if (i < cassetteDurability)
                cassetteHealthBar[i].SetActive(true);
            else
                cassetteHealthBar[i].SetActive(false);
        }

        if (cassetteDurability <= 0)
        {
            cassetteDurability = 0;
            gameOver = true;
        }
    }

    public void ChargeRewindAbility(int amount)
    {
        currentRewindCharge += amount;
        if (currentRewindCharge >= maxRewindCharge)
        {
            currentRewindCharge = maxRewindCharge;
            rewindAbilityReady = true;
            abilityReadyAudio.Play();
        }
    }

    public bool isRewindAbilityReady()
    {
        return rewindAbilityReady;
    }

    public void ResetRewindAbility()
    {
        currentRewindCharge = 0;
        rewindAbilityReady = false;
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            isPaused = true;
            progressBar.SetActive(false);
            foreach (var bar in cassetteHealthBar)
                bar.SetActive(false);
            audioCassetteObject.SetActive(false);
            Time.timeScale = 0;
            musicSource.Pause();
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
        else
        {
            isPaused = false;
            progressBar.SetActive(true);
            foreach (var bar in cassetteHealthBar)
                bar.SetActive(true);
            audioCassetteObject.SetActive(true);
            Time.timeScale = 1;
            musicSource.Play();
            SceneManager.UnloadSceneAsync("PauseMenu");
        }

    }

    public void ToggleStartingUI()
    {
        if (blinkToggle)
        {
            canvasHintText.SetActive(false);
            progressBar.SetActive(true);
            foreach (var bar in cassetteHealthBar)
                bar.SetActive(true);
        }

        blinkToggle = !blinkToggle;
    }

    public void ToggleIcon(bool setting)
    {
        if (setting)
        {
            rewindIcon.GetComponent<Image>().sprite = enabledSprite;
        }
        else
        {
            rewindIcon.GetComponent<Image>().sprite = disabledSprite;
        }
    }

    public void BackToMain()
    {
        backToMain = true;
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    //public void CreateBurstEffects(Vector3 position, int effectType, int soundFXType)
    //{
    //    soundFX[soundFXType].transform.position = position;
    //    burstFX[effectType].transform.position = position;
    //    burstFX[effectType].transform.localScale = new Vector3(220, 220, 220);

    //    soundFX[soundFXType].Play();
    //    burstFX[effectType].GetComponent<ParticleSystem>().Play();
    //}
}
