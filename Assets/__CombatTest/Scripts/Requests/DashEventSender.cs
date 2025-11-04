using Proyecto26;
using System;
using UnityEngine;
using Zenject;
using static GameInstaller;

public class ApiEventBridge : MonoBehaviour
{

    [Inject] SignalBus _signalBus;

    [Header("API")]
    public string baseUrl = "http://localhost:9002/api/events";

    [Header("Options")]
    public bool onAttackWithHitSendsHit = true;

    // Sample data (used if you want to populate attack info)
    public string sampleAttacker = "Player";
    public string sampleDefender = "Dummy";
    public string sampleAttackType = "Sword Slash";
    public string sampleCharacter = "Player";

    private bool attackHitThisFrame = false;
    private void OnEnable()
    {
        _signalBus.Subscribe<PunshBTNSignal>(StartedAttackRequest);
        _signalBus.Subscribe<KickBTNSignal>(StartedAttackRequest);

        _signalBus.Subscribe<KickRecived>(OnAttackWithHit);
        _signalBus.Subscribe<PunchRecived>(OnAttackWithHit);
        _signalBus.Subscribe<DashBTNSignal>(OnDash);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<PunshBTNSignal>(StartedAttackRequest);
        _signalBus.Unsubscribe<KickBTNSignal>(StartedAttackRequest);
        _signalBus.Unsubscribe<KickRecived>(OnAttackWithHit);
        _signalBus.Unsubscribe<PunchRecived>(OnAttackWithHit);
        _signalBus.Unsubscribe<DashBTNSignal>(OnDash);
    }

    // Sends a full attack event
    // Sends a simple attack attempt (no hit info, silent)
    public void StartedAttackRequest()
    {
        string url = $"{baseUrl}/attack";

        // Send an empty JSON object {} to indicate an attack attempt
        string json = "{}";

        Debug.Log($"OnAttack() -> POST attack (silent) JSON: {json}");
        PostJson(url, json, HandleResponse, HandleError);
    }


    public void OnAttackWithHit(BaseAttackEvent attackEvent)
    {
        attackHitThisFrame = true; // mark that this attack hit

        string attackType = "Unknown";

        // Detect the type of attack
        if (attackEvent is KickRecived)
            attackType = "Kick";
        else if (attackEvent is PunchRecived)
            attackType = "Punch";

        var payload = new AttackRequest
        {
            attacker = sampleAttacker,
            defender = sampleDefender,
            attackType = attackType,
            hit = onAttackWithHitSendsHit
        };

        string url = $"{baseUrl}/attack";
        string json = JsonUtility.ToJson(payload);
        Debug.Log($"OnAttackWithHit() -> POST attack JSON: {json}");
        PostJson(url, json, HandleResponse, HandleError);
    }

    // Sends a dash event
    public void OnDash()
    {
        var payload = new DashRequest { character = sampleCharacter };
        string url = $"{baseUrl}/dash";
        string json = JsonUtility.ToJson(payload);
        Debug.Log($"OnDash() -> POST dash JSON: {json}");
        PostJson(url, json, HandleResponse, HandleError);
    }

    private void PostJson(string url, string jsonBody, Action<string> onSuccess, Action<string> onError)
    {
        var req = new RequestHelper
        {
            Uri = url,
            Method = "POST",
            BodyString = jsonBody,
            ContentType = "application/json",
            EnableDebug = true
        };

        RestClient.Request(req)
            .Then(response =>
            {
                Debug.Log($"API response ({url}) Status: {response.StatusCode} Text: {response.Text}");
                onSuccess?.Invoke(response.Text);
            })
            .Catch(err =>
            {
                Debug.LogError($"API request error ({url}): {err}");
                onError?.Invoke(err.ToString());
            });
    }

    private void HandleResponse(string responseText)
    {
        try
        {
            var parsed = JsonUtility.FromJson<ApiResponse>(responseText);
            if (parsed != null)
            {
                Debug.Log($"Parsed response: message='{parsed.message}', handled={parsed.handled}");
                if (!parsed.handled)
                    Debug.LogWarning("Server accepted the request but no dashboard instance handled it.");
            }
            else
            {
                Debug.Log("Received response (unparsed): " + responseText);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to parse API response: " + e.Message + " -> raw: " + responseText);
        }
    }

    private void HandleError(string err)
    {
        Debug.LogError("API error: " + err);
    }
}

[Serializable]
public class AttackRequest
{
    public string attacker;
    public string defender;
    public string attackType;
    public bool hit;
}

[Serializable]
public class DashRequest
{
    public string character;
}

[Serializable]
public class ApiResponse
{
    public string message;
    public bool handled;
}
