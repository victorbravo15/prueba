using Catalog.Class;
using System.Globalization;
using System.Text.Json;
using System.Xml.Serialization;

namespace CatalogGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Rutas de los archivos de entrada y salida
            string categoriesFile = "Files\\Categories.csv";
            string productsFile = "Files\\Products.csv";
            string jsonOutputFile = desktopPath + "\\Catalog.json";
            string xmlOutputFile = desktopPath + "\\Catalog.xml";

            // Lee las categorías desde el archivo CSV
            List<Category> categories = ReadCategories(categoriesFile);

            // Lee los productos desde el archivo CSV
            List<Product> products = ReadProducts(productsFile);

            // Agrupa los productos por categoría
            GroupProductsByCategory(categories, products);

            // Genera y guarda el catálogo en formato JSON
            GenerateJSONCatalog(categories, jsonOutputFile);

            // Genera y guarda el catálogo en formato XML
            GenerateXMLCatalog(categories, xmlOutputFile);

            Console.WriteLine($"Catalog json and xml files generated successfully at {desktopPath}.");
        }

        private static List<Category> ReadCategories(string filePath)
        {
            List<Category> categories = new List<Category>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Omitir la primera línea (encabezados)
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split(';');
                    categories.Add(new Category { Id = int.Parse(data[0]), Name = data[1], Description = data[2], Products = new List<Product>() });
                }
            }

            return categories;
        }

        private static List<Product> ReadProducts(string filePath)
        {
            List<Product> products = new List<Product>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Omitir la primera línea (encabezados)
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split(';');
                    int id = int.Parse(data[0]);
                    int categoryId = int.Parse(data[1]);
                    string name = data[2];

                    // Ajusta el formato de la cadena del precio
                    decimal price = decimal.Parse(data[3].Replace(',', '.'), CultureInfo.InvariantCulture);

                    products.Add(new Product
                    {
                        Id = id,
                        CategoryId = categoryId,
                        Name = name,
                        Price = price
                    });
                }
            }

            return products;
        }

        private static void GroupProductsByCategory(List<Category> categories, List<Product> products)
        {
            foreach (var category in categories)
            {
                category.Products = products.Where(p => p.CategoryId == category.Id).ToList();
            }
        }

        private static void GenerateJSONCatalog(List<Category> categories, string filePath)
        {
            string jsonString = JsonSerializer.Serialize(categories, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, jsonString);
        }

        private static void GenerateXMLCatalog(List<Category> categories, string filePath)
        {
            // Utiliza XmlSerializer para serializar las categorías en formato XML
            XmlSerializer serializer = new XmlSerializer(typeof(List<Category>), new XmlRootAttribute("ArrayOfCategory") { Namespace = "http://schemas.datacontract.org/2004/07/Catalog" });

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, categories);
            }
        }
    }
}
