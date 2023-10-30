using System.Reflection;
using System.Xml;

using L01Reflection.Common;

namespace L01Reflection;

internal class Program
{
    private const string SuperCalcRelativePath =
        @"..\..\..\..\L01Reflection.SuperCalc\bin\Debug\net7.0\L01Reflection.SuperCalc.dll";

    static void Main(string[] args)
    {
        var p = new Program();
        p.Start();
    }

    private void Start()
    {
        var assembly = LoadAssembly(SuperCalcRelativePath);
        PrintAssemblyContent(assembly);
        var calcType = assembly.GetTypes()
            .Single(t => typeof(IOperations).IsAssignableFrom(t));
        object? calc = Activator.CreateInstance(calcType);
        if (calc == null)
            throw new Exception($"Cannot create an instance of the calculator");

        IOperations operations = (IOperations)calc;
        UseCalculator(operations);

        var simpleCalculator = GetCalculator("L01Reflection.SuperCalc.SuperCalc, L01Reflection.SuperCalc");
        UseCalculator(simpleCalculator);
    }


    private void PrintAssemblyContent(Assembly assembly)
    {
        Console.WriteLine($"Content of the assembly {assembly.GetName().ToString()}");
        Type[] types = assembly.GetTypes();
        int typeCounter = 0;
        foreach (var type in types)
        {
            Console.WriteLine($"Type#{typeCounter} {type.Name} ({type.Namespace})");
            Console.WriteLine($"Properties:");
            foreach (var property in type.GetProperties())
            {
                Console.WriteLine($" >{property.Name}");
            }

            Console.WriteLine($"Methods:");
            foreach (var method in type.GetMethods())
            {
                Console.WriteLine($" >{method.Name}");
            }

        }
    }

    public Assembly LoadAssembly(string assemblyFilename)
    {
        var filename = Path.GetFullPath(assemblyFilename);
        if (!File.Exists(filename))
            throw new Exception($"Cannot find a file named {filename}");

        Assembly assembly = Assembly.LoadFrom(filename);
        return assembly;
    }

    public IOperations GetCalculator(string name)
    {
        Type? calculatorType = Type.GetType(name, false);
        if (calculatorType == null)
            throw new ArgumentException($"The name {name} is not a valid type");

        object? instance = Activator.CreateInstance(calculatorType);
        if (instance == null)
            throw new Exception($"Cannot create an instance of {name}");

        return (IOperations)instance;
    }


    public void UseCalculator(IOperations calculator)
    {
        var sum = calculator.Add(2, 3);
        Console.WriteLine($"Sum is {sum}");
    }
}
