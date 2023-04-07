The `IconExtension` simplifies the use of icon fonts in Xamarin.Forms.<br />
Instead of using the `Image` view to display an icon, you can create a font from the SVGs and add it to your app.<br />
Afterwards, you copy the declaration of which code belongs to which icon into the `Icon` enum.<br />

## Usage example
```csharp
public enum Icon
{
	Info = 0xe915,
    Image = 0xe916,
    Video = 0xe917,
}
```
```xaml
<Style
	x:Key="Icon"
	TargetType="{x:Type Label}">
	<Setter Property="MinimumHeightRequest" Value="24" />
	<Setter Property="MinimumWidthRequest" Value="24" />
	<Setter Property="VerticalOptions" Value="Center" />
	<Setter Property="HorizontalOptions" Value="End" />
</Style>

<StackLayout
	Orientation="Horizontal"
	Spacing="4">

	<Label Text="Some clickable info text here..." />

	<Label
		Text="{extensions:Icon Icon=Info}"
		Style="{StaticResource Icon}" />

</StackLayout>
```