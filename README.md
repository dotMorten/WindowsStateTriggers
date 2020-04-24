# WindowsStateTriggers
A collection of custom visual state triggers

## Sponsoring

If you like this library and use it a lot, consider sponsoring me. Anything helps and encourages me to keep going.

See here for details: https://github.com/sponsors/dotMorten

### NuGet

Get it on nuget:
```
   PM> Install-Package WindowsStateTriggers 
```

See more information on my blog:
http://www.sharpgis.net/post/2015/03/24/Using-Custom-Visual-State-Triggers

##### Triggers available:
- `DeviceFamilyStateTrigger`: Trigger based on the device family (Desktop, Mobile, IoT or Team)
- `NetworkConnectionStateTrigger`: Trigger if internet connection is available or not
- `OrientationStateTrigger`: Trigger based on portrait/landscape mode
- `IsTrueStateTrigger`: Trigger if a value is true - REMOVED ! use `StateTrigger` and bind to `IsActive`
- `IsFalseStateTrigger`: Trigger if a value is false
- `IsNullOrEmptyStateTrigger`: Trigger if an object is null, or if a String or IEnumerable is empty
- `IsTypePresentStateTrigger`: Trigger if a type is present (ie hardware backbutton etc)
- `EqualsStateTrigger`: Trigger if `Value` is equal to `EqualTo`
- `NotEqualStateTrigger`: Trigger if `Value` is not equal to `NotEqualTo`
- `CompareStateTrigger`: Trigger if `Value` is equal, less than or greater than `CompareTo`
- `InputTypeTrigger`: Trigger based on the `PointerType` you're using on the `TargetElement`
- `RegexStateTrigger`: This trigger evaluates a regular expression against a string and triggers if a match is found.
- `CompositeStateTrigger`: This trigger combines other triggers using, And, Or or Xor to create even more powerful triggers.
- `AdaptiveTrigger`: This trigger inherits from `AdaptiveTrigger` from Windows and adds compatibility with `CompositeStateTrigger` 

Run the test app to see a set of examples of these in use.

![windowsstatetriggers](https://cloud.githubusercontent.com/assets/1378165/7996451/483cb19e-0ad5-11e5-9be8-a41aa2127fef.gif)
