// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state if the value is false
	/// </summary>
	public class IsFalseStateTrigger : ConditionStateTriggerBase<bool>
	{
		/// <summary>
		/// Predicate that causes the trigger to activate when satisfied.
		/// </summary>
		/// <param name="value">The value used as input to this trigger.</param>
		/// <returns>A <see cref="bool"/> indicating whether the trigger is active.</returns>
		protected override bool Condition(bool value)
		{
			return !value;
		}
	}
}
