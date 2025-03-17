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
            uri = new Uri($"kcp://{endpoint.Address}:{networkManagerRTS.GetComponent<KcpTransport>().Port}"),
            serverId = ServerId,
            ipAddress = endpoint.Address.ToString(),
            port = networkManagerRTS.GetComponent<KcpTransport>().Port,
            currentPlayers = networkManagerRTS.numPlayers,
            maxPlayers = networkManagerRTS.maxConnections
        };
    }

    public void StartDiscovery()
    {
        base.StartDiscovery();
    }

    public void StopDiscovery()
    {
        base.StopDiscovery();
    }

    protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint)
    {
        MainMenu mainMenu = FindObjectOfType<MainMenu>();
        if (mainMenu != null)
        {
            mainMenu.OnDiscoveredServer(response);
        }
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