﻿// Machine generated by peg-sharp 0.4.1021.0 from 14.unconsumed2/Test14.peg.

using System;
using System.Collections.Generic;
using System.Globalization;
//using System.Linq;								// TODO: this is handy enough that we'd like to include it by default, but we want the generated parsers to work with .NET 2.0 for now
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
internal sealed partial class ParserException : Exception
{
	public ParserException(int line, int col, int offset, string file, string input, string message) : base(string.Format("{0} at line {1} col {2}{3}", message, line, col, file != null ? (" in " + file) : "."))
	{
		File = file;
		Line = line;
		Col = col;
	}
	
	public string File {get; private set;}
	public int Line {get; private set;}
	public int Col {get; private set;}
}

// Thread safe if Parser instances are not shared across threads.
internal sealed partial class Test14
{
	public Test14()
	{
		m_nonterminals.Add("Expr", new ParseMethod[]{this.DoParseExprRule});

		OnCtorEpilog();
	}
	
	public double Parse(string input)
	{
		return DoParseFile(input, null, "Expr");
	}
	
	// File is used for error reporting.
	public double Parse(string input, string file)
	{
		return DoParseFile(input, file, "Expr");
	}
	
	#region Non-Terminal Parse Methods
	// Expr := 'x'+
	private State DoParseExprRule(State _state, List<Result> _outResults)
	{
		State _start = _state;
		List<Result> results = new List<Result>();
		
			_state = DoRepetition(_state, results, 1, 2147483647,
			delegate (State s, List<Result> r) {return DoParseLiteral(s, r, "x");});
		
		if (_state.Parsed)
		{
			double value = results.Count > 0 ? results[0].Value : default(double);
			
			_outResults.Add(new Result(this, _start.Index, _state.Index - _start.Index, m_input, value));
		}
			
		return _state;
	}

	#endregion
	
	#region Private Helper Methods
	partial void OnCtorEpilog();
	partial void OnParseProlog();
	partial void OnParseEpilog(State state);
	
	private void DoBuildLineStarts()
	{
		m_lineStarts = new List<int>();
		
		m_lineStarts.Add(0);		// line 1 starts at index 0 (even if we have no text)
		
		int i = 0;
		while (i < m_input.Length)
		{
			char ch = m_input[i++];
			
			if (ch == '\r' && m_input[i] == '\n')
			{
				m_lineStarts.Add(++i);
			}
			else if (ch == '\r')
			{
				m_lineStarts.Add(i);
			}
			else if (ch == '\n')
			{
				m_lineStarts.Add(i);
			}
		}
	}
	
	private State DoChoice(State state, List<Result> results, params ParseMethod[] methods)
	{
		State start = state;
		int startResult = results.Count;
		
		foreach (ParseMethod method in methods)
		{
			State temp = method(state, results);
			if (temp.Parsed)
			{
				state = temp;
				break;
			}
			else
			{
				state = new State(start.Index, false, ErrorSet.Combine(state.Errors, temp.Errors));
				results.RemoveRange(startResult, results.Count - startResult);
			}
		}
		
		return state;
	}
	
	public string DoEscapeAll(string s)
	{
		System.Text.StringBuilder builder = new System.Text.StringBuilder(s.Length);
		
		foreach (char ch in s)
		{
			if (ch == '\n')
				builder.Append("\\n");
			
			else if (ch == '\r')
				builder.Append("\\r");
			
			else if (ch == '\t')
				builder.Append("\\t");
			
			else if (ch < ' ')
				builder.AppendFormat("\\x{0:X2}", (int) ch);
			
			else
				builder.Append(ch);
		}
		
		return builder.ToString();
	}
	
	private int DoGetCol(int index)
	{
		int start = index;
		
		while (index > 0 && m_input[index - 1] != '\n' && m_input[index - 1] != '\r')
		{
			--index;
		}
		
		return start - index;
	}
	
	// This is most often used just for error handling where it is a bit overkill.
	// However it's also sometimes used in rule prologs where efficiency is more
	// important (and doing a bit of extra work in the error case is not very harmful).
	private int DoGetLine(int index)
	{
		if (m_lineStarts == null)
			DoBuildLineStarts();
			
		int line = m_lineStarts.BinarySearch(index);
		if (line >= 0)
			return line + 1;
			
		return ~line;
	}
	
	private State DoParse(State state, List<Result> results, string nonterminal)
	{
		State start = state;
		
		CacheValue cache;
		CacheKey key = new CacheKey(nonterminal, start.Index, m_context);
		if (!m_cache.TryGetValue(key, out cache))
		{
			ParseMethod[] methods = m_nonterminals[nonterminal];
			
			int oldCount = results.Count;
			state = DoChoice(state, results, methods);
			
			bool hasResult = state.Parsed && results.Count > oldCount;
			double value = hasResult ? results[results.Count - 1].Value : default(double);
			cache = new CacheValue(state, value, hasResult);
			m_cache.Add(key, cache);
		}
		else
		{
			if (cache.HasResult)
				results.Add(new Result(this, start.Index, cache.State.Index - start.Index, m_input, cache.Value));

		}
		
		return cache.State;
	}
	
	private double DoParseFile(string input, string file, string rule)
	{
		m_file = file;
		m_input = m_file;				// we need to ensure that m_file is used or we will (in some cases) get a compiler warning
		m_input = input + "\x0";		// add a sentinel so we can avoid range checks
		m_cache.Clear();
		m_lineStarts = null;
		
		State state = new State(0, true);
		List<Result> results = new List<Result>();
		OnParseProlog();
		state = DoParse(state, results, rule);
			
		OnParseEpilog(state);
		
		return results[0].Value;
	}
	
	private State DoParseLiteral(State state, List<Result> results, string literal)
	{
		State result;
		
		if (string.Compare(m_input, state.Index, literal, 0, literal.Length) == 0)
		{
			results.Add(new Result(this, state.Index, literal.Length, m_input, default(double)));
			result = new State(state.Index + literal.Length, true, state.Errors);
		}
		else
		{
			result = new State(state.Index, false, ErrorSet.Combine(state.Errors, new ErrorSet(state.Index, literal)));
		}
		
		return result;
	}
	
	private State DoRepetition(State state, List<Result> results, int min, int max, ParseMethod method)
	{
		State start = state;
		
		int count = 0;
		while (count <= max)
		{
			State temp = method(state, results);
			if (temp.Parsed && temp.Index > state.Index)
			{
				state = temp;
				++count;
			}
			else
			{
				state = new State(state.Index, true, ErrorSet.Combine(state.Errors, temp.Errors));
				break;
			}
		}
		
		if (count < min || count > max)
			state = new State(start.Index, false, ErrorSet.Combine(start.Errors, state.Errors));
		
		return state;
	}
	
	#endregion
	
	#region Private Types
	private struct CacheKey : IEquatable<CacheKey>
	{
		public CacheKey(string rule, int index, object context)
		{
			m_rule = rule;
			m_index = index;
			m_context = context;
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			
			if (GetType() != obj.GetType())
				return false;
			
			CacheKey rhs = (CacheKey) obj;
			return this == rhs;
		}
		
		public bool Equals(CacheKey rhs)
		{
			return this == rhs;
		}
		
		public static bool operator==(CacheKey lhs, CacheKey rhs)
		{
			if (lhs.m_rule != rhs.m_rule)
				return false;
			
			if (lhs.m_index != rhs.m_index)
				return false;
			
			if (lhs.m_context != rhs.m_context)
				return false;
			
			return true;
		}
		
		public static bool operator!=(CacheKey lhs, CacheKey rhs)
		{
			return !(lhs == rhs);
		}
		
		public override int GetHashCode()
		{
			int hash = 0;
			
			unchecked
			{
				hash += m_rule.GetHashCode();
				hash += m_index.GetHashCode();
				hash += m_context.GetHashCode();
			}
			
			return hash;
		}
		
		private string m_rule;
		private int m_index;
		private object m_context;
	}
	
	private struct CacheValue
	{
		public CacheValue(State state, double value, bool hasResult)	
		{
			State = state;
			Value = value;
			HasResult = hasResult;
		}
		
		public State State;
		public double Value;
		public bool HasResult;
	}
	
	private delegate State ParseMethod(State state, List<Result> results);
	
	// These are either an error that caused parsing to fail or the reason a
	// successful parse stopped.
	private struct ErrorSet
	{
		public ErrorSet(int index, string expected)
		{
			Index = index;
			Expected = new string[]{expected};
		}
		
		public ErrorSet(int index, string[] expected)
		{
			Index = index;
			Expected = expected;
		}
		
		// The location associated with the errors. For a failed parse this will be the
		// same as State.Index. For a successful parse it will be State.Index or later.
		public int Index;
		
		// This will be the name of something which was expected, but not found.
		public string[] Expected;
		
		public static ErrorSet Combine(ErrorSet lhs, ErrorSet rhs)
		{
			if (lhs.Index > rhs.Index)
			{
				return lhs;
			}
			else if (lhs.Index < rhs.Index)
			{
				return rhs;
			}
			else
			{
				List<string> errors = new List<string>(lhs.Expected.Length + rhs.Expected.Length);
				errors.AddRange(lhs.Expected);
				foreach (string err in rhs.Expected)
				{
					if (errors.IndexOf(err) < 0)
						errors.Add(err);
				}
				return new ErrorSet(lhs.Index, errors.ToArray());
			}
		}
		
		public override string ToString()
		{
			if (Expected.Length > 0)
				return string.Format("Expected {0}", string.Join(" or ", Expected));
			else
				return "<none>";
		}
	}
	
	// The state of the parser.
	private struct State
	{
		public State(int index, bool parsed)
		{
			Index = index;
			Parsed = parsed;
			Errors = new ErrorSet(index, new string[0]);
		}
		
		public State(int index, bool parsed, ErrorSet errors)
		{
			Index = index;
			Parsed = parsed;
			Errors = errors;
		}
		
		// Index of the first unconsumed character.
		public int Index;
		
		// True if the expression associated with the state successfully parsed.
		public bool Parsed;
		
		// If Parsed is false then this will explain why. If Parsed is true it will
		// say why the parse stopped.
		public ErrorSet Errors;
	}
	
	// The result of parsing a literal or non-terminal.
	private struct Result
	{
		public Result(Test14 parser, int index, int length, string input, double value)
		{
			m_parser = parser;
			m_index = index;
			m_length = length;
			m_input = input;
			Value = value;
		}
		
		// The text which was parsed by the terminal or non-terminal.
		public string Text {get {return m_input.Substring(m_index, m_length);}}
		
		// The 0-based character index the (non)terminal started on.
		public int Index {get {return m_index;}}
		
		// The 1-based line number the (non)terminal started on.
		public int Line {get {return m_parser.DoGetLine(m_index);}}
		
		// The 1-based column number the (non)terminal started on.
		public int Col {get {return m_parser.DoGetCol(m_index);}}
			
		// For non-terminals this will be the result of the semantic action,
		// otherwise it will be the default value.
		public double Value;
		
		private Test14 m_parser;
		private int m_index;
		private int m_length;
		private string m_input;
	}
	#endregion
	
	#region Fields
	private string m_input;
	private string m_file;
	private object m_context = 0;
	private Dictionary<string, ParseMethod[]> m_nonterminals = new Dictionary<string, ParseMethod[]>();
	private Dictionary<CacheKey, CacheValue> m_cache = new Dictionary<CacheKey, CacheValue>();
	private List<int> m_lineStarts;		// offsets at which each line starts
	#endregion
}


