using Microsoft.AspNetCore.Components;
using MircoLog.Lama.Client.Components.Dialogs;
using MircoLog.Lama.Client.Services;
using MudBlazor;
using System;

namespace MircoLog.Lama.Client.Pages.Base;

public abstract class BaseComponent : ComponentBase
{
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }

    protected void ShowSuccess(string message)
    {
        Snackbar.Add(message, Severity.Success);
    }

    protected void ShowMessage(string message)
    {
        Snackbar.Add(message, Severity.Info);
    }

    protected void ShowError(string message)
    {
        Snackbar.Add(message, Severity.Error);
    }

    protected void ShowWarningDialog(string title, string content, Action onConfirmed)
    {
        var parameters = new DialogParameters();
        parameters.Add("Action", onConfirmed);
        parameters.Add("ContentText", content);
        parameters.Add("ButtonText", "Confirm");
        parameters.Add("Color", Color.Primary);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small };

        DialogService.Show<ConfirmationDialog>(title, parameters, options);
    }

    public void ShowLogDetails(LogItem log)
    {
        var parameters = new DialogParameters();
        parameters.Add("Log", log);
        var options = new DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            Position = DialogPosition.Center
        };

        DialogService.Show<LogDetailsDialog>("Log Details", parameters, options);
    }
}
