This behavior allows you to bind an `ICommand` to an event.<br />
You have to provide both the name of the event and the `ICommand` binding.<br />
You can also provide a command parameter or an `IValueConverter` that is used to convert the `EventArgs` into a structure you prefer.<br />
**Please note: if you provide both a command parameter and a converter, the defined command parameter always wins since `ICommand` only accepts one parameter by default.**
<br /><br />
### Usage example
XAML:
```xaml
<ListView ItemsSource="{Binding Pets}">
	<ListView.Behaviors>
		<behaviors:EventToCommandBehavior
			EventName="ItemAppearing"
			Command="ItemAppearingCommand"
			Converter="{StaticResource ItemVisibilityEventArgsToPet}" />
	</ListView.Behaviors>
</ListView.Behaviors>
```
Converter:
```csharp
public class ItemVisibilityEventArgsToPet : IValueConverter
{
	public object Convert(
		object value,
		object parameter,
		...)
	{
		if (!(value is ItemVisibilityEventArgs eventArgs) ||
			eventArgs.Item == null)
		{
			return null;
		}


		if (eventArgs.Item is Dog dog)
		{
			return dog;
		}
		else if (eventArgs.Item is Cat cat)
		{
			return cat;
		}
		else
		{
			return null;
		}
	}
}
```
ViewModel:
```csharp
public ICommand ItemAppearingCommand { get; set; } =
	new Command<Pet>(OnItemAppearing);

public void OnItemAppearing(
	Pet pet)
{
	// do something smart ...
}

```