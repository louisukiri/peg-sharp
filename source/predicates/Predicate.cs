// Copyright (C) 2010 Jesse Jones
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

// Base class for predicate expressions within templates.
internal abstract class Predicate
{
	protected Predicate()
	{
	}
	
	public object Evaluate(Context context)
	{
		return OnEvaluate(context);
	}
	
	public bool EvaluateBool(Context context)
	{
		object result = OnEvaluate(context);
		if (result is bool)
			return (bool) result;
		else
			throw new Exception("Expected a bool result but have " + this);
	}
	
	public string EvaluateString(Context context)
	{
		object result = OnEvaluate(context);
		if (result is string)
			return (string) result;
		else
			throw new Exception("Expected a string result but have " + this);
	}
	
	protected abstract object OnEvaluate(Context context);
}
