using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public enum ControlScheme { PC, Mobile }
    public ControlScheme currentScheme;

    private GameObject mobileControlsUI;
    private Joystick movementJoystick;

    public float VerticalAxis { get; private set; }
    public float HorizontalAxis { get; private set; }
    public float MouseX { get; private set; }
    public bool IsShooting { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Se inscreve para "ouvir" o evento de carregamento de cena.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Carrega a prefer�ncia, mas n�o aplica o cursor ainda.
        currentScheme = (ControlScheme)PlayerPrefs.GetInt("ControlScheme", 0);
    }

    // Fun��o chamada toda vez que uma nova cena termina de carregar.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Procura os componentes da UI na nova cena, se necess�rio.
        if (currentScheme == ControlScheme.Mobile)
        {
            mobileControlsUI = GameObject.Find("MobileControlsContainer");
            if (mobileControlsUI != null)
            {
                movementJoystick = mobileControlsUI.GetComponentInChildren<Joystick>();
            }
        }

        // Aplica o esquema de controle, passando o NOME da cena atual.
        ApplyScheme(scene.name);
    }

    // --- FUN��O MODIFICADA PARA CONSIDERAR A CENA ---
    private void ApplyScheme(string sceneName)
    {
        bool isMobile = currentScheme == ControlScheme.Mobile;

        if (mobileControlsUI != null)
        {
            mobileControlsUI.SetActive(isMobile);
        }

        // --- L�GICA DO CURSOR CORRIGIDA ---
        // VERIFIQUE SE O NOME DA SUA CENA DE MENU � "MenuScene". Se for diferente, mude aqui.
        if (sceneName == "MainMenuScene")
        {
            // Se estivermos na cena do menu, o cursor SEMPRE estar� livre e vis�vel.
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else // Para qualquer outra cena (a do jogo)
        {
            // A l�gica de travar/esconder o cursor s� se aplica se o modo N�O for mobile.
            Cursor.visible = isMobile;
            Cursor.lockState = isMobile ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    // A fun��o SetControlScheme agora est� mais simples, pois ApplyScheme ser� chamada ao carregar a cena.
    public void SetControlScheme(int schemeIndex)
    {
        currentScheme = (ControlScheme)schemeIndex;
        PlayerPrefs.SetInt("ControlScheme", schemeIndex);
        Debug.Log("Esquema de controle definido para: " + (ControlScheme)schemeIndex);
    }

    // O resto do seu c�digo (Update, ReadInputs, etc.) permanece exatamente o mesmo.
    #region FuncoesRestantes
    void Update()
    {
        switch (currentScheme)
        {
            case ControlScheme.PC: ReadPCInput(); break;
            case ControlScheme.Mobile: ReadMobileInput(); break;
        }
    }

    private void ReadPCInput()
    {
        VerticalAxis = Input.GetAxis("Vertical");
        HorizontalAxis = Input.GetAxis("Horizontal");
        MouseX = Input.GetAxis("Mouse X");
        IsShooting = Input.GetKey(KeyCode.Space);
    }

    private void ReadMobileInput()
    {
        if (movementJoystick != null)
        {
            VerticalAxis = movementJoystick.Vertical;
            HorizontalAxis = movementJoystick.Horizontal;
        }
        else { VerticalAxis = 0; HorizontalAxis = 0; }
        MouseX = 0;
    }

    public void SetShooting(bool isShooting)
    {
        if (currentScheme == ControlScheme.Mobile) IsShooting = isShooting;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion
}