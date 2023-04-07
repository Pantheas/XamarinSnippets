The `RapidTapPreventor` wraps a function inside a mutex to prevent it from executing multiple times while the first run is not yet finished.<br />
It is useful to e.g. prevent a page from opening twice just because the user double taps on a link.<br />
The `RapidTapPreventor` has a method for both synchronous and asynchrounos functions.

### Usage example
```csharp
await RapidTapPreventor.TryExecuteAsync(
    () => _navigationService.ShowAsync<SomeViewModel>(
    data));
```