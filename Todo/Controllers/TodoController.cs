using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;

namespace Todo.Controllers
{

        [ApiController]
        [Route("[controller]")]
        public class ItemsController : ControllerBase
        {
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(ILogger<ItemsController> logger)
        {
            _logger = logger;
        }

        private static readonly List<TodoItem> _items = new List<TodoItem>
            {
            new TodoItem { Id = 1, Time = "00:00", Title = "Item 1", Description = "Task1", IsComplete = true },
            new TodoItem { Id = 2, Time = "00:00", Title = "Item 2", Description = "Task1", IsComplete = false },
            // Предварительно заполненные элементы
            };

            // GET: /Items
            [HttpGet]
            public IEnumerable<TodoItem> Get()
            {
                _logger.LogInformation("Getting all items");
                return _items;
            }

            // GET: /Items/id
            [HttpGet("{id}")]
            public ActionResult<TodoItem> GetItem(int id)
            {
                _logger.LogInformation($"Fetching item with id {id}");
                 var item = _items.Find(i => i.Id == id);
                if (item == null)
                {
                    return NotFound();
                }
                return item;
            }

            // POST: /Items
            [HttpPost]
            public ActionResult<TodoItem> PostItem(TodoItem newItem)
            {
                // Проверяем, существует ли элемент с таким же ID.
                if (_items.Any(i => i.Id == newItem.Id))
                {
                    _logger.LogWarning($"Attempt to create a duplicate item with ID {newItem.Id}");
                    // Возвращаем статусный код 409 Conflict, если элемент с таким ID уже существует.
                    return Conflict(new { message = $"An item with ID {newItem.Id} already exists." });
                }

                // Добавляем новый элемент, если ID уникальный.
                _items.Add(newItem);
                _logger.LogInformation($"Item with ID {newItem.Id} created");
                return CreatedAtAction(nameof(GetItem), new { id = newItem.Id }, newItem);
            }


            // PUT: /Items/{id}
            [HttpPut("{id}")]
            public IActionResult PutItem(int id, TodoItem updatedItem)
            {
                var item = _items.Find(i => i.Id == id);
                if (item == null)
                {
                    return NotFound();
                }

                item.Time = updatedItem.Time;
                item.Title = updatedItem.Title;
                item.IsComplete = updatedItem.IsComplete;
                item.Description = updatedItem.Description;

                return NoContent();
            }

            // DELETE: /Items/{id}
            [HttpDelete("{id}")]
            public IActionResult DeleteItem(int id)
            {
                var itemIndex = _items.FindIndex(i => i.Id == id);
                if (itemIndex == -1)
                {
                    return NotFound();
                }

                _items.RemoveAt(itemIndex);
                return NoContent();
            }
        }
}
