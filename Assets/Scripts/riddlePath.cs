using UnityEngine;
using TMPro; // Import the TextMeshPro namespace
using UnityEngine.UI; // Import for Button
using System.Collections;

public class RandomSentenceManager : MonoBehaviour
{
    public TextMeshProUGUI sentenceText;
    public TextMeshProUGUI riddleText;
    public Button startButton;
    public GameObject menuGameObject;
    public GameObject riddleLoadingGameObject;
    public GameObject riddleGameObject;

    public AudioSource Audio;
    public AudioClip wisdomMusic;
    public AudioClip warMusic;
    public AudioClip wealthMusic;

    public AudioClip solvedWisdomClip;
    public AudioClip solvedWarClip;
    public AudioClip solvedWealthClip;

    private string currentGod;
    private int currentSentenceIndex = -1;
    private int currentRiddleIndex = -1;

    private string[] sentences = {
        "The God of Wisdom calls...",
        "The God of War calls...",
        "The God of Wealth calls..."
    };

    private string[][] riddles = {
        new string[] {
            "In the midst of the fiery crater's heat,\nThou shalt offer the twin flames to greet.",
            "In the stillness of sacred waters deep,\nThou shalt offer the twin streams to keep.",
            "Within the halls of knowledge grand,\nThou must surrender wisdom, understand.",
            "Where forces meet, the elements align,\nOffer Fire, Water, Air, and Earth in kind.\nOnly when all are given, the power is clear,\nAnd the path ahead will soon appear.",
            "In the realm of the arcane, where powers are held,\nReveal thy hand, let thy secrets be quelled.\nThe one with the fewest, their power shall arise,\nAwaken, O chosen, to claim the prize.",
            "In the realm of the arcane, where powers are held,\nReveal thy hand, let thy secrets be quelled.\nThe one with the most, their fate shall unfold,\nAwaken, O chosen, with power untold.",
            "Discard thy magic, take the price,\nChoose to stop or pay the sacrifice.\nThe one who offers most shall rise,\nAwakened, they claim their prize."
        },
        new string[] {
            "The graveyard calls—two blades must fall,\nGive them up, or gain nothing at all.",
            "Lost in the forest where truths distort,\nThrow two punches, but not from the court.",
            "Step onto the field, where battles are won,\nThree strikes, all different, and the deed is done.",
            "Where warriors toil, the price is clear,\nA mighty tool falls, and pain draws near.",
            "Show what you’ve got, but don’t be too proud,\nThe one with the fewest will rise from the crowd,\nAnd take the first swing when the battle is loud.",
            "Show your strength, let it be seen,\nThe one with the most will take the lead,\nAnd awaken, ready to strike the scene.",
            "Discard a card, take the hit,\nJoin the cycle, or call it quit.\nThe one with most shall rise and lead,\nTheir turn begins, their fate decreed."
        },
        new string[] {
            "In the tomb, toss two tricks away,\nAnd let’s see what happens—what do you say?",
            "Trade a bit of life, it’s no big deal,\nTwo sips gone, just to make it real.",
            "Three trinkets to lose, do you really need them?\nWhat’s the worst that could happen?",
            "Pop two pills, roll the dice,\nIt’s a gamble, but that’s the price.\nTake the hit, don’t think twice.",
            "Show your trinkets, let’s see who’s light,\nLowest count gets the first bite.\nReady to go, or just a little fright?",
            "Show your trinkets, who’s holding the most?\nThe one with plenty gets to boast.\nReady to lead, or just playing close?",
            "Toss your trinkets, feel the sting,\nHow much damage will you bring?\nThe one with most gets the first swing,\nStep up now, let’s see what you’ll bring."
        }
    };

    private string[][] answers = {
        new string[] {
            "At the Volcano Tile, sacrifice 2 Fire Cards",
            "At the Pond Tile, sacrifice 2 Water Cards",
            "At the Library Tile, sacrifice 3 different Magic Cards",
            "At the Altar Tile, sacrifice a Fire, Water, Air, and Earth Card.",
            "Each player shows all the magic cards on their hand. Whoever has the lowest number of magic card starts their turn and awakens.",
            "Each player shows all the magic cards on their hand. Whoever has the highest number of magic card starts their turn and awakens.",
            "A cycle starts where each player may discard a magic card and deal 1 damage to themselves. They may choose to not participate anymore. Whoever has the highest number of discarded magic cards immediately starts their turn and awakens."
        },
        new string[] {
            "At the Graveyard Tile, sacrifice 2 Sword Cards.",
            "At the Illusion Forest Tile, sacrifice 2 Punch Cards. Cards discarded through the effect of the trap do not count",
            "At the Battlefield Tile, sacrifice 3 different Physical Cards.",
            "At the Training Grounds Tile, sacrifice 1 Hammer then lose 4 HP.",
            "Each player shows all the physical cards on their hand. Whoever has the lowest number of physical card starts their turn and awakens.",
            "Each player shows all the physical cards on their hand. Whoever has the highest number of physical card starts their turn and awakens.",
            "A cycle starts where each player may discard a physical card and deal 1 damage to themselves. They may choose to not participate anymore. Whoever has the highest number of discarded physical cards immediately starts their turn and awakens."
        },
        new string[] {
            "At the Undead's Tomb Tile, sacrifice 2 Pickpocket Cards",
            "At the Sanctuary Tile, sacrifice 2 Health Potion Cards",
            "At the Tavern Tile, sacrifice 3 different Item Cards",
            "At the Training Grounds Tile, sacrifice 2 Steroids Cards. Roll 2 dice then lose HP equal to the result.",
            "Each player shows all the item cards on their hand. Whoever has the lowest number of item card starts their turn and awakens.",
            "Each player shows all the item cards on their hand. Whoever has the highest number of item card starts their turn and awakens.",
            "A cycle starts where each player may discard an item card and deal 1 damage to themselves. They may choose to not participate anymore. Whoever has the highest number of discarded item cards immediately starts their turn and awakens."
        }
    };

    void Start()
    {



        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void OnStartButtonClick()
    {
        ShowRandomSentence();
    }

    public void ShowRandomSentence()
    {
        menuGameObject.SetActive(false);
        riddleLoadingGameObject.SetActive(true);

        currentSentenceIndex = Random.Range(0, sentences.Length);
        currentGod = sentences[currentSentenceIndex].Replace("The ", "").Replace(" calls...", "").Trim();
        sentenceText.text = sentences[currentSentenceIndex];

        PlayBackgroundMusic();

        StartCoroutine(WaitAndActivateRiddle(3f));
    }

    IEnumerator WaitAndActivateRiddle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        riddleLoadingGameObject.SetActive(false);
        riddleGameObject.SetActive(true);

        ShowRandomRiddle();
    }

    void ShowRandomRiddle()
    {
        if (currentSentenceIndex >= 0 && currentSentenceIndex < riddles.Length)
        {
            string[] selectedRiddles = riddles[currentSentenceIndex];
            currentRiddleIndex = Random.Range(0, selectedRiddles.Length);
            riddleText.text = selectedRiddles[currentRiddleIndex];
        }
        else
        {
            Debug.LogError("Invalid sentence index for riddles.");
        }
    }

    void PlayBackgroundMusic()
    {
        if (Audio == null)
        {
            Debug.LogError("AudioSource is not assigned.");
            return;
        }

        Audio.Stop();

        switch (currentGod)
        {
            case "God of Wisdom":
                Audio.clip = wisdomMusic;
                break;
            case "God of War":
                Audio.clip = warMusic;
                break;
            case "God of Wealth":
                Audio.clip = wealthMusic;
                break;
            default:
                Debug.LogWarning("No matching music for the selected god.");
                return;
        }

        Audio.Play();
        Debug.Log("Background Music: " + Audio.clip.name + " is now playing.");
    }

    public void PlaySolvedClip()
    {
        if (Audio == null)
        {
            Debug.LogError("AudioSource is not assigned.");
            return;
        }

     
        Audio.Stop();
        Debug.Log("Stopping background music before playing solved clip.");

        switch (currentGod)
        {
            case "God of Wisdom":
                Audio.PlayOneShot(solvedWisdomClip);
                break;
            case "God of War":
                Audio.PlayOneShot(solvedWarClip);
                break;
            case "God of Wealth":
                Audio.PlayOneShot(solvedWealthClip);
                break;
            default:
                Debug.LogWarning("No matching solved clip for the selected god.");
                return;
        }

        Debug.Log("Solved Clip: " + Audio.clip.name + " is now playing.");
    }

    public string GetCurrentGod() => currentGod;

    public string GetCurrentAnswer()
    {
        if (currentSentenceIndex >= 0 && currentSentenceIndex < answers.Length &&
            currentRiddleIndex >= 0 && currentRiddleIndex < answers[currentSentenceIndex].Length)
        {
            return answers[currentSentenceIndex][currentRiddleIndex];
        }

        Debug.LogWarning("No valid answer available.");
        return "No answer available";
    }
}
