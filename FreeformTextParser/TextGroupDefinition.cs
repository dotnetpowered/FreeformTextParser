using System;
using System.Collections.Generic;

namespace FreeformTextParser
{
	public class TextGroupDefinition
	{
		public string Name { get; set; }
		public string StartString { get;set; }
		public string EndString { get;set; }
		public List<TextElementDefinition> Elements {get;set;}
		public List<TextGroupDefinition> Groups {get;set;}
		public int ElementsStartOnLine { get; set; }
		public bool RecordBreakOnLine {get;set;}

		public TextGroupDefinition()
		{
			Elements = new List<TextElementDefinition> ();
			Groups = new List<TextGroupDefinition> ();
			ElementsStartOnLine = 1;
		}
	}
}