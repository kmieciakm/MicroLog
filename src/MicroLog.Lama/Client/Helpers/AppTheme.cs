using MudBlazor;

namespace MircoLog.Lama.Client.Helpers;

public class AppTheme
{
    public static MudTheme Theme = new()
    {
        Palette = new()
        {
            AppbarBackground = new("#27272f"),
            DrawerBackground = new("#27272f"),
            Background = new("#32333d"),
            Surface = new("#27272f"),
            TextPrimary = new ("#eeeeee"),
            TextSecondary = new("eeeeee"),
            ActionDefault = new("ffffff"),
            Primary = new("#0f9aa7"),
            Error = new("#de2d6d"),
            Success = new("#009d81")
        }
    };
}

