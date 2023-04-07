using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

// TODO: remove when not using CodeMonkeysMVVM
using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.MVVM.Factories;

using DID.Infrastructure;
using DID.Infrastructure.Extensions;
using DID.Interfaces.Services;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

// TODO: remove when not using CodeMonkeysMVVM
using Activator = CodeMonkeys.Activator;

namespace XamarinSnippets
{
    public class PopupController :
        IPopupController
    {
        private static readonly ILogService _log =  LogServiceFactory.Instance.Create(
            nameof(PopupController));


        private static readonly ConcurrentDictionary<Type, Type> _registrations =
            new ConcurrentDictionary<Type, Type>();


        
        public async Task ShowAsync<TViewModel>(
            bool animate = true)
        {
            if (!TryGetRegisteredPageType<TViewModel>(
                out var pageType))
            {
                return;
            }


            var view = Activator.CreateInstance(
                pageType);


            if (!(view is PopupPage page))
            {
                return;
            }


            page.BindingContext = await ViewModelFactory.ResolveAsync<TViewModel>();


            await PopupNavigation.Instance.PushAsync(
                page,
                animate);
        }


        public async Task ShowAsync<TViewModel, TData>(
            TData data,
            bool animate = true)

            where TViewModel : class, IViewModel<TData>
        {
            if (!TryGetRegisteredPageType<TViewModel>(
                out var pageType))
            {
                return;
            }


            var view = Activator.CreateInstance(
                pageType);


            if (!(view is PopupPage page))
            {
                return;
            }


            page.BindingContext = await ViewModelFactory.ResolveAsync<TViewModel, TData>(
                data);


            await PopupNavigation.Instance.PushAsync(
                page,
                animate);
        }


        public async Task CloseAsync(
            bool animate = true)
        {
            _log?.Debug(Constants.Logging.METHOD_ENTER);

            await PopupNavigation.Instance.PopAsync(
                animate);

            _log?.Debug(Constants.Logging.METHOD_EXIT);
        }


        public async Task CloseAllAsync(
            bool animate = true)
        {
            _log?.Debug(Constants.Logging.METHOD_ENTER);

            await PopupNavigation.Instance.PopAllAsync(
                animate);

            _log?.Debug(Constants.Logging.METHOD_EXIT);
        }


        public async Task RemoveAsync<TViewModel>(
            bool animate = true)

            where TViewModel : class, IViewModel
        {
            _log?.Debug(Constants.Logging.METHOD_ENTER);

            try
            {
                if (!TryGetRegisteredPageType<TViewModel>(
                    out var pageType))
                {
                    return;
                }


                var page = PopupNavigation.Instance.PopupStack.FirstOrDefault(
                    popup => popup.GetType() == pageType);


                if (page == null)
                {
                    _log.Error($"Cannot find popup of type '{pageType.Name}' in stack.");
                    return;
                }
                                

                await PopupNavigation.Instance.RemovePageAsync(
                    page,
                    animate);
            }
            finally
            {
                _log?.Debug(Constants.Logging.METHOD_EXIT);
            }
        }


        public void Register<TViewModel, TPopupPage>()

            where TViewModel : class, IViewModel
        {
            _log?.Debug(Constants.Logging.METHOD_ENTER);

            _registrations.AddOrUpdate(
                typeof(TViewModel),
                typeof(TPopupPage),
                (viewModelType, pageType) => pageType);

            _log?.Debug(Constants.Logging.METHOD_EXIT);
        }


        private bool TryGetRegisteredPageType<TViewModelInterface>(
            out Type pageType)
        {
            if (!_registrations.TryGetValue(typeof(TViewModelInterface), out var type))
            {
                string message = $"Can't find page type for ViewModel {typeof(TViewModelInterface)}. Did you register it?";

                _log.Critical(message);
                throw new InvalidOperationException(
                    message);
            }

            pageType = type;


            return true;
        }
    }


    public interface IPopupController
    {
        void Register<TViewModel, TPopupPage>()
            where TViewModel : class, IViewModel;


        Task ShowAsync<TViewModel>(
            bool animate = true)
            where TViewModel : class, IViewModel;

        Task ShowAsync<TViewModel, TData>(
            TData data,
            bool animate = true)

            where TViewModel : class, IViewModel<TData>;


        Task CloseAsync(
            bool animate = true);

        Task CloseAllAsync(
            bool animate = true);


        Task RemoveAsync<TViewModel>(
            bool animate = true)

            where TViewModel : class, IViewModel;
    }
}