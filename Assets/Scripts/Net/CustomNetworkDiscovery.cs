using UnityEngine;
using Mirror;
using Mirror.Discovery;
using System;
using System.Net;
using kcp2k;

public class CustomNetworkDiscovery : NetworkDiscoveryBase<ServerRequest, ServerResponse>
{
    public NetworkManagerRTS networkManagerRTS;

    protected override ServerResponse ProcessRequest(ServerRequest request, IPEndPoint endpoint)
    {
        return new ServerResponse
        {
            uri = new Uri($"kcp://127.0.0.1:{networkManagerRTS.GetComponent<KcpTransport>().Port}"), // Force localhost
            serverId = ServerId,
            ipAddress = "127.0.0.1",
            port = networkManagerRTS.GetComponent<KcpTransport>().Port,
            currentPlayers = networkManagerRTS.numPlayers,
            maxPlayers = networkManagerRTS.maxConnections
        };
    }

    public new void StartDiscovery()
    {
        if (networkManagerRTS == null)
        {
            networkManagerRTS = FindObjectOfType<NetworkManagerRTS>();
        }

        if (networkManagerRTS != null)
        {
            Debug.Log("Starting network discovery...");
            base.StartDiscovery();
        }
        else
        {
            Debug.LogError("NetworkManagerRTS not found!");
        }
    }

    public new void StopDiscovery()
    {
        Debug.Log("Stopping network discovery...");
        base.StopDiscovery();
    }

    protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint)
    {
        FindObjectOfType<MainMenu>().OnDiscoveredServer(response);
    }
}


public struct ServerRequest : NetworkMessage { }

public struct ServerResponse : NetworkMessage
{
    public Uri uri;
    public long serverId;
    public int currentPlayers;
    public int maxPlayers;
    public string ipAddress;
    public int port;
}