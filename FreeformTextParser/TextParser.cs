using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace FreeformTextParser
{
    public class TextParser
	{
		System.IO.StringWriter _jsonOutputBuffer;
		string _line;
		Newtonsoft.Json.JsonWriter _writer;
		System.IO.StreamReader _reader;
		TextGroupDefinition _rootGroup;

		public TextParser (TextGroupDefinition groupDefinition)
		{
			_rootGroup = groupDefinition;
		}

		public TextParser (JsonReader groupDefinitionJson)
		{
			_rootGroup =(TextGroupDefinition) JsonSerializer.Create ().Deserialize (groupDefinitionJson, typeof(TextGroupDefinition));		
		}

		public string Parse(Stream stream)
		{
			_reader = new StreamReader (stream);
			_jsonOutputBuffer = new StringWriter ();
			_writer = new JsonTextWriter (_jsonOutputBuffer);
			_writer.Formatting = Formatting.Indented;

			ParseGroup (_rootGroup);

			return _jsonOutputBuffer.ToString ();
		}
			
		void ParseGroup (TextGroupDefinition group)
		{
			_writer.WritePropertyName (group.Name);
			if (!group.RecordBreakOnLine) 
				_writer.WriteStartObject ();
			else
				_writer.WriteStartArray ();
			int lineNo = 0;
			while (!_reader.EndOfStream) {
				if (_line == null || lineNo > 0)
					_line = _reader.ReadLine ();
				lineNo++;
				if (_line.Trim () != string.Empty && lineNo>=group.ElementsStartOnLine) {
					if (group.EndString != null && _line.Contains (group.EndString))
						break;
					if (group.RecordBreakOnLine) 
						_writer.WriteStartObject ();

					foreach (var subGroup in group.Groups) {
						if (_line.Contains (subGroup.StartString)) 
							ParseGroup (subGroup);
					}

					foreach (var element in group.Elements) {
						ParseElement (element, lineNo);
					}
					if (group.RecordBreakOnLine) 
						_writer.WriteEnd ();
				}
			}
			_writer.WriteEnd (); // object or array
		}

		void ParseElement (TextElementDefinition element, int lineNo)
		{
			string value = null;

			if (element.StartString != null && _line.Contains (element.StartString) && (element.LineNo==0 || element.LineNo == lineNo)) {
				value = _line.Substring (_line.IndexOf (element.StartString) + element.StartString.Length);
				if (element.EndString != null) {
					value = value.Substring (0, value.IndexOf (element.EndString));
				}
			} else if (element.RegEx != null && (element.LineNo==0 || element.LineNo == lineNo)) {
				var match = Regex.Match (_line, element.RegEx);
				if (match.Success)
					value = match.Value;
			} else if (element.LineNo!=0 && element.LineNo == lineNo) {
				value = _line;
			}

			if (value != null) {
				if (element.StripExtraSpaces)
					value = value.Trim ();
				_writer.WritePropertyName (element.Name);
				_writer.WriteValue (value);
			}
		}
	}
}

