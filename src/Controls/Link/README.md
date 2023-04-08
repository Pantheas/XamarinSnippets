Basically, the `Link` is just a `Label` with a `TapGestureRecognizer` on it.<br />
It allows you to bind to a `Command` without defining the `TapGestureRecognizer` in XAML everywhere you need it.<br />
So if you have a lot of tapable labels, `Link` is here to help you.
<br /><br />
### Usage example:
```xaml
<controls:Link
	Text="{Binding SomeImportantViewModelInformation}"
	Command="{Binding ShowMoreImportantInformationCommand}" />
```