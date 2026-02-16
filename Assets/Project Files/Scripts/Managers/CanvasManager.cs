using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private RectTransform _PauseScreen, _GameOverScreen;
    #endregion

    #region Monobehaviour Methods
    // Start is called before the first frame update
    private void OnEnable()
    {
        SpaceShooterManager.Instance.onPlayerWasDefeated += ShowGameOverScreen;
        SpaceShooterManager.Instance.onPlayerPaused += ShowPauseScreen;
    }

    private void OnDisable()
    {
        SpaceShooterManager.Instance.onPlayerWasDefeated -= ShowGameOverScreen;
        SpaceShooterManager.Instance.onPlayerPaused -= ShowPauseScreen;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    public void HideAllScreens()
    {
        _PauseScreen?.gameObject.SetActive(false);
        _GameOverScreen?.gameObject.SetActive(false);
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
