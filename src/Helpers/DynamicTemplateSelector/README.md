The `DynamicTemplateSelector` allows you to map a `BindingContext` type to a specific template directly in XAML.<br />
The correct template is then selected on runtime, e.g. when creating the view for a list item.

### Usage example
```xaml
<ContentPage
	...
	xmlns:controls="clr-namespace=YOUR.NAMESPACE.TO.CONTROLS.FOLDER;assembly=YOUR.CONTROLS.ASSEMBLY"
	xmlns:viewModels="clr-namespace=YOUR.NAMESPACE.TO.VIEWMODELS.FOLDER;assembly=YOUR.VIEWMODELS.ASSEMBLY">

<ContentPage.Resources>
<DataTemplate x:Key="Fallback">
	<Label Text="*** Silence ***" />
</DataTemplate>

<DataTemplate x:Key="DogTemplate">
	<Label Text="Woof!" />
</DataTemplate>

<DataTemplate x:Key="CatTemplate">
	<Label Text="Meow!" />
</DataTemplate>

<controls:DynamicTemplateSelector
	x:Key="PetTemplateSelector"
	Fallback="{StaticResource Fallback}">
	<controls:Template ItemType="{x:Type viewModels:Pets.Dog}" DataTemplate="{StaticResource DogTemplate}" />
	<controls:Template ItemType="{x:Type viewModels:Pets.Cat}" DataTemplate="{StaticResource CatTemplate}" />
</controls:DynamicTemplateSelector>
</ContentPage.Resources>

<ListView
	ItemsSource="{Binding Pets}"
	Template="{StaticResource PetTemplateSelector}" />

</ContentPage>
```