// <copyright file="TodoList.cs" company="Natk">
// Copyright (c) Natk. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Todo.Core
{
    public class TodoList
    {
        private readonly List<TodoItem> itemsValue = new();
        public IReadOnlyList<TodoItem> Items => itemsValue.AsReadOnly();

        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            itemsValue.Add(item);
            return item;
        }

        public bool Remove(Guid id) => itemsValue.RemoveAll(i => i.Id == id) > 0;

        public IEnumerable<TodoItem> Find(string substring) =>
            itemsValue.Where(i => i.Title.Contains(
                substring ?? string.Empty,
                StringComparison.OrdinalIgnoreCase));

        public int Count => itemsValue.Count;

        private static readonly JsonSerializerOptions SwriteOptions = new()
        {
            WriteIndented = true,
        };
        public void Save(string path)
        {
            string jsonString = JsonSerializer.Serialize(itemsValue, SwriteOptions);
            File.WriteAllText(path, jsonString);
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Файл не найден", path);
            }

            string jsonString = File.ReadAllText(path);
            var loadedItems = JsonSerializer.Deserialize<List<TodoItem>>(jsonString);

            itemsValue.Clear();
            if (loadedItems != null)
            {
                itemsValue.AddRange(loadedItems);
            }
        }
    }
}
