`BehaviorBase<T>` is a generic base class for behaviors in Xamarin.Forms<br />
However, it can probably be used in WPF and MAUI with some small API adjustments.<br />
<br />
It provides a property `AssociatedObject` that contains the current view instance the behavior is attached to.<br />
The `BindingContext` is automatically set to the `BindingContext` of the `View` if it has one.