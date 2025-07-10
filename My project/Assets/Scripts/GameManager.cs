using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ENUMS e CLASSES
    public enum FoodType { Coxinha, Brigadeiro, Cuscuz, Pastel }
    public enum FoodStation { Up, Down, Left, Right }

    [System.Serializable]
    public class FoodStationData
    {
        public FoodStation stationDirection;
        public FoodType currentFood;
        public Transform stationTransform;
        public SpriteRenderer stationSpriteRenderer;
    }

    // --- Vari�veis do Inspector ---
    [Header("Configura��es das Esta��es de Comida")]
    [SerializeField] private List<FoodStationData> foodStations;

    // <<< ALTERA��O: Novas vari�veis para a anima��o de troca
    [SerializeField] private float shuffleAnimationDuration = 0.7f;


    [Header("Configura��es de Dificuldade")]
    [SerializeField] private int sequenceLength = 4;
    [SerializeField] private int shufflesAfterXCorrects = 3;

    [Header("Configura��es de Tempo")]
    [SerializeField] private float initialTime = 30f;
    [SerializeField] private float timeBonusOnCorrect = 5f;
    [SerializeField] private float timePenaltyOnWrong = 10f;
    [SerializeField] private Slider timerBar;

    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private float comboAnimationSpeed = 5f;
    [SerializeField] private float comboMinScale = 0.95f;
    [SerializeField] private float comboMaxScale = 1.05f;
    [SerializeField] private Color comboBlinkColor = Color.yellow;

    [Header("Pontua��o")]
    [Tooltip("O objeto de texto que mostrar� a pontua��o.")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int basePointsForCorrect = 100;
    [SerializeField] private int bonusPointsForConsecutive = 50;

    [Header("Refer�ncias da UI")]
    [SerializeField] private Image[] requiredSequenceUI;
    [SerializeField] private Image[] playerSequenceUI;

    [Header("Sprites das Comidas")]
    [SerializeField] private Sprite coxinhaSprite;
    [SerializeField] private Sprite brigadeiroSprite;
    [SerializeField] private Sprite cuscuzSprite;
    [SerializeField] private Sprite pastelSprite;
    [SerializeField] private Sprite emptySlotSprite;

    [Header("Sprites das Barracas")]
    [SerializeField] private Sprite barracaCoxinhaSprite;
    [SerializeField] private Sprite barracaBrigadeiroSprite;
    [SerializeField] private Sprite barracaCuscuzSprite;
    [SerializeField] private Sprite barracaPastelSprite;

    [Header("Faces do Cachorro")]
    [SerializeField] private SpriteRenderer dogFaceImage;
    [SerializeField] private Sprite normalFaceSprite;
    [SerializeField] private Sprite madFaceSprite;
    [SerializeField] private Sprite furiousFaceSprite;
    [SerializeField, Range(0, 1)] private float madTimeThreshold = 0.5f;
    [SerializeField, Range(0, 1)] private float furiousTimeThreshold = 0.25f;

    [Header("Anima��es")]
    [SerializeField] private DogAnimator dogAnimator;

    [Header("Efeitos de C�mera")]
    [SerializeField] private float errorShakeDuration = 0.3f;
    [SerializeField] private float errorShakeMagnitude = 0.1f;
    [SerializeField] private float gameOverShakeDuration = 0.8f;
    [SerializeField] private float gameOverShakeMagnitude = 0.3f;

    [Header("Configura��es de �udio")] // M�SICA
    [SerializeField] private AudioSource musicSource; // O AudioSource da m�sica
    [SerializeField] private AudioSource sfxSource;   // O AudioSource dos efeitos

    [SerializeField] private AudioClip correctSound; // O som de acerto
    [SerializeField] private AudioClip wrongSound;   // O som de erro
    [SerializeField] private AudioClip shuffleSound; // O som de embaralhar (b�nus!)

    // Vari�veis Internas
    private int playerScore = 0;
    private int consecutiveCorrectAnswers = 0;
    private float currentTime;
    private List<FoodType> requiredSequence = new List<FoodType>();
    private List<FoodType> playerSequence = new List<FoodType>();
    private bool isGameOver = false;
    private bool isShuffling = false; // <<< ALTERA��O: Flag para controlar o estado de embaralhamento

    private Coroutine comboAnimationCoroutine;
    private Color originalComboTextColor;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        currentTime = initialTime;
        timerBar.value = 1;

        playerScore = 0;
        UpdateScoreUI();

        // <<< ALTERA��O: Salva a cor original do texto de combo
        if (comboText != null)
        {
            originalComboTextColor = comboText.color;
        }

        UpdateAllStationSprites();
        GenerateNewSequence();
    }

    void Update()
    {
        if (isGameOver || isShuffling) return; // <<< ALTERA��O: Pausa o timer durante o embaralhamento
        currentTime -= Time.deltaTime;
        timerBar.value = currentTime / initialTime;
        UpdateDogFace();
        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    public void SubmitSequence()
    {
        if (playerSequence.SequenceEqual(requiredSequence))
        {
            // --- L�GICA DE ACERTO ---
            sfxSource.PlayOneShot(correctSound); // <<< NOVO: Toca o som de acerto

            consecutiveCorrectAnswers++;

            int pointsToAdd = basePointsForCorrect;
            if (consecutiveCorrectAnswers > 1)
            {
                pointsToAdd += bonusPointsForConsecutive;
            }
            playerScore += pointsToAdd;
            UpdateScoreUI();

            UpdateComboDisplay();

            currentTime = Mathf.Min(currentTime + timeBonusOnCorrect, initialTime);

            if (consecutiveCorrectAnswers >= shufflesAfterXCorrects)
            {
                consecutiveCorrectAnswers = 0;
                UpdateComboDisplay();
                // <<< ALTERA��O: Em vez de chamar a fun��o diretamente, inicia a corrotina
                StartCoroutine(ShuffleAndMoveStationsCoroutine());
            }
            else
            {
                GenerateNewSequence();
            }
        }
        else
        {
            // --- L�GICA DE ERRO ---
            sfxSource.PlayOneShot(wrongSound); // <<< NOVO: Toca o som de erro

            consecutiveCorrectAnswers = 0;
            UpdateComboDisplay();


            currentTime -= timePenaltyOnWrong;
            CameraShake.Instance.StartShake(errorShakeDuration, errorShakeMagnitude);
            playerSequence.Clear();
            ClearPlayerUI();
        }
    }

    // <<< ALTERA��O: Fun��o antiga foi substitu�da por uma Corrotina
    private IEnumerator ShuffleAndMoveStationsCoroutine()
    {
        isShuffling = true;
        sfxSource.PlayOneShot(shuffleSound); // <<< NOVO: Toca o som de embaralhar
        Debug.Log("INICIANDO ANIMA��O DE EMBARALHAMENTO!");


        // 1. Guarda as posi��es atuais e as embaralha para saber os destinos
        List<Vector3> targetPositions = foodStations.Select(s => s.stationTransform.position).ToList();
        for (int i = 0; i < targetPositions.Count - 1; i++)
        {
            int rnd = Random.Range(i, targetPositions.Count);
            (targetPositions[i], targetPositions[rnd]) = (targetPositions[rnd], targetPositions[i]); // Troca moderna
        }

        // 2. Inicia a anima��o de movimento para todas as barracas ao mesmo tempo
        List<Coroutine> movementCoroutines = new List<Coroutine>();
        for (int i = 0; i < foodStations.Count; i++)
        {
            Coroutine coroutine = StartCoroutine(MoveObjectCoroutine(foodStations[i].stationTransform, targetPositions[i], shuffleAnimationDuration));
            movementCoroutines.Add(coroutine);
        }

        // 3. Espera todas as anima��es terminarem
        foreach (var coroutine in movementCoroutines)
        {
            yield return coroutine;
        }

        // 4. ATUALIZA A L�GICA INTERNA:
        //    As barracas se moveram, agora precisamos reordenar nossa lista 'foodStations'
        //    para que os dados (tipo de comida, etc.) correspondam �s novas posi��es f�sicas.
        foodStations = foodStations.OrderBy(s =>
        {
            // Ordena a lista baseado em qual posi��o de destino a barraca ocupa agora
            return targetPositions.IndexOf(s.stationTransform.position);
        }).ToList();

        Debug.Log("ANIMA��O CONCLU�DA! GERANDO NOVA SEQU�NCIA.");

        // 5. Gera a nova sequ�ncia e libera o jogo
        GenerateNewSequence();
        isShuffling = false; // Libera o jogo
    }

    // <<< ALTERA��O: Nova corrotina para animar o movimento de um objeto
    private IEnumerator MoveObjectCoroutine(Transform objectToMove, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = objectToMove.position;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Usa Lerp para mover o objeto suavemente
            objectToMove.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Espera o pr�ximo frame
        }

        // Garante que o objeto termine exatamente na posi��o final
        objectToMove.position = targetPosition;
    }


    private void UpdateComboDisplay()
    {
        if (comboAnimationCoroutine != null)
        {
            StopCoroutine(comboAnimationCoroutine);
        }

        if (consecutiveCorrectAnswers > 1)
        {
            comboText.gameObject.SetActive(true);
            comboText.text = $"COMBO x{consecutiveCorrectAnswers}";
            comboAnimationCoroutine = StartCoroutine(AnimateComboText());
        }
        else
        {
            comboText.gameObject.SetActive(false);
            comboText.transform.localScale = Vector3.one;
            comboText.color = originalComboTextColor;
        }
    }

    private IEnumerator AnimateComboText()
    {
        while (true)
        {
            float scaleSin = Mathf.Sin(Time.time * comboAnimationSpeed);
            float scaleValue = Mathf.Lerp(comboMinScale, comboMaxScale, (scaleSin + 1) / 2f);
            comboText.transform.localScale = new Vector3(scaleValue, scaleValue, 1f);

            float colorSin = Mathf.Sin(Time.time * comboAnimationSpeed * 2f);
            comboText.color = (colorSin > 0) ? comboBlinkColor : originalComboTextColor;

            yield return null;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = playerScore.ToString("D4");
        }
    }

    private void GenerateNewSequence()
    {
        requiredSequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            requiredSequence.Add((FoodType)Random.Range(0, System.Enum.GetValues(typeof(FoodType)).Length));
        }

        playerSequence.Clear();
        UpdateRequiredUI();
        ClearPlayerUI();
    }

    public void PlayerSelectedStation(FoodStation station)
    {
        if (isGameOver || isShuffling) return; // <<< ALTERA��O: Impede sele��o durante o embaralhamento

        foreach (var s in foodStations)
        {
            if (s.stationDirection == station)
            {
                AddFoodToPlayerSequence(s.currentFood);
                break;
            }
        }
    }

    #region Fun��es de UI e Efeitos Visuais (Sem altera��es aqui)

    private void UpdateAllStationSprites()
    {
        foreach (var station in foodStations)
        {
            station.stationSpriteRenderer.sprite = GetSpriteForBarraca(station.currentFood);
        }
    }

    private void UpdateRequiredUI()
    {
        for (int i = 0; i < requiredSequenceUI.Length; i++)
        {
            requiredSequenceUI[i].sprite = GetSpriteForFood(requiredSequence[i]);
        }
    }

    private void UpdatePlayerUI()
    {
        for (int i = 0; i < playerSequenceUI.Length; i++)
        {
            if (i < playerSequence.Count)
            {
                playerSequenceUI[i].sprite = GetSpriteForFood(playerSequence[i]);
            }
            else
            {
                playerSequenceUI[i].sprite = emptySlotSprite;
            }
        }
    }

    private void ClearPlayerUI()
    {
        foreach (var image in playerSequenceUI)
        {
            image.sprite = emptySlotSprite;
        }
    }

    private void UpdateDogFace()
    {
        if (dogFaceImage == null) return;

        float timePercentage = currentTime / initialTime;

        if (timePercentage <= furiousTimeThreshold)
        {
            if (dogFaceImage.sprite != furiousFaceSprite) dogFaceImage.sprite = furiousFaceSprite;
        }
        else if (timePercentage <= madTimeThreshold)
        {
            if (dogFaceImage.sprite != madFaceSprite) dogFaceImage.sprite = madFaceSprite;
        }
        else
        {
            if (dogFaceImage.sprite != normalFaceSprite) dogFaceImage.sprite = normalFaceSprite;
        }
    }

    private Sprite GetSpriteForFood(FoodType food)
    {
        switch (food)
        {
            case FoodType.Coxinha: return coxinhaSprite;
            case FoodType.Brigadeiro: return brigadeiroSprite;
            case FoodType.Cuscuz: return cuscuzSprite;
            case FoodType.Pastel: return pastelSprite;
            default: return emptySlotSprite;
        }
    }
    private Sprite GetSpriteForBarraca(FoodType food)
    {
        switch (food)
        {
            case FoodType.Coxinha: return barracaCoxinhaSprite;
            case FoodType.Brigadeiro: return barracaBrigadeiroSprite;
            case FoodType.Cuscuz: return barracaCuscuzSprite;
            case FoodType.Pastel: return barracaPastelSprite;
            default: return emptySlotSprite;
        }
    }

    private void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("FIM DE JOGO!");
              
        if (dogAnimator != null)
        {
            dogAnimator.PlayGameOverAnimation();
        }
        else
        {
            CameraShake.Instance.StartShake(gameOverShakeDuration, gameOverShakeMagnitude);
        }
    }

    #endregion

    #region Fun��es do Jogo (O resto do c�digo permanece o mesmo)

    public Transform GetTransformForStation(FoodStation station) { foreach (var s in foodStations) { if (s.stationDirection == station) return s.stationTransform; } return null; }
    private void AddFoodToPlayerSequence(FoodType food) 
    { if (playerSequence.Count >= sequenceLength || isGameOver) return; 
        playerSequence.Add(food);
        UpdatePlayerUI(); }

    #endregion
}