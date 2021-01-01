using System;

namespace FreeformTextParser
{
	public class TextElementDefinition
	{
		public string Name {get;set;}
		public string StartString {get;set;}
		public string EndString {get;set;}
		public string RegEx {get;set;}
		public int LineNo { get; set;}
		public bool StripExtraSpaces { get;set;}

		public TextElementDefinition()
		{
			StripExtraSpaces = true;
		}
	}
}

