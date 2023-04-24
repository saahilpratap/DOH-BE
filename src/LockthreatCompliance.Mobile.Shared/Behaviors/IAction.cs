using Xamarin.Forms.Internals;

namespace LockthreatCompliance.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}