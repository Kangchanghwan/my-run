using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    public Button pauseButton;
    public Button restartButton;
    [FormerlySerializedAs("soundButton")] public Button soundOffButton;
    [FormerlySerializedAs("soundButton")] public Button soundOnButton;
    public Button startButton;
    public GameObject pausePanel;  // 일시정지 패널 (선택사항)
    
    private bool isPaused = false;
    private bool isSoundOff = false;
    
    private const string SOUND_PREF_KEY = "GameSoundEnabled";

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadSoundSettings();
        // 방법 1: 코드로 버튼 이벤트 연결
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnGamePause);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnGameRestart);
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnGameResume);
        }

        if (soundOffButton != null)
        {
            soundOffButton.onClick.AddListener(OnGameSoundOff);
        }

        if (soundOnButton != null)
        {
            soundOnButton.onClick.AddListener(OnGameSoundOn);
        }
        
        // 초기 상태에서는 일시정지 패널 비활성화
        if (pausePanel != null)
        {
            Menu(false);
        }
    }

    public void OnGameSoundOn()
    {
        isSoundOff = false;
        AudioListener.volume = 1;
        soundOnButton.gameObject.SetActive(false);
        soundOffButton.gameObject.SetActive(true);
        SaveSoundSettings();
    }

    public void OnGameSoundOff()
    {
        isSoundOff = true;
        AudioListener.volume = 0;
        soundOffButton.gameObject.SetActive(false);
        soundOnButton.gameObject.SetActive(true);
        SaveSoundSettings();
    }

    // Update is called once per frame
    void Update()
    {
        // ESC 키로도 일시정지 가능하게 (선택사항)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnGamePause();
        }
    }

    public void OnGamePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            // 게임 일시정지
            Time.timeScale = 0f;
            Debug.Log("게임이 일시정지되었습니다.");
            
            // 일시정지 패널 활성화
            if (pausePanel != null)
            {
                Menu(true);
            }
        }
        else
        {
            // 게임 재개
            Time.timeScale = 1f;
            Debug.Log("게임이 재개되었습니다.");
            
            // 일시정지 패널 비활성화
            if (pausePanel != null)
            {
                Menu(false);
            }
        }
    }

    private void Menu(bool visible)
    {
        pausePanel.SetActive(visible);
        restartButton.gameObject.SetActive(visible);
        startButton.gameObject.SetActive(visible);
        if (isSoundOff)
        {
            soundOnButton.gameObject.SetActive(visible);
        }
        else
        {
            soundOffButton.gameObject.SetActive(visible);
        }
    }

    public void OnGameRestart()
    {
        Time.timeScale = 1f;
        Menu(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);      
    }
    
    // 게임 재개 전용 함수 (Resume 버튼용)
    public void OnGameResume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        
        if (pausePanel != null)
        {
            Menu(false);

        }
        
        Debug.Log("게임이 재개되었습니다.");
    }
    
    // 메인 메뉴로 이동 (선택사항)
    public void OnMainMenu()
    {
        Time.timeScale = 1f;  // 시간 스케일 정상화
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void SaveSoundSettings()
    {
        PlayerPrefs.SetInt(SOUND_PREF_KEY, isSoundOff ? 0 : 1);
        PlayerPrefs.Save();
    }
    // 사운드 설정 불러오기
    private void LoadSoundSettings()
    {
        isSoundOff = PlayerPrefs.GetInt(SOUND_PREF_KEY, 1) == 0; // 기본값: 켜짐
    }
    

}