using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ReplayUIController : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private Button recordButton;

    [SerializeField] private Button stopButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_InputField seedInput;
    [SerializeField] private Recorder recorder;
    [SerializeField] private Player player;
    [SerializeField] private TickManager tickManager;
    
    private DeterministicRng rng;
    private ReplayData lastReplay;

    void Start()
    {
        int initialSeed = Random.Range(1, 999999);
        if (!string.IsNullOrEmpty(seedInput.text))
            int.TryParse(seedInput.text, out initialSeed);
        rng = new DeterministicRng(initialSeed);
        seedInput.text = initialSeed.ToString();
        
        recordButton.onClick.AddListener(OnRecordStart);
        stopButton.onClick.AddListener(OnRecordStop);
        playButton.onClick.AddListener(OnPlayLast);
        saveButton.onClick.AddListener(OnSaveReplay);
        loadButton.onClick.AddListener(OnLoadReplay);

        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
        
        if (Input.GetKeyDown(KeyCode.F1)) OnRecordStart();
        if (Input.GetKeyDown(KeyCode.F2)) OnRecordStop();
        if (Input.GetKeyDown(KeyCode.F3)) OnPlayLast();
    }

    void OnRecordStart()
    {
        if (recorder.IsRecording) return;

        int newSeed;
        if (!int.TryParse(seedInput.text, out newSeed))
            newSeed = Random.Range(1, 999999);

        rng.SetSeed(newSeed);
        recorder.StartRecording(newSeed);
        seedInput.text = newSeed.ToString();
        tickManager.ResetTick();
    }

    void OnRecordStop()
    {
        if (!recorder.IsRecording) return;

        recorder.StopRecording();
        lastReplay = recorder.GetReplayData();
        SaveReplayToFile(lastReplay, "last_replay.replay");
        statusText.text = $"Recorded {lastReplay.Commands.Count} commands";
    }

    void OnPlayLast()
    {
        if (player.IsPlaying) return;
        if (lastReplay == null) return;
        
        rng.SetSeed(lastReplay.Seed);
        player.Play(lastReplay);
        statusText.text = "Playing";
    }

    void OnSaveReplay()
    {
        if (lastReplay == null) return;
        string filename = $"replay_{System.DateTime.Now:yyyyMMdd_HHmmss}.replay";
        SaveReplayToFile(lastReplay, filename);
        statusText.text = $"Saved to {filename}";
    }

    void OnLoadReplay()
    {
        string path = Application.dataPath + "/Replays/";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        var files = Directory.GetFiles(path, "*.replay");
        if (files.Length > 0)
        {
            LoadReplayFromFile(files[files.Length - 1]);
            statusText.text = $"Loaded {Path.GetFileName(files[files.Length - 1])}";
        }
    }

    void SaveReplayToFile(ReplayData data, string filename)
    {
        string path = Application.dataPath + "/Replays/";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path + filename, json);
        Debug.Log($"Replay saved to {path + filename}");
    }

    void LoadReplayFromFile(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            lastReplay = JsonUtility.FromJson<ReplayData>(json);
            Debug.Log($"Replay loaded from {path}");
        }
    }

    void UpdateUI()
    {
        if (recorder.IsRecording)
            statusText.text = $"Recording... (seed: {rng.GetSeed()})";
        else if (player.IsPlaying)
            statusText.text = "Playing...";
        else
            statusText.text = "Ready";

        recordButton.interactable = !recorder.IsRecording && !player.IsPlaying;
        stopButton.interactable = recorder.IsRecording || player.IsPlaying;
        playButton.interactable = lastReplay != null && !player.IsPlaying;
    }
}