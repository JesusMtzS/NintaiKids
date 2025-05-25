using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nintai_kids.Models
{
    internal class Cuento
    {
        public enum AdvancedOperators
        {
            Multiply,
            Divide
        }

        [FunctionDescription("Adds two numbers.")]
        public float Add(float a, float b)
        {
            return a + b;
        }

        [FunctionDescription("Subtracts two numbers.")]
        public float Subtract(float a, float b)
        {
            return a - b;
        }

        [FunctionDescription("Performs advanced math operators on two numbers.")]
        public float AdvancedMath(float a, float b, AdvancedOperators advancedOperator)
        {
            return advancedOperator switch
            {
                AdvancedOperators.Multiply => a * b,
                AdvancedOperators.Divide => a / b,
                _ => throw new ArgumentOutOfRangeException(nameof(advancedOperator), advancedOperator, null)
            };
        }
    }
}
