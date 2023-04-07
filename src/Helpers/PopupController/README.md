The `PopupController` helps you handling your popups when using [Rotorgames Popup](https://github.com/rotorgames/Rg.Plugins.Popup).<br />
You can register popups to a specific view model type and afterwards show or hide them from within any view model, allowing you to respect the rules of the MVVM pattern.
<br /><br />
The following methods are available:<br />
- `Register<TViewModel, TPopup>()`
	ties a view model type to a popup page type
- `ShowAsync<TViewModel>(bool animate = true)`
	shows the popup that is tied to the given view model type, if it is registered
- `ShowAsync<TViewModel, TData>(bool animate = true)`
	shows the popup that is tied to the given view model type and passes initialization data, if it is registered
- `CloseAsync()`
	closes the current popup
- `CloseAllAsync()`
	closes all popups
- `RemoveAsync<TViewModel>(bool animate = true)`
	removes the popup that is tied to the given view model type from the stack


** NOTE: the `PopupController` has some dependencies to the MVVM framework [CodeMonkeys](https://github.com/UltimateCodeMonkeys/CodeMonkeys) **
If you do not use the framework, you will have to do some adaptions in order to resolve your view model and page instances.