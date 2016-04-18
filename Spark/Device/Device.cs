
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Particle
{
	public class Device
	{
		public Device ()
		{
		}

		private Object _variables;
					
		public string Id	{get;set;}				
		public string Name	{ get; set;}
		public string Last_App	{get;set;}
		public string Last_Ip_Address	{get;set;}
		public string Last_Heard	{get;set;}
		public int Product_Id	{get;set;}
		public bool Connected	{get;set;}
		public string Platform_Id	{get;set;}
		public bool Cellular	{get;set;}
		public string Cc3000_Patch_Version { get; set;}
		public string[] Functions { get; set; }

		/// <summary>
		/// Gets or sets the variables during the JSONDeserialize operation.
		/// </summary>
		/// <value>The variables blob object.</value>
		public Object Variables
		{
			get
			{
				return this._variables;
			}

			set
			{
				if (value != this._variables)
				{
					this._variables = value;
					ReBuildVariablesList ();
				}
			}
		}

		/// <summary>
		/// Gets or sets the variable list after the Variables parsing into proper Objects.
		/// </summary>
		/// <value>The parsed Variable objects list.</value>
		public List<Variable> VariableList {
			get;
			set;
		}

		// Build & Rebuild the a more usable variables list
		private void ReBuildVariablesList()
		{

			VariableList = new List<Variable> ();
			var variableContainer = (JContainer)Variables;
			foreach (var token in variableContainer.Children()) {
				if (token is JProperty)
				{
					var prop = token as JProperty;

					VariableList.Add (new Variable {
						Name = prop.Name,
						Type = prop.Value.ToString ()
					});
				}
			}
		}
	}
}

