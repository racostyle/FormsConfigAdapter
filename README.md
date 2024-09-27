# Configuration Adapter for Windows Forms Controls

This repository demonstrates how to use the `ConfigurationAdapter` class for serializing and deserializing control values from a `Dictionary<string, string>`. The adapter handles different types of controls (such as `TextBox`, `ComboBox`, `RichTextBox`) and packs/unpacks the control values based on registered handlers.

## Features

- **Pack Controls**: Extracts control values from a parent control and stores them in a dictionary, where keys are control names (without prefixes) and values are the control's text or state.
- **Unpack Controls**: Updates controls in a form based on the values in a dictionary, using the control names (without prefixes) as keys.
- **Control Handlers**: You can register custom handlers for different control types, making the adapter easily extendable.

## How It Works

The `ConfigurationAdapter` takes in a set of control handlers (`IControlHandler`) for different types of controls (e.g., `TextBox`, `ComboBox`, etc.). It traverses through all controls in a parent form or container and uses these handlers to interact with recognized controls.

## Installation

To use the `ConfigurationAdapter`, include the `ConfigurationAdapter.cs` file and any control handlers (e.g., `Handler_TextBox.cs`) in your project.

```csharp
_adapter = new ConfigurationAdapter(
    new Handler_TextBox(),
    new Handler_ComboBox(),
    new Handler_RichTextBox()
);
```

## Usage

### Packing Controls

The `PackControls` method collects all controls recognized by the registered handlers in a parent control (e.g., a form or panel) and stores their values in a `Dictionary<string, string>`.

```csharp
var packedDict = _adapter.PackControls(this);
```

This will create a dictionary where:

- **Keys**: Control names without their prefix.
- **Values**: The text or value of each control.

### Unpacking Controls

The `UnpackControls` method takes a dictionary and applies the stored values back to the corresponding controls in a parent container.

```csharp
_adapter.UnpackControls(this, packedDict);
```

This will update the controls based on the key-value pairs in the dictionary.

### Example

Here is a full example of how to use the `ConfigurationAdapter` to pack and unpack control values:

```csharp
// Initialize the adapter with control handlers
_adapter = new ConfigurationAdapter(
    new Handler_TextBox(),
    new Handler_ComboBox(),
    new Handler_RichTextBox()
);

// Pack control values into a dictionary
var packedDict = _adapter.PackControls(this);

// Unpack control values from a dictionary
_adapter.UnpackControls(this, packedDict);
```

### Control Handlers

Each control handler must implement the `IControlHandler` interface and define:

- How to extract the value from the control.
- How to assign a value to the control.
- How to recognize whether a control matches the handler.

#### Example: TextBox Handler

```csharp
public class Handler_TextBox : IControlHandler
{
    public void AssignValueToControl(Control ctrl, string value)
    {
        ((TextBox)ctrl).Text = value;
    }

    public string GetControlValue(Control ctrl) => ((TextBox)ctrl).Text ?? "";

    public bool DoesMatchTo(Control ctrl) => ctrl is TextBox;

    public string GetControlNameWithoutPrefix(Control ctrl) =>
        new string(ctrl.Name.ToArray().SkipWhile(x => char.IsLower(x)).ToArray());
}
```

### Adding New Control Handlers

To add support for new controls, implement the `IControlHandler` interface for the specific control type and pass the handler to the `ConfigurationAdapter`:

```csharp
public class Handler_CheckBox : IControlHandler
{
    public void AssignValueToControl(Control ctrl, string value)
    {
        ((CheckBox)ctrl).Checked = value == "true";
    }

    public string GetControlValue(Control ctrl) => ((CheckBox)ctrl).Checked ? "true" : "false";

    public bool DoesMatchTo(Control ctrl) => ctrl is CheckBox;

    public string GetControlNameWithoutPrefix(Control ctrl) =>
        new string(ctrl.Name.ToArray().SkipWhile(x => char.IsLower(x)).ToArray());
}
```

### API

#### `ConfigurationAdapter`

```csharp
public ConfigurationAdapter(params IControlHandler[] acceptedHandlers)
```

**Constructor**: Takes a variable number of `IControlHandler` implementations as parameters. These handlers define how the adapter should interact with various control types.

```csharp
internal Dictionary<string, string> PackControls(Control parent)
```

**PackControls**: Packs the recognized controls from the parent container into a `Dictionary<string, string>`, where each key is the control's name without a prefix, and the value is the control's current state or text.

```csharp
internal void UnpackControls(Control parent, Dictionary<string, string> config)
```

**UnpackControls**: Unpacks the dictionary values back into the controls within the parent container.

---

#### `IControlHandler`

```csharp
public interface IControlHandler
{
    void AssignValueToControl(Control ctrl, string value);
    string GetControlValue(Control ctrl);
    bool DoesMatchTo(Control ctrl);
    string GetControlNameWithoutPrefix(Control ctrl);
}
```

**AssignValueToControl**: Assigns the given value to the control.
**GetControlValue**: Retrieves the control's value.
**DoesMatchTo**: Determines if the control matches this handler (e.g., for `TextBox`, `ComboBox`).
**GetControlNameWithoutPrefix**: Extracts the control name without its naming convention prefix.

### Naming Conventions for Controls

The `ConfigurationAdapter` assumes a specific naming convention for controls:

- **Prefixes**:
  - `tb`: TextBox
  - `rtb`: RichTextBox
  - `chb`: CheckBox
  - `cbb`: ComboBox
  - `rbtn`: RadioButton
- Example: A `TextBox` named `rtbTextBox` will be stored in the dictionary as `TextBox`, with the prefix removed.

## License

This project is licensed under the MIT License.
