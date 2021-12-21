using MicroLog.Core;
using Microsoft.AspNetCore.Components;
using MircoLog.Lama.Client.Components.Dialogs;
using MudBlazor;

namespace MircoLog.Lama.Client.Pages.Base;

public abstract class BasePage : BaseComponent
{
    [Inject] private IDialogService DialogService { get; set; }

    public void ShowLogDetails(LogEvent _log)
    {
        var parameters = new DialogParameters();
        parameters.Add("Log", _log);
        var options = new DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            Position = DialogPosition.Center
        };

        DialogService.Show<LogDetailsDialog>("Log Details", parameters, options);
    }
}
