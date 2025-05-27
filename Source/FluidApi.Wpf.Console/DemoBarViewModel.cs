using System.Collections.ObjectModel;
using ActiproSoftware.Windows.Controls.Bars.Mvvm;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace ActiproSoftware.Windows.Controls.Bars.Mvvm;

public class DemoBarViewModel : BarKeyedObjectViewModelBase, IHasVariantImages {

	/// <summary>
	/// Description here
	/// </summary>
	/// <remarks>Some remarks with default value.</remarks>
	/// <value>This value sets name of the bar.</value>
	public string Name { get; set; }

	/// <summary>
	/// Set the viewmodel as read-only
	/// </summary>
	/// <remarks>Default value is <c>false</c></remarks>
	public bool IsReadOnly { get; set; }
	/// <summary>
	/// Gets the collection of menu items that appear above the menu gallery in a popup.
	/// </summary>
	/// <value>The collection of menu items that appear above the menu gallery in a popup.</value>
	public ObservableCollection<object> AboveMenuItems { get; } = new();

	/// <summary>
	/// The command to execute when selecting in the bar.
	/// </summary>
	public ICommand Command { get; set; }

	public object CommandParameter { get; set; }

	public ICommand SomeOtherCommand { get; set; }
	public ImageSource LargeImageSource { get; set; }
	public ImageSource MediumImageSource { get; set; }
	public ImageSource SmallImageSource { get; set; }
}
