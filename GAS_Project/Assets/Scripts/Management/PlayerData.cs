using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//Class that contains most value affect the player directly
//like the amount of money he owns or what items and upgrádes

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Master mixer")]
    private AudioMixer masterMixer;
    public AudioMixer MasterMixer
    {
        get { return masterMixer; }
    }

    [SerializeField]
    [Tooltip("Master slider")]
    private Slider masterSlider;
    public Slider MasterSlider
    {
        get { return masterSlider; }
    }

    [SerializeField]
    [Tooltip("Effects slider")]
    private Slider effectsSlider;
    public Slider EffectsSlider
    {
        get { return effectsSlider; }
    }

    [SerializeField]
    [Tooltip("Music slider")]
    private Slider musicSlider;
    public Slider MusicSlider
    {
        get { return musicSlider; }
    }

    //instance of player data
    private static PlayerData _instance = null;
    public static PlayerData instance
    {
        get { return _instance; }
    }

    public int HighestDistance { get; set; } = 0;

    public int TotalDistance { get; set; } = 0;

    public int Deaths { get; set; } = 0;

    public int DestroyedObjects { get; set; } = 0;

    public int TotalGold { get; set; } = 0;

    public bool FirstPLayed { get; set; } = true;

    //save
    //Amount of money the player currently has
    private int currentMoney = 0;
    public int CurrentMoney
    {
        get { return currentMoney; }
        set 
        { 
            currentMoney = value;
            if (currentMoney < 0) currentMoney = 0;

            //If UpdateShopDisplay has an instance and the money text is assigned then update the money text
            if(UpdateShopDisplay.instance != null && UpdateShopDisplay.instance.MoneyText != null) 
                UpdateShopDisplay.instance.MoneyText.text = "" + currentMoney;
        }
    }

    private bool firstLoaded = true;

    //save
    //IDs of the permanent upgrades the player owns
    private SerializableDictionary<string, string> permanentUpgradeIDsPlayerOwns = new SerializableDictionary<string, string>();
    public SerializableDictionary<string, string> PermanentUpgradeIDsPlayerOwns
    {
        get { return permanentUpgradeIDsPlayerOwns; }
        set { permanentUpgradeIDsPlayerOwns = value; }
    }

    //save
    //dictionary containing the temporary upgrades the player has bought and how many of them
    private SerializableDictionary<Upgrade.UpgradeTypes, int> temporaryItemsOwned = new SerializableDictionary<Upgrade.UpgradeTypes, int>();
    public SerializableDictionary<Upgrade.UpgradeTypes, int> TemporaryItemsOwned
    {
        get { return temporaryItemsOwned; }
        set { temporaryItemsOwned = value; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        //Initialize the temporary items/boosters dictionary
        temporaryItemsOwned = new SerializableDictionary<Upgrade.UpgradeTypes, int>();
        temporaryItemsOwned.Add(Upgrade.UpgradeTypes.REFUEL, 0);
        temporaryItemsOwned.Add(Upgrade.UpgradeTypes.NUMSHIELDS, 0);
        temporaryItemsOwned.Add(Upgrade.UpgradeTypes.HEADSTART, 0);

        //load the last saved game
        if (firstLoaded)
        {
            SaveLoadService.LoadGame();
            firstLoaded = false;
        }

        Application.wantsToQuit += OnApplicationQuitting;
    }

    private void Start()
    {
        if (masterSlider == null) return;

        //initialize volume with saved values
        SetVolume("Master", masterSlider.value);
        SetVolume("Effects", effectsSlider.value);
        SetVolume("Music", musicSlider.value);
    }

    //Set the volume of the given group to given value
    private void SetVolume(string groupName, float value)
    {
        masterMixer.SetFloat(groupName, Mathf.Log10(value) * 20);
    }

    static bool OnApplicationQuitting()
    {
        Debug.Log("Quat");
        if(SelectionScreen.instance != null) SelectionScreen.instance.ResetBoostersTaken(true);
        SaveLoadService.SaveGame();
        Application.wantsToQuit -= OnApplicationQuitting;
        return true;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && !GameManager.InRun)
        {
            Debug.Log("Poose");
            SaveLoadService.SaveGame();
        }
    }
}
