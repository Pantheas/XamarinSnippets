using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.MVVM.Factories;

using DID.Infrastructure;
using DID.Infrastructure.Extensions;
using DID.Interfaces.Services;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

// remove when not using CodeMonkeysMVVM
using Activator = CodeMonkeys.Activator;

// find interface below

namespace XamarinSnippets
{
    /// <summary>
    /// Copyright by Jan Morfeld
    /// https://github.com/Pantheas/XamarinSnippets
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    /// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// </summary>
    public class PopupController :
        IPopupController
    {
        private static readonly ILogService _log = LogServiceFactory.Instance.Create(
                nameof(PopupController));


        private static readonly ConcurrentDictionary<Type, Type> _registrations =
            new ConcurrentDictionary<Type, Type>();

        

        public async Task ShowAsync<TViewModel, TModel>(
            TModel model,
            bool animate = true)

            where TViewModel : class, IViewModel<TModel>
        {
            _log?.Debug(Constants.Logging.METHOD_ENTER);


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


            page.BindingContext = await ViewModelFactory.ResolveAsync<TViewModel, TModel>(
                model);


            await PopupNavigation.Instance.PushAsync(
                page,
                animate);


            _log?.Debug(Constants.Logging.METHOD_EXIT);
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

        public async Task RemoveAsync<TViewModelInterface>(
            bool animate = true)

            where TViewModelInterface : class, IViewModel
        {
            _log?.Debug(Constants.Logging.METHOD_ENTER);

            try
            {
                if (!TryGetRegisteredPageType<TViewModelInterface>(
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
            _log?.Debug(Constants.Logging.METHOD_ENTER);

            try
            {
                if (!_registrations.TryGetValue(typeof(TViewModelInterface), out var type))
                {
                    string message = $"Can't find page type for ViewModel {typeof(TViewModelInterface)}. Did you register it?";

                    _log.Critical(message);
                    throw new InvalidOperationException(message);
                }

                pageType = type;

                return true;
            }
            finally
            {
                _log?.Debug(Constants.Logging.METHOD_EXIT);
            }
        }
    }


    public interface IPopupController
    {
        void Register<TViewModel, TPopupPage>()
            where TViewModel : class, IViewModel;


        Task ShowAsync<TViewModel, TModel>(
            TModel mode,
            bool animate = true)

            where TViewModel : class, IViewModel<TModel>;


        Task CloseAsync(
            bool animate = true);

        Task CloseAllAsync(
            bool animate = true);


        Task RemoveAsync<TViewModel>(
            bool animate = true)

            where TViewModel : class, IViewModel;
    }
}