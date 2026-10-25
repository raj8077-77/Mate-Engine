using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Anshu Telegram Controller
/// - Magic Spell (hashed) to start 20-min session
/// - Receives text / voice / images
/// - Forwards to Google AI Brain and local models
/// </summary>
public class AnshuTelegramManager : MonoBehaviour
{
    [Header("Telegram")]
    public string botToken = "YOUR_BOT_TOKEN_HERE";
    public long allowedChatId = 0; // Set after first message or hardcode

    [Header("Magic Spell")]
    [Tooltip("SHA-256 hash of the Magic Spell (never store plain text)")]
    public string magicSpellHash = "PASTE_SHA256_HASH_HERE";

    [Header("Session")]
    public float sessionDurationMinutes = 20f;
    private float sessionEndTime = 0f;
    private bool sessionActive = false;

    private string lastUpdateId = "0";
    private bool isPolling = false;

    void Start()
    {
        Debug.Log("[Anshu] Telegram Manager started. Waiting for Magic Spell...");
        StartCoroutine(PollTelegram());
    }

    IEnumerator PollTelegram()
    {
        isPolling = true;
        while (isPolling)
        {
            string url = $"https://api.telegram.org/bot{botToken}/getUpdates?offset={lastUpdateId}&timeout=20";
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    ProcessUpdates(www.downloadHandler.text);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void ProcessUpdates(string json)
    {
        // TODO: Parse JSON properly (use Newtonsoft.Json or simple string check for prototype)
        // For now, basic detection of Magic Spell
        if (json.Contains("message") && !sessionActive)
        {
            // Check if message matches Magic Spell hash
            // Implement proper JSON parsing here
            Debug.Log("[Anshu] New message received. Checking Magic Spell...");
        }

        if (sessionActive && Time.time > sessionEndTime)
        {
            sessionActive = false;
            Debug.Log("[Anshu] Session expired. Magic Spell required again.");
        }
    }

    public bool CheckMagicSpell(string input)
    {
        string hash = ComputeSha256(input);
        if (hash == magicSpellHash)
        {
            sessionActive = true;
            sessionEndTime = Time.time + (sessionDurationMinutes * 60f);
            Debug.Log("[Anshu] Magic Spell accepted. Session started for 20 minutes.");
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
        if (!sessionActive) return;
        StartCoroutine(SendMessageCoroutine(text));
    }

    IEnumerator SendMessageCoroutine(string text)
    {
        string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={allowedChatId}&text={UnityWebRequest.EscapeURL(text)}";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
        }
    }

    void OnDestroy()
    {
        isPolling = false;
    }
}
