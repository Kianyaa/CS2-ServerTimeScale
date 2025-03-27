using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

using Microsoft.Extensions.Logging;

namespace ServerTimeScale;

public class ServerTimeScale : BasePlugin
{
    public override string ModuleName => "ServerTimeScale";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Kianya";
    public override string ModuleDescription => "control host_timescale command in cs2 server";

    private float _startTimeScale = 1.0f;

    public override void Load(bool hotReload)
    {
        Logger.LogInformation("ServerTimeScale are loaded");
    }

    [ConsoleCommand("css_timescale", "control host_timescale command in cs2 server")]
    //[RequiresPermissions("@css/admin")] // TODO: Add permission for admin only
    [CommandHelper(minArgs: 1, usage: "expect value between 0 - 10", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void TimeScaleCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player != null && player is { IsValid: true, PlayerPawn.IsValid: true } &&
            player.Connected == PlayerConnectedState.PlayerConnected)
        {
            var value = commandInfo.GetArg(1);

            //if (commandInfo.ArgCount != 1) 
            //{

            //    player.PrintToChat($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Time scale the server between 0 - 10");

            //}

            // Check if the value is a number
            if (float.TryParse(value, out var valueExecute))
            {
                // Limit to 1 decimal place
                valueExecute = (float)Math.Round(valueExecute, 1);

                if (Math.Abs(valueExecute - _startTimeScale) < 0.01f)
                {
                    Server.ExecuteCommand($"host_timescale 1");
                    player.PrintToChat($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Time scale reset to {ChatColors.Red}1");
                }

                else if (valueExecute == 0.0f)
                {
                    player.PrintToChat($" {ChatColors.Red}[Warning] {ChatColors.Green}[TimeScale] {ChatColors.Default}Time scale can not be 0");
                }

                else
                {
                    Server.ExecuteCommand($"sv_cheats 1"); // TODO: Its open cheat to true
                    Server.ExecuteCommand($"host_timescale {valueExecute}"); 
                    _startTimeScale = valueExecute;
                    player.PrintToChat($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Time scale set to {ChatColors.Red}{valueExecute}");
                }

            }

            else
            {

                player.PrintToChat($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Time scale the server between 0 - 10");

            }
            
        }

        else
        {
            Logger.LogError("This command running from the server");
        }
    }


    public override void Unload(bool hotReload)
    {
        Logger.LogInformation("ServerTimeScale are unloaded");
    }
}