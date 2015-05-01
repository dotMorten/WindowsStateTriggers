# WindowsStateTriggers
A collection of custom visual state triggers


See more information on my blog:
http://www.sharpgis.net/post/2015/03/24/Using-Custom-Visual-State-Triggers

##### Triggers available:
- `DeviceFamilyAdaptiveTrigger`: Trigger based on the device family (Desktop or Phone)
- `NetworkConnectionStateTrigger`: Trigger if internet connection is available or not
- `OrientationStateTrigger`: Trigger based on portrait/landscape mode
- `IsTrueStateTrigger`: Trigger if a value is true
- `IsFalseStateTrigger`: Trigger if a value is false
- `IsTypePresentStateTrigger`: Trigger if a type is present (ie hardware backbutton etc)
- `EqualsStateTrigger`: Trigger if `Value` is equal to `EqualTo`
- `NotEqualStateTrigger`: Trigger if `Value` is not equal to `NotEqualTo`
- `CompareStateTrigger`: Trigger if `Value` is equal, less than or greater than `CompareTo`
- `InputTypeTrigger`: Trigger based on the `PointerType` you're using on the `TargetElement`

Run the test app to see a set of examples of these in use.


![st](https://cloud.githubusercontent.com/assets/1378165/6913751/a1573d92-d738-11e4-9c29-0e08d1762405.PNG)
