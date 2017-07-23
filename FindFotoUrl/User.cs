using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FindFotoUrl
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), CompilerGenerated]
	internal sealed class User : ApplicationSettingsBase
	{
	    public static User Default { get; } = (User)Synchronized(new User());

	    [DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return (int)this["id"];
			}
			set
			{
				this["id"] = value;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string Token
		{
			get
			{
				return (string)this["token"];
			}
			set
			{
				this["token"] = value;
			}
		}
	}
}
