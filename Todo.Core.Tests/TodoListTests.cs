using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Threading.Tasks;

namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void AddIncreasesCount()
        {
            var list = new TodoList();
            _ = list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items[0].Title);
        }
        [Fact]
        public void RemoveByIdWorks()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }
        [Fact]
        public void FindReturnsMatches()
        {
            var list = new TodoList();
            _ = list.Add("Buy milk");
            _ = list.Add("Read book");
            var found = list.Find("buy").ToList();
            _ = Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }

        [Fact]
        public void SaveCreatesJsonFile()
        {
            var list = new TodoList();
            _ = list.Add("Buy milk");
            string path = Path.GetTempFileName();

            list.Save(path);
            Assert.True(File.Exists(path));
            string json = File.ReadAllText(path);
            Assert.Contains("Buy milk", json);
            File.Delete(path);
        }

        [Fact]
        public void LoadRestoresTasksFromFile()
        {
            var list = new TodoList();
            _ = list.Add("Buy milk");
            _ = list.Add("Read book");

            string path = Path.GetTempFileName();
            list.Save(path);
            var loaded = new TodoList();
            loaded.Load(path);
            Assert.Equal(2, loaded.Count);
            Assert.Contains(loaded.Items, i => i.Title == "Buy milk");
            Assert.Contains(loaded.Items, i => i.Title == "Read book");

            File.Delete(path);
        }
    }
}
