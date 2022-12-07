namespace L02WebApi;

public class InMemoryStorage
{
    private List<Todo> _todoList = new List<Todo>();

    public InMemoryStorage()
    {
        var now = DateTime.Now;
        _todoList.Add(new Todo("Grocery", "Buy lettuce", now));
        _todoList.Add(new Todo("Pharmacy", "Aspirin and paracetamol", now));
        _todoList.Add(new Todo("Car", "Winter tyres", now));
        _todoList.Add(new Todo("Library", "Return the book on Q#", now));
    }


    public Task<IEnumerable<Todo>> GetByTitle(string title)
        => Task.FromResult(_todoList
                .Where(t => t.Title.Contains(title, StringComparison.InvariantCultureIgnoreCase)));

    public Task Add(string title, string text)
    {
        _todoList.Add(new Todo(title, text, DateTime.Now));
        return Task.CompletedTask;
    }

    public Task Add(Todo todo)
    {
        _todoList.Add(todo with { CreatedOn = DateTime.Now });
        return Task.CompletedTask;
    }

    public IEnumerable<Todo> All
        => (IEnumerable<Todo>)_todoList;


    public async Task Fill(int number)
    {
        var now = DateTime.Now;

        for (int i=0; i<number; i++)
        {
            var todo = new Todo($"Title{i}", $"Some text {i}", now);
            await Add(todo);
        }
    }
}
