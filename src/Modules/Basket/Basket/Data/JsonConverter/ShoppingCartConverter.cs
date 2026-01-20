using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.JsonConverter
{
    public class ShoppingCartConverter : JsonConverter<ShoppingCart>
    {
        public override void Write(Utf8JsonWriter writer, ShoppingCart value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("id", value.Id.ToString());
            writer.WriteString("userName", value.UserName);

            writer.WritePropertyName("items");
            JsonSerializer.Serialize(writer, value.Items, options);

            writer.WriteEndObject();
        }

        public override ShoppingCart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var rootElemnt = jsonDocument.RootElement;


            var id = rootElemnt.GetProperty("id").GetGuid();
            var userName = rootElemnt.GetProperty("userName").GetString();

            var shoppingCart = ShoppingCart.Create(id, userName);
            // shoppingCart._items = []  --> initial state
            // We need to fill _items with data from JSON.
            // Direct access not allowed because ._items is private readonly 
            // and Items property is IReadOnlyList.

            var itemsElements = rootElemnt.GetProperty("items");
            // itemsElements = JsonElement representing the "items" array in JSON
            // shoppingCart._items = []  --> still empty

            var items = itemsElements.Deserialize<List<ShoppingCartItem>>(options);
            // After executing this line:
            // items = [item1, item2]  --> list of items deserialized from JSON
            // shoppingCart._items = [] --> still empty, nothing changed in the object yet

            if (items != null) // items may be null, empty, or contain data
            {
                var itemsFields = typeof(ShoppingCart)
                    .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
                // itemsFields = FieldInfo pointing to private field "_items" in ShoppingCart

                itemsFields?.SetValue(shoppingCart, items);
                // After this line:
                // shoppingCart._items = [item1, item2]  --> private field now filled
                // items = [item1, item2]  --> same as before, unchanged
            }

            return shoppingCart;
            // Returns the fully populated ShoppingCart object:
            // shoppingCart.Id = id
            // shoppingCart.UserName = userName
            // shoppingCart.Items = [item1, item2]



        }
    }
}

 