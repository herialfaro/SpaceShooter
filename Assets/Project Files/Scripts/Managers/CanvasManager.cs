using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private RectTransform _GameScreen, _PauseScreen, _GameOverScreen;

    [SerializeField] private TMP_Text _Score, _Lives;
    #endregion

    #region Monobehaviour Methods
    // Start is called before the first frame update
    private void OnEnable()
    {
        SpaceShooterManager.Instance.onGameStart += ShowGameScreen;
        SpaceShooterManager.Instance.onGameOver += ShowGameOverScreen;
        SpaceShooterManager.Instance.onPlayerPaused += ShowPauseScreen;
    }

    private void OnDisable()
    {
        SpaceShooterManager.Instance.onGameStart -= ShowGameScreen;
        SpaceShooterManager.Instance.onGameOver -= ShowGameOverScreen;
        SpaceShooterManager.Instance.onPlayerPaused -= ShowPauseScreen;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _Score.text = SpaceShooterManager.Instance.i_CurrentScore.ToString();
        _Lives.text = SpaceShooterManager.Instance.i_RemainingLives.ToString();
    }
    #endregion

    #region Public Methods
    public void HideAllScreens()
    {
        _GameScreen?.gameObject.SetActive(false);
        _PauseScreen?.gameObject.SetActive(false);
        _GameOverScreen?.gameObject.SetActive(false);
    }
    public void ShowGameScreen()
    {
        HideAllScreens();
        _GameScreen?.gameObject.SetActive(true);
    }
    public void ShowPauseScreen()
    {
        HideAllScreens();
        _PauseScreen?.gameObject.SetActive(true);
    }
    public void ShowGameOverScreen()
    {
        HideAllScreens();
        _GameOverScreen?.gameObject.SetActive(true);
    }
    #endregion
}
