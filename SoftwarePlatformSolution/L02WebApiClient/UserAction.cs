using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace L02WebApiClient;

public record class UserAction(string Text, Delegate Action, params StackPanel[] Controls);
