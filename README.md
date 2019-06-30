# TreeWork
Линейный (без использования рекурсии) обход дерева: метод расширения AsTree (AsTreeWithContext) получает IEnumerable-объект со специальным итератором обхода, в качестве второго параметра используется делегат, возвращающий вложенные элементы для каждого узла.

# Sample:

``` c#
class TreeItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TreeItem> Childs { get; set; }

    public override string ToString()
    {
        return Id.ToString() + " " + Name;
    }
}

static void Main(string[] args)
{
    var heads = new List<TreeItem>
    {
        new TreeItem
        {
            Id = 1, Name = "David", Childs = new List<TreeItem>
            {
                new TreeItem
                {
                    Id = 2, Name = "Joey", Childs = new List<TreeItem>
                    {
                        new TreeItem { Id = 3, Name = "Chandler" },
                        new TreeItem { Id = 4, Name = "Richard" }
                    }
                },
                new TreeItem { Id = 5, Name = "Ross" }
            }
        },
        new TreeItem
        {
            Id = 6, Name = "Marta", Childs = new List<TreeItem>
            {
                new TreeItem
                {
                    Id = 7, Name = "Rachel", Childs = new List<TreeItem>
                    {
                        new TreeItem { Id = 8, Name = "Monica" }
                    }
                },
                new TreeItem { Id = 9, Name = "Phoebe" }
            }
        }
    };

    foreach (var item in heads.AsTree(head => head.Childs))
    {
        Console.WriteLine(string.Empty.PadLeft(item.Level, ' ') + item.Value);
    }
}

//Output:
//
//1 David
// 2 Joey
//  3 Chandler
//  4 Richard
// 5 Ross
//6 Marta
// 7 Rachel
//  8 Monica
// 9 Phoebe
```
