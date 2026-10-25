using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Anshu Telegram Controller
/// - Settings se Bot Token aur Magic Spell set/reset kar sakte ho
/// - Magic Spell hashed save hota hai
/// - Reset ke liye current password chahiye
/// </summary>
public class AnshuTelegramManager : MonoBehaviour
{
    [Header("Telegram")]
    public string botToken = "";
    public long allowedChatId = 0;

    [Header("Magic Spell")]
    public string magicSpellHash = "";

    [Header("Session")]
    public float sessionDurationMinutes = 20f;
    private float sessionEndTime = 0f;
    public bool sessionActive = false;

    private string lastUpdateId = "0";
    private bool isPolling = false;

    private string configPath;

    [Serializable]
    class AnshuConfig
    {
        public string botToken;
        public string magicSpellHash;
        public long allowedChatId;
    }

    void Awake()
    {
        configPath = Path.Combine(Application.persistentDataPath, "anshu_config.json");
        LoadConfig();
    }

    void Start()
    {
        Debug.Log("[Anshu] Telegram Manager ready.");
        if (!string.IsNullOrEmpty(botToken))
            StartCoroutine(PollTelegram());
    }

    public void SaveConfig()
    {
        var cfg = new AnshuConfig
        {
            botToken = botToken,
            magicSpellHash = magicSpellHash,
            allowedChatId = allowedChatId
        };
        File.WriteAllText(configPath, JsonUtility.ToJson(cfg, true));
        Debug.Log("[Anshu] Config saved.");
    }

    void LoadConfig()
    {
        if (!File.Exists(configPath)) return;
        try
        {
            var cfg = JsonUtility.FromJson<AnshuConfig>(File.ReadAllText(configPath));
            botToken = cfg.botToken;
            magicSpellHash = cfg.magicSpellHash;
            allowedChatId = cfg.allowedChatId;
            Debug.Log("[Anshu] Config loaded.");
        }
        catch { }
    }

    // Settings se call karna: Set new Magic Spell (hash bana ke)
    public void SetMagicSpell(string plainSpell)
    {
        magicSpellHash = ComputeSha256(plainSpell);
        SaveConfig();
        Debug.Log("[Anshu] Magic Spell updated.");
    }

    // Reset ke liye current password verify karo
    public bool VerifyAndResetSpell(string currentPlain, string newPlain)
    {
        if (ComputeSha256(currentPlain) != magicSpellHash)
        {
            Debug.Log("[Anshu] Wrong current password.");
            return false;
        }
        SetMagicSpell(newPlain);
        return true;
    }

    public void SetBotToken(string token)
    {
        botToken = token;
        SaveConfig();
        if (!isPolling && !string.IsNullOrEmpty(botToken))
            StartCoroutine(PollTelegram());
    }

    IEnumerator PollTelegram()
    {
        isPolling = true;
        while (isPolling && !string.IsNullOrEmpty(botToken))
        {
            string url = $"https://api.telegram.org/bot{botToken}/getUpdates?offset={lastUpdateId}&timeout=15";
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                    ProcessUpdates(www.downloadHandler.text);
            }
            yield return new WaitForSeconds(0.5f);
        }
        isPolling = false;
    }

    void ProcessUpdates(string json)
    {
        // Basic prototype — proper JSON parsing baad mein add karenge
        if (sessionActive && Time.time > sessionEndTime)
        {
            sessionActive = false;
            Debug.Log("[Anshu] Session expired.");
        }
    }

    public bool CheckMagicSpell(string input)
    {
        if (string.IsNullOrEmpty(magicSpellHash)) return false;
        if (ComputeSha256(input) == magicSpellHash)
        {
            sessionActive = true;
            sessionEndTime = Time.time + (sessionDurationMinutes * 60f);
            Debug.Log("[Anshu] Session started.");
            return true;
        }
        return false;
    }

    string ComputeSha256(string raw)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    public void SendTelegramMessage(string text)
    {
        if (!sessionActive || string.IsNullOrEmpty(botToken)) return;
        StartCoroutine(SendMessageCoroutine(text));
    }

    IEnumerator SendMessageCoroutine(string text)
    {
        string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={allowedChatId}&text={UnityWebRequest.EscapeURL(text)}";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
            yield return www.SendWebRequest();
    }

    void OnDestroy() => isPolling = false;
}
