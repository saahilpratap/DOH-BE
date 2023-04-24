using System.Threading.Tasks;
using LockthreatCompliance.Views;
using Xamarin.Forms;

namespace LockthreatCompliance.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
