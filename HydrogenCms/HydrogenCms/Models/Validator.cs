using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HydrogenCms.Models
{
	public abstract class Validator
	{
		private StringDictionary _brokenRules = new StringDictionary();

		/// <summary>
		/// Add or remove a broken rule.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="errorMessage">The description of the error</param>
		/// <param name="isBroken">True if the validation rule is broken.</param>
		protected virtual void AddRule(string propertyName, string errorMessage, bool isBroken)
		{
			if (isBroken)
			{
				_brokenRules[propertyName] = errorMessage;
			}
			else
			{
				if (_brokenRules.ContainsKey(propertyName))
				{
					_brokenRules.Remove(propertyName);
				}
			}
		}

		/// <summary>
		/// Reinforces the business rules by adding additional rules to the 
		/// broken rules collection.
		/// </summary>
		public abstract void Validate();

		/// <summary>
		/// Gets whether the object is valid or not.
		/// </summary>
		public bool IsValid
		{
			get
			{
				Validate();
				return this._brokenRules.Count == 0;
			}
		}

		/// /// <summary>
		/// If the object has broken business rules, use this property to get access
		/// to the different validation messages.
		/// </summary>
		public virtual string ValidationMessage
		{
			get
			{
				if (!IsValid)
				{
					StringBuilder stringBuilder = new StringBuilder();

					foreach (string messages in this._brokenRules.Values)
					{
						stringBuilder.AppendLine(messages);
					}

					return stringBuilder.ToString();
				}
				else
				{
					return string.Empty;
				}
			}
		}
	}
}
