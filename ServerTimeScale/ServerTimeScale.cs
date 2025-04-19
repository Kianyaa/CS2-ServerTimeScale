using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.Logging;


namespace ServerTimeScale;

public class ServerTimeScale : BasePlugin
{
    public override string ModuleName => "ServerTimeScale";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Kianya";
    public override string ModuleDescription => "control host_timescale command in cs2 server";

    private float _startTimeScale = 1.00f;
    private ConVar? _timeScale = null;

    public override void Load(bool hotReload)
    {
        _timeScale = ConVar.Find("host_timescale");
    }

    [ConsoleCommand("css_timescale", "control host_timescale command in cs2 server")]
    [RequiresPermissions("@css/generic")]
    [CommandHelper(minArgs: 1, usage: "expect value between 0 - 10", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void TimeScaleCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player != null && player is { IsValid: true, PlayerPawn.IsValid: true } &&
            player.Connected == PlayerConnectedState.PlayerConnected)
        {
            var value = commandInfo.GetArg(1);


            // Check if the value is a number
            if (float.TryParse(value, out var valueExecute))
            {

                // Limit to 2 decimal place
                valueExecute = (float)Math.Round(valueExecute, 2);

                // Check if the value is between 0 and 10
                if (valueExecute is > 10.00f or < 0.00f)
                {
                    player.PrintToChat($" {ChatColors.Red}[Warning] {ChatColors.Green}[TimeScale] {ChatColors.Default}Server time scale the server can be only between 0 - 10");
                    return;
                }

                // If _startTimeScale set to 1.0f after round start and user input also 1.0f 
                if ((Math.Abs(valueExecute - 1.00f) < 0.001f) && (Math.Abs(_startTimeScale - 1.00f) < 0.001f))
                {
                    _timeScale!.Flags &= ~ConVarFlags.FCVAR_CHEAT;
                    _timeScale.SetValue(valueExecute);

                    Server.PrintToChatAll($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Server time scale set to {ChatColors.Red}1");
                    return;
                }

                // Check if the value is the same as the current timescale
                if (Math.Abs(valueExecute - _startTimeScale) < 0.001f)
                {
                    _startTimeScale = 1.00f;
                    _timeScale!.Flags &= ~ConVarFlags.FCVAR_CHEAT;
                    _timeScale.SetValue(_startTimeScale);


                    Server.PrintToChatAll($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Server time scale reset to {ChatColors.Red}1");
                    return;
                }

                // Check if the value is 0
                if (valueExecute == 0.00f)
                {
                    player.PrintToChat($" {ChatColors.Red}[Warning] {ChatColors.Green}[TimeScale] {ChatColors.Default}Server time scale can not be 0");
                    return;
                }

                // Set the timescale if passes all the checks 

                _timeScale!.Flags &= ~ConVarFlags.FCVAR_CHEAT;
                _timeScale.SetValue(valueExecute);
                _startTimeScale = valueExecute;

                Server.PrintToChatAll($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Server time scale set to {ChatColors.Red}{valueExecute:0.##}");
                
            }

            // Invalid value can not be converted to a number usually this is a string input
            else
            {

                player.PrintToChat($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Invalid input. Use a number between 0 - 10.");

            }
            
        }

        // Command executed from the server
        if (player == null) 
        {
            var value = commandInfo.GetArg(1);

            if (float.TryParse(value, out var valueExecute) && valueExecute is > 0.00f and <= 10.00f)
            {
                valueExecute = (float)Math.Round(valueExecute, 2);

                _timeScale!.Flags &= ~ConVarFlags.FCVAR_CHEAT;
                _timeScale.SetValue(valueExecute);
                _startTimeScale = valueExecute;

                Logger.LogInformation($"[Server]:Server time scale set to {valueExecute:0.##}");
                Server.PrintToChatAll($" {ChatColors.DarkRed}[Server] {ChatColors.Green}[TimeScale] {ChatColors.Default}Server Time scale set to {ChatColors.Red}{valueExecute:0.##}");
            }

            else
            {
                Logger.LogError("Invalid timescale value. Expected a number between 0 - 10.");
            }
        }
    }

    // Reset the timescale to 1 when the round ends
    [GameEventHandler(HookMode.Post)]
    public HookResult OnEventRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        
        var value = _timeScale!.GetPrimitiveValue<float>();

        if (Math.Abs(value - 1.00f) > 0.01f)
        {
            _timeScale!.Flags &= ~ConVarFlags.FCVAR_CHEAT;
            _timeScale.SetValue(1.00f);
            _startTimeScale = 1.00f;

            Server.PrintToChatAll($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Server time scale reset to {ChatColors.Red}1");
            
        }

        return HookResult.Continue;
    }

    // Reset the timescale to 1 when the round starts
    [GameEventHandler(HookMode.Post)]
    public HookResult OnEventRoundStart(EventRoundStart @event, GameEventInfo info)
    {

        var value = _timeScale!.GetPrimitiveValue<float>();

        if (Math.Abs(value - 1.00f) > 0.01f)
        {
            _timeScale!.Flags &= ~ConVarFlags.FCVAR_CHEAT;
            _timeScale.SetValue(1.00f);
            _startTimeScale = 1.00f;

            Server.PrintToChatAll($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Server time scale reset to {ChatColors.Red}1");
        }

        return HookResult.Continue;
    }

    // TODO - 1 Reset the timescale to 1 when the map changing between the round and lagging when new map starts

    //[GameEventHandler(HookMode.Post)]
    //public HookResult OnEventMapChanged(Listeners.OnMapStart @event, GameEventInfo info)
    //{

    //    var value = _timeScale!.GetPrimitiveValue<float>();

    //    if (Math.Abs(value - 1.0f) > 0.01f)
    //    {

    //        _timeScale!.Flags &= ~ConVarFlags.FCVAR_CHEAT;
    //        _timeScale.SetValue(1.0f);
    //        _startTimeScale = 1.0f;

    //        Server.PrintToChatAll($" {ChatColors.Green}[TimeScale] {ChatColors.Default}Time scale reset to {ChatColors.Red}1");

    //    }

    //    return HookResult.Continue;
    //}

    public override void Unload(bool hotReload)
    {

    }
}


