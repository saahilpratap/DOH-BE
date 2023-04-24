using System.Collections.Generic;
using MvvmHelpers;
using LockthreatCompliance.Models.NavigationMenu;

namespace LockthreatCompliance.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}