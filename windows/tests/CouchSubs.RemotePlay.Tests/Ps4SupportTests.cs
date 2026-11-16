// SPDX-License-Identifier: AGPL-3.0-only

using System.Text.Json;

namespace CouchSubs.RemotePlay.Tests;

public class Ps4SupportTests
{
    [Fact]
    public void StoredRegistrationFromBeforePs4SupportReadsAsPs5()
    {
        // PS4 desteğinden önce SecureStore'a yazılmış kayıtta IsPs4 alanı yoktur.
        const string json = """{"Host":"192.168.1.20","Name":"PS5-001","RegistKey":"QUFB","Morning":"QUFB"}""";
        var console = JsonSerializer.Deserialize<RegisteredConsole>(json);
        Assert.NotNull(console);
        Assert.False(console!.IsPs4);
    }

    [Fact]
    public void Ps4RegistrationSurvivesSerializationAndHostChange()
    {
        var console = new RegisteredConsole("192.168.1.30", "PS4-001", new byte[16], new byte[16], IsPs4: true);
        var roundTripped = JsonSerializer.Deserialize<RegisteredConsole>(JsonSerializer.Serialize(console));
        Assert.True(roundTripped!.IsPs4);
        Assert.True(console.WithHost("192.168.1.31").IsPs4);
    }

    [Fact]
    public void PickMovedWorksOnPs4ListsToo()
    {
        var found = new[] { new ConsoleInfo("192.168.1.31", "PS4-001", true, IsPs4: true) };
        Assert.Equal("192.168.1.31", ConsoleDiscovery.PickMoved(found, "PS4-001")?.Host);
    }
}
