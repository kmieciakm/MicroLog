using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MircoLog.Lama.Client.Pages.Base;

public abstract class BaseComponent : ComponentBase
{
    [Inject] private ISnackbar Snackbar { get; set; }

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
}
