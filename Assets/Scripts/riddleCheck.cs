using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class RiddleManager : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public Button cycleLeftButton;
    public GameObject solvedWindow;
    public Button yesButton;
    public Button noButton;
    public Button showSolve;
    public GameObject riddleLoadingGameObject;
    public GameObject riddleGameObject;
    public GameObject riddleAnswerGameObject;
    public GameObject roleSelectionWindow;
    public GameObject abilityScreen;

    public Button warriorButton;
    public Button magicianButton;
    public Button adventurerButton;
    public Button conjurerButton;

    public TextMeshProUGUI abilityRoleText;
    public TextMeshProUGUI abilityDescriptionText;
    public TextMeshProUGUI cyclesLeftText;
    public Button endTurnButton;

    public TextMeshProUGUI answerText;

    private int cycleCount = 1;
    private bool isSolving = false;
    private string currentGod;
    private string selectedRole;
    private int abilityCyclesLeft;
    private RandomSentenceManager randomSentenceManager;

    void Start()
    {
        cycleLeftButton.onClick.AddListener(OnCycleLeftClicked);
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        showSolve.onClick.AddListener(OnShowSolveClicked);

        warriorButton.onClick.AddListener(() => OnRoleSelected("Warrior"));
        magicianButton.onClick.AddListener(() => OnRoleSelected("Magician"));
        adventurerButton.onClick.AddListener(() => OnRoleSelected("Adventurer"));
        conjurerButton.onClick.AddListener(() => OnRoleSelected("Conjurer"));

        endTurnButton.onClick.AddListener(OnEndTurnClicked);

        solvedWindow.SetActive(false);
        riddleAnswerGameObject.SetActive(false);
        roleSelectionWindow.SetActive(false);
        abilityScreen.SetActive(false);

        randomSentenceManager = FindObjectOfType<RandomSentenceManager>();
    }

    void OnEndTurnClicked()
    {
        abilityCyclesLeft--;
        cyclesLeftText.text = $"Cycles Left: {abilityCyclesLeft}";

        if (abilityCyclesLeft <= 0)
        {
            abilityScreen.SetActive(false);
            RestartRiddleProcess();
        }
    }

    void OnCycleLeftClicked()
    {
        if (isSolving) return;

        cycleCount--;
        counterText.text = cycleCount.ToString();

        if (cycleCount <= 0)
        {
            ShowRiddleAnswer();
        }
    }

    void ShowRiddleAnswer()
    {
        riddleGameObject.SetActive(false);
        riddleAnswerGameObject.SetActive(true);

        string answer = randomSentenceManager.GetCurrentAnswer();
        answerText.text = answer;

        currentGod = randomSentenceManager.GetCurrentGod();

        PlaySolvedSound(currentGod);
    }

    void OnShowSolveClicked()
    {
        solvedWindow.SetActive(true);
        riddleAnswerGameObject.SetActive(false);
    }
    void OnYesClicked()
    {
        solvedWindow.SetActive(false);
        ShowRoleSelectionWindow();
    }

    void OnNoClicked()
    {
        solvedWindow.SetActive(false);
        RestartRiddleProcess();
    }

    void RestartRiddleProcess()
    {
        cycleCount = 1;
        counterText.text = cycleCount.ToString();

        riddleGameObject.SetActive(false);
        riddleLoadingGameObject.SetActive(true);

        randomSentenceManager.ShowRandomSentence();
    }

    void ShowRoleSelectionWindow()
    {
        roleSelectionWindow.SetActive(true);
    }

    void OnRoleSelected(string role)
    {
        selectedRole = role;
        roleSelectionWindow.SetActive(false);
        ShowAbilityScreen();
    }

    void ShowAbilityScreen()
    {
        abilityScreen.SetActive(true);
        abilityRoleText.text = selectedRole;

        string abilityDescription = GetAbilityDescription(selectedRole, currentGod);
        abilityDescriptionText.text = abilityDescription;

        abilityCyclesLeft = 2;
        cyclesLeftText.text = $"Cycles Left: {abilityCyclesLeft}";
    }

    string GetAbilityDescription(string role, string god)
    {
        switch (role)
        {
            case "Warrior":
                if (god == "God of Wisdom") return "Fully Infused: You now use the range of the magic card when imbuing a physical card.";
                if (god == "God of War") return "Bladeworks:\nAll your physical cards now costs 2 physical resources, has 2 range, and deals 3 damage.";
                if (god == "God of Wealth") return "Greed's Blade:\nEvery time you successfully deal damage, draw 1 card.";
                break;

            case "Magician":
                if (god == "God of Wisdom") return "Overload:\nWhen doing a magic combination, double the imbue effect of one of the magic used in the combination.";
                if (god == "God of War") return "Overcharge:\nWhen using a magic by itself, double its effect.";
                if (god == "God of Wealth") return "Golem Construct:\nOnce per turn, you may discard an Item card to summon a Golem, represented by a token/marker, that has the following effect:\nThey have 1 speed and you move them during your turn. When they come in contact with a player, the golem deals 3 to every player in that tile. They persists even when awakening has ended.";
                break;

            case "Adventurer":
                if (god == "God of Wisdom") return "Secret Passage:\nAll field tiles now act as a Portal only to you. You may move from one field tile to another at the cost of 1 movement.";
                if (god == "God of War") return "Bear Trap:\nYour physical cards all have the same attributes:\nCost: 2\nEffect: Place a Trap Marker on the tile you're standing on which will turn the tile into a Trap tile that deals 2 damage to any player who enters. They persists even when awakening has ended.";
                if (god == "God of Wealth") return "Efficiency:\nDouble the effects of every Item Card";
                break;

            case "Conjurer":
                if (god == "God of Wisdom") return "Expend:\nIncrease the damage of an imbued attack by half of the amount of Magic Resources used rounded up.";
                if (god == "God of War") return "Exhaust:\nIncrease the damage of an imbued attack by half of the amount of Physical Resources used rounded up.";
                if (god == "God of Wealth") return "Transmute:\nYou may use Item Cards as a substitute for a single Physical or Magic Resource";
                break;
        }

        return "Unknown Ability";
    }

    void PlaySolvedSound(string god)
    {
        AudioSource audioSource = randomSentenceManager.GetComponent<AudioSource>(); // Get the AudioSource from RandomSentenceManager

        // Save the current background music clip if needed
        AudioClip backgroundMusicClip = audioSource.clip;

        // Stop the background music before playing the solved clip
        audioSource.Stop();

        switch (god)
        {
            case "God of Wisdom":
                audioSource.PlayOneShot(randomSentenceManager.solvedWisdomClip);
                break;
            case "God of War":
                audioSource.PlayOneShot(randomSentenceManager.solvedWarClip);
                break;
            case "God of Wealth":
                audioSource.PlayOneShot(randomSentenceManager.solvedWealthClip);
                break;
            default:
                Debug.LogWarning("No sound assigned for this god.");
                break;
        }

        // Optionally, resume background music after the solved sound is done
        StartCoroutine(ResumeBackgroundMusic(audioSource, backgroundMusicClip));
    }

    IEnumerator ResumeBackgroundMusic(AudioSource audioSource, AudioClip backgroundMusicClip)
    {
        yield return new WaitForSeconds(1f); // Wait for the solved sound clip to finish

        // Resume the background music if there was a clip
        if (backgroundMusicClip != null)
        {
            audioSource.clip = backgroundMusicClip;
            audioSource.Play();
        }
    }


}
