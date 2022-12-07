using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02WebApiClient;

internal record class Todo(string Title, string Text, DateTime? CreatedOn = null);
