using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Fxf.Shared.Models;

/// <summary>
/// Represents an option item for a selection control, such as a dropdown list or combo box.
/// </summary>
/// <remarks>
/// Use this class to define selectable options in UI components that require a value and display
/// text. The Selected property can be used to indicate whether the option is currently selected,
/// typically by setting it to a non-empty value such as "selected" for compatibility with HTML
/// select elements.
/// </remarks>
public class SelectOption
{
	/// <summary>
	/// Initializes a new instance of the SelectOption class and sets the option as selected if the
	/// specified value matches the option's value.
	/// </summary>
	/// <param name="selectedValue">
	/// The value to compare against the option's value. If not null or empty and equal to the
	/// option's value, the option will be marked as selected.
	/// </param>
	public SelectOption(string? selectedValue = null)
	{
		if(!string.IsNullOrEmpty(selectedValue) && selectedValue == Value)
		{ Selected = true; }
	}

	/// <summary>
	/// Gets or sets the currently selected item as a string.
	/// </summary>
	public bool Selected { get; set; } = false;

	/// <summary>
	/// Gets or sets the text content associated with this instance.
	/// </summary>
	public string Text { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the string value associated with this instance.
	/// </summary>
	public string Value { get; set; } = string.Empty;
}