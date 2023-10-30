using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using L01Reflection.Common;

namespace L01Reflection;

public class SimpleCalc : IOperations
{
    public double Add(double x, double y) => x + y;
    public double Div(double x, double y) => x / y;
    public double Mul(double x, double y) => x * y;
    public double Sub(double x, double y) => x - y;
}
