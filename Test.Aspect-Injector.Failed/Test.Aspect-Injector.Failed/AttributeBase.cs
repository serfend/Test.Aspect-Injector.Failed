namespace Common.Attribute
{
	/// <summary>
	///
	/// </summary>
	public abstract class AttributeBase : System.Attribute
	{
		string Description { get; set; } = string.Empty;
	}
}